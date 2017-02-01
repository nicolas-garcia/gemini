using System;
using System.ComponentModel.Composition;
using System.Windows.Input;
using System.Windows.Media;
using Gemini.Framework.Commands;
using Gemini.Properties;

namespace Gemini.Modules.UndoRedo.Commands
{
    [CommandDefinition]
    public class RedoCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Edit.Redo";

        public override string Name
        {
            get { return CommandName; }
        }

        public override string Text
        {
            get { return Resources.EditRedoCommandText; }
        }

        public override string ToolTip
        {
            get { return Resources.EditRedoCommandToolTip; }
        }

        public override ImageSource IconSource
        {
            get { return ToolBars.Converters.BitmapImageToImageSourceConverter.Convert(Properties.Resources.redo_32x32_blue); }
        }

        [Export]
        public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<RedoCommandDefinition>(new KeyGesture(Key.Y, ModifierKeys.Control));
    }
}