using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;

namespace Gemini.Modules.StatusBar
{
    /// <summary>
    ///     List of availables statusbar items type
    /// </summary>
    public enum EStatusBarType
    {
        Text,
        ProgressBar,
        Button
    }

    /// <summary>
    ///     This interface controls how status bar works.
    ///     You can add/remove/update items from bar using its methods.
    /// </summary>
    public interface IStatusBar
    {
        /// <summary>
        ///     List of all items in the statusbar. You should not update the items list from here, use methods instead.
        /// </summary>
        IObservableCollection<BaseStatusBarItem> Items { get; }

        /// <summary>
        ///     Add a new item to the statusbar if it do not already exists.
        ///     According to the type of items you will create, the content parameter should contains differents information.
        ///     In case of :
        ///     - Text : the string to display. string type
        ///     - ProgressBar: the value of the progressbar. double value. You can set to double.NaN if you want an indeterminated
        ///     progress.
        ///     - Button: the most complex type. (System.Colletion.Generic.Dictionary&lt;string, object&gt;) A dictionary is
        ///     expected with the following structure:
        ///     ["ImageSource"]: (System.Windows.Media.ImageSource) - the image to display as icon
        ///     ["Tooltip"]: (string) - the string to display as tooltip
        ///     ["ButtonAction"]: (System.Windows.Input.ICommand) - the command to use on this button
        /// </summary>
        /// <param name="name">This parameter is used like a key for items. It must be unique.</param>
        /// <param name="content">Item content given to its constructor for filling its content.</param>
        /// <param name="width">Item width in the bar</param>
        /// <param name="dock">The position to dock in status bar</param>
        /// <param name="type">Type of item to create</param>
        /// <returns>False if the creation failed. True instead. Creation can fail if the given name is already created.</returns>
        bool AddItem(string name, dynamic content, GridLength width, Dock dock = Dock.Left, EStatusBarType type = EStatusBarType.Text);

        /// <summary>
        ///     Hide the item
        /// </summary>
        /// <param name="name">name of the item to hide</param>
        void HideItem(string name);

        /// <summary>
        ///     Show the item
        /// </summary>
        /// <param name="name">name of the item to show</param>
        void ShowItem(string name);

        /// <summary>
        ///     Update the content of the given item
        /// </summary>
        /// <param name="name">name of the item to update</param>
        /// <param name="value">
        ///     value to use for update.
        ///     According to the type of the item, the value parameter should contains differents information.
        ///     In case of :
        ///     - Text : the string to display. string type
        ///     - ProgressBar: the value of the progressbar. double value. You can set to double.NaN if you want an indeterminated
        ///     progress.
        ///     - Button: the most complex type. (System.Colletion.Generic.Dictionary&lt;string, object&gt;) A dictionary is
        ///     expected with the following structure:
        ///     ["ImageSource"]: (System.Windows.Media.ImageSource) - the image to display as icon
        ///     ["Tooltip"]: (string) - the string to display as tooltip
        ///     ["ButtonAction"]: (System.Windows.Input.ICommand) - the command to use on this button
        ///     In case of update, all keys are optional
        /// </param>
        void UpdateItemContent(string name, object value);
    }
}
