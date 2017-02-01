using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;

namespace Gemini.Modules.StatusBar.Views
{
    /// <summary>
    ///     Interaction logic for StatusBarView.xaml
    /// </summary>
    public partial class StatusBarView : UserControl
    {
        private Grid statusBarGrid;
        private bool listenerRegistred;

        public StatusBarView()
        {
            this.InitializeComponent();
        }

        private void OnStatusBarGridLoaded(object sender, RoutedEventArgs e)
        {
            this.statusBarGrid = (Grid) sender;
            this.RefreshGridColumns();
        }

        private void RefreshGridColumns()
        {
            if (this.statusBarGrid == null)
                return;
            this.statusBarGrid.ColumnDefinitions.Clear();
            foreach (var item in this.StatusBar.Items.Cast<BaseStatusBarItem>())
                this.statusBarGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = item.Width });
            if (this.listenerRegistred)
                return;
            var collection = this.StatusBar.ItemsSource as IObservableCollection<BaseStatusBarItem>;
            if (collection == null)
                return;
            this.listenerRegistred = true;
            collection.PropertyChanged += this.collection_PropertyChanged;
        }

        private void collection_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.RefreshGridColumns();
        }
    }
}
