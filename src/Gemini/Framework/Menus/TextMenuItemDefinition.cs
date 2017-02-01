using System;
using System.Windows.Input;
using System.Windows.Media;
using Gemini.Framework.Commands;

namespace Gemini.Framework.Menus
{
    public class TextMenuItemDefinition : MenuItemDefinition
    {
        private readonly string _text;
        private readonly ImageSource _iconSource;

        public override string Text
        {
            get { return _text; }
        }

        public override ImageSource IconSource
        {
            get { return _iconSource; }
        }

        public override KeyGesture KeyGesture
        {
            get { return null; }
        }

        public override CommandDefinitionBase CommandDefinition
        {
            get { return null; }
        }

        public TextMenuItemDefinition(MenuItemGroupDefinition group, int sortOrder, string text, ImageSource iconSource = null)
            : base(group, sortOrder)
        {
            _text = text;
            _iconSource = iconSource;
        }
    }
}