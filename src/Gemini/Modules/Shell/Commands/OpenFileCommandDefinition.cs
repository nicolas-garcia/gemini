using System;
using System.ComponentModel.Composition;
using System.Windows.Input;
using System.Windows.Media;
using Gemini.Framework.Commands;
using Gemini.Properties;

namespace Gemini.Modules.Shell.Commands
{
    [CommandDefinition]
    public class OpenFileCommandDefinition : CommandDefinition
    {
        public const string CommandName = "File.OpenFile";

        public override string Name
        {
            get { return CommandName; }
        }

        public override string Text
        {
            get { return Resources.FileOpenCommandText; }
        }

        public override string ToolTip
        {
            get { return Resources.FileOpenCommandToolTip; }
        }

        public override ImageSource IconSource
        {
            get { return ToolBars.Converters.BitmapImageToImageSourceConverter.Convert(Resources.load_32x32_blue); }
        }

        [Export]
        public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<OpenFileCommandDefinition>(new KeyGesture(Key.O, ModifierKeys.Control));
    }
}