using System;
using System.ComponentModel.Composition;
using System.Windows.Input;
using System.Windows.Media;
using Gemini.Framework.Commands;
using Gemini.Modules.Shell.Commands;
using Gemini.Properties;

namespace Gemini.Modules.UndoRedo.Commands
{
    [CommandDefinition]
    public class UndoCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Edit.Undo";

        public override string Name
        {
            get { return CommandName; }
        }

        public override string Text
        {
            get { return Resources.EditUndoCommandText; }
        }

        public override string ToolTip
        {
            get { return Resources.EditUndoCommandToolTip; }
        }

        public override ImageSource IconSource
        {
            get { return ToolBars.Converters.BitmapImageToImageSourceConverter.Convert(Properties.Resources.undo_32x32_blue); }
        }

        [Export]
        public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<UndoCommandDefinition>(new KeyGesture(Key.Z, ModifierKeys.Control));
    }
}