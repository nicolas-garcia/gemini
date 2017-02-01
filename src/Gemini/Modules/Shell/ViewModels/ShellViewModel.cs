using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Services;
using Gemini.Framework.Themes;
using Gemini.Modules.MainMenu;
using Gemini.Modules.Shell.Services;
using Gemini.Modules.Shell.Views;
using Gemini.Modules.StatusBar;
using Gemini.Modules.ToolBars;

namespace Gemini.Modules.Shell.ViewModels
{
    [Export(typeof(IShell))]
    public class ShellViewModel : Conductor<IDocument>.Collection.OneActive, IShell
    {
        public event EventHandler ActiveDocumentChanging;
        public event EventHandler ActiveDocumentChanged;

#pragma warning disable 649
        [ImportMany(typeof(IModule))]
        private IEnumerable<IModule> _modules;

        [Import]
        private IThemeManager _themeManager;

        [Import]
        private IMenu _mainMenu;

        [Import]
        private IToolBars _toolBars;

        [Import]
        private IStatusBar _statusBar;

        [Import]
        private ILayoutItemStatePersister _layoutItemStatePersister;
#pragma warning restore 649

        private IShellView _shellView;
        private bool _closing;

        public IMenu MainMenu
        {
            get { return this._mainMenu; }
        }

        public IToolBars ToolBars
        {
            get { return this._toolBars; }
        }

        public IStatusBar StatusBar
        {
            get { return this._statusBar; }
        }

        private ILayoutItem _activeLayoutItem;
        public ILayoutItem ActiveLayoutItem
        {
            get { return this._activeLayoutItem; }
            set
            {
                if (ReferenceEquals(this._activeLayoutItem, value))
                    return;

                this._activeLayoutItem = value;

                if (value is IDocument)
                    this.ActivateItem((IDocument) value);

                this.NotifyOfPropertyChange(() => this.ActiveLayoutItem);
            }
        }

        private readonly BindableCollection<ITool> _tools;
        public IObservableCollection<ITool> Tools
        {
            get { return this._tools; }
        }

        public IObservableCollection<IDocument> Documents
        {
            get { return this.Items; }
        }

        private bool _showFloatingWindowsInTaskbar;
        public bool ShowFloatingWindowsInTaskbar
        {
            get { return this._showFloatingWindowsInTaskbar; }
            set
            {
                this._showFloatingWindowsInTaskbar = value;
                this.NotifyOfPropertyChange(() => this.ShowFloatingWindowsInTaskbar);
                if (this._shellView != null)
                    this._shellView.UpdateFloatingWindows();
            }
        }

        public virtual string StateFile
        {
            get { return @".\ApplicationState.bin"; }
        }

        public bool HasPersistedState
        {
            get { return File.Exists(this.StateFile); }
        }

        public ShellViewModel()
        {
            ((IActivate) this).Activate();

            this._tools = new BindableCollection<ITool>();
        }

        protected override void OnViewLoaded(object view)
        {
            foreach (var module in this._modules)
                foreach (var globalResourceDictionary in module.GlobalResourceDictionaries)
                    Application.Current.Resources.MergedDictionaries.Add(globalResourceDictionary);

            foreach (var module in this._modules)
                module.PreInitialize();
            foreach (var module in this._modules)
                module.Initialize();

            // If after initialization no theme was loaded, load the default one
            if (_themeManager.CurrentTheme == null)
            {
                if (!_themeManager.SetCurrentTheme(Properties.Settings.Default.ThemeName))
                {
                    Properties.Settings.Default.ThemeName = (string)Properties.Settings.Default.Properties["ThemeName"].DefaultValue;
                    Properties.Settings.Default.Save();
                    if (!_themeManager.SetCurrentTheme(Properties.Settings.Default.ThemeName))
                    {
                        throw new InvalidOperationException("unable to load application theme");
                    }
                }
            }

            _shellView = (IShellView)view;
            if (!_layoutItemStatePersister.LoadState(this, _shellView, StateFile))
            {
                this.LoadDefaultLayout();
            }

            foreach (var module in this._modules)
                module.PostInitialize();

            base.OnViewLoaded(view);
        }

        public void ShowTool<TTool>() where TTool : ITool
        {
            this.ShowTool(IoC.Get<TTool>());
        }

        public void ShowTool(ITool model)
        {
            if (this.Tools.Contains(model))
                model.IsVisible = true;
            else
                this.Tools.Add(model);
            model.IsSelected = true;
            this.ActiveLayoutItem = model;
            model.Activate();
        }

        public void OpenDocument(IDocument model)
        {
            this.ActivateItem(model);
        }

        public void CloseDocument(IDocument document)
        {
            this.DeactivateItem(document, true);
        }

        private bool _activateItemGuard = false;

        public override void ActivateItem(IDocument item)
        {
            if (_closing || _activateItemGuard)
                return;

            _activateItemGuard = true;

            try
            {
                if (ReferenceEquals(item, ActiveItem))
                    return;

                RaiseActiveDocumentChanging();

                var currentActiveItem = ActiveItem;

                base.ActivateItem(item);

                this.RaiseActiveDocumentChanged();
            }
            finally
            {
                _activateItemGuard = false;
            }
        }

        private void RaiseActiveDocumentChanging()
        {
            var handler = this.ActiveDocumentChanging;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private void RaiseActiveDocumentChanged()
        {
            var handler = this.ActiveDocumentChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        protected override void OnActivationProcessed(IDocument item, bool success)
        {
            if (!ReferenceEquals(this.ActiveLayoutItem, item))
                this.ActiveLayoutItem = item;

            base.OnActivationProcessed(item, success);
        }

        public override void DeactivateItem(IDocument item, bool close)
        {
            this.RaiseActiveDocumentChanging();

            base.DeactivateItem(item, close);

            this.RaiseActiveDocumentChanged();
        }

        protected override void OnDeactivate(bool close)
        {
            // Workaround for a complex bug that occurs when
            // (a) the window has multiple documents open, and
            // (b) the last document is NOT active
            // 
            // The issue manifests itself with a crash in
            // the call to base.ActivateItem(item), above,
            // saying that the collection can't be changed
            // in a CollectionChanged event handler.
            // 
            // The issue occurs because:
            // - Caliburn.Micro sees the window is closing, and calls Items.Clear()
            // - AvalonDock handles the CollectionChanged event, and calls Remove()
            //   on each of the open documents.
            // - If removing a document causes another to become active, then AvalonDock
            //   sets a new ActiveContent.
            // - We have a WPF binding from Caliburn.Micro's ActiveItem to AvalonDock's
            //   ActiveContent property, so ActiveItem gets updated.
            // - The document no longer exists in Items, beacuse that collection was cleared,
            //   but Caliburn.Micro helpfully adds it again - which causes the crash.
            //
            // My workaround is to use the following _closing variable, and ignore activation
            // requests that occur when _closing is true.
            this._closing = true;

            this._layoutItemStatePersister.SaveState(this, this._shellView, this.StateFile);

            base.OnDeactivate(close);
        }

        public void Close()
        {
            Application.Current.MainWindow.Close();
        }

        public void CloseTool(ITool model)
        {
            if (!this.Tools.Contains(model))
                return;
            model.Deactivate(true);
            if (model.CloseCommand != null)
                model.CloseCommand.Execute(null);
            this.Tools.Remove(model);
        }

        public void HideTool(ITool model)
        {
            if (!this.Tools.Contains(model))
                return;
            model.IsVisible = false;
            model.Deactivate(false);
        }

        private void LoadDefaultLayout()
        {
            foreach (var defaultDocument in this._modules.SelectMany(x => x.DefaultDocuments))
                this.OpenDocument(defaultDocument);
            foreach (var defaultTool in this._modules.SelectMany(x => x.DefaultTools))
                this.ShowTool((ITool) IoC.GetInstance(defaultTool, null));
        }
    }
}
