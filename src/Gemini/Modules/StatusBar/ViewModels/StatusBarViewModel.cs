using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;

namespace Gemini.Modules.StatusBar.ViewModels
{
    [Export(typeof(IStatusBar))]
    public class StatusBarViewModel : PropertyChangedBase, IStatusBar
    {
        private readonly StatusBarItemCollection items;
        public IObservableCollection<BaseStatusBarItem> Items
        {
            get { return this.items; }
        }
        
        private readonly Dictionary<EStatusBarType, Func<string, object, GridLength, Dock, BaseStatusBarItem>> itemsFactory;

        public StatusBarViewModel()
        {
            this.items = new StatusBarItemCollection();
            this.itemsFactory = new Dictionary<EStatusBarType, Func<string, object, GridLength, Dock, BaseStatusBarItem>>()
                                {
                                    {
                                        EStatusBarType.Text,
                                        (name, content, width, dock) =>
                                            new TextStatusBarViewModel(
                                                name, content, width,
                                                dock)
                                    },
                                    {
                                        EStatusBarType.ProgressBar,
                                        (name, content, width, dock)
                                            =>
                                                new ProgressBarStatusBarViewModel
                                                (name, content,
                                                 width, dock)
                                    },
                                    {
                                        EStatusBarType.Button,
                                        (name, content, width, dock)
                                            =>
                                                new ButtonStatusBarViewModel
                                                (name, content,
                                                 width, dock)
                                    }
                                };
        }

        public bool AddItem(string name, dynamic content, GridLength width, Dock dock = Dock.Left, EStatusBarType type = EStatusBarType.Text)
        {
            if (this.items.Any(e => string.Equals(e.Name, name, StringComparison.InvariantCultureIgnoreCase)))
                return false;
            if (!this.itemsFactory.ContainsKey(type))
                return false;
            BaseStatusBarItem item = this.itemsFactory[type](name, content, width, dock);
            if (item == null)
                return false;
            this.Items.Add(item);
            return true;
        }

        public void HideItem(string name)
        {
            var item = this.Items.FirstOrDefault(e => string.Equals(name, e.Name, StringComparison.InvariantCultureIgnoreCase));
            if (item == null)
                return;
            item.Visibility = Visibility.Collapsed;
        }

        public void ShowItem(string name)
        {
            var item = this.Items.FirstOrDefault(e => string.Equals(name, e.Name, StringComparison.InvariantCultureIgnoreCase));
            if (item == null)
                return;
            item.Visibility = Visibility.Visible;
        }

        public void UpdateItemContent(string name, object value)
        {
            var item = this.Items.FirstOrDefault(e => string.Equals(name, e.Name, StringComparison.InvariantCultureIgnoreCase));
            if (item == null)
                return;
            item.Content = value;
        }

        private class StatusBarItemCollection : BindableCollection<BaseStatusBarItem> {}
    }
}
