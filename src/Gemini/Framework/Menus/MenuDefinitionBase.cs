using System;
using System.Windows.Input;
using System.Windows.Media;
using Gemini.Framework.Commands;

namespace Gemini.Framework.Menus
{
    public abstract class MenuDefinitionBase
    {
        public abstract int SortOrder { get; }
        public abstract string Text { get; }
        public abstract ImageSource IconSource { get; }
        public abstract KeyGesture KeyGesture { get; }
        public abstract CommandDefinitionBase CommandDefinition { get; }
    }
}