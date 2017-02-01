using System;
using System.Windows.Media;

namespace Gemini.Framework.Commands
{
    public abstract class CommandListDefinition : CommandDefinitionBase
    {
        public override sealed string Text
        {
            get { return "[NotUsed]"; }
        }

        public override sealed string ToolTip
        {
            get { return "[NotUsed]"; }
        }

        public override sealed ImageSource IconSource
        {
            get { return null; }
        }

        public override sealed bool IsList
        {
            get { return true; }
        }
    }
}