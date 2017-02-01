using System;
using System.Windows.Input;
using System.Windows.Media;

namespace Gemini.Modules.MainMenu.Models
{
    public abstract class StandardMenuItem : MenuItemBase
    {
        public abstract string Text { get; }
        public abstract ImageSource IconSource { get; }
        public abstract string InputGestureText { get; }
        public abstract ICommand Command { get; }
        public abstract bool IsChecked { get; }
        public abstract bool IsVisible { get; }
    }
}