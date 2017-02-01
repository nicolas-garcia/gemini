using System.ComponentModel.Composition;
using System.Windows.Input;
using System.Windows.Media;
using Gemini.Framework.Commands;
using Gemini.Properties;

namespace Gemini.Modules.Shell.Commands
{
    [CommandDefinition]
    public class ExitCommandDefinition : CommandDefinition
    {
        public const string CommandName = "File.Exit";

        public override string Name
        {
            get { return CommandName; }
        }

        public override string Text
        {
            get { return Resources.FileExitCommandText; }
        }

        public override string ToolTip
        {
            get { return Resources.FileExitCommandToolTip; }
        }

        public override ImageSource IconSource
        {
            get { return ToolBars.Converters.BitmapImageToImageSourceConverter.Convert(Resources.disconnect_32x32_red); }
        }

        [Export]
        public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<ExitCommandDefinition>(new KeyGesture(Key.Q, ModifierKeys.Control));
    }
}