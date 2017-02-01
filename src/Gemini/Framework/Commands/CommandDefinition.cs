using System;
using System.Windows.Media;

namespace Gemini.Framework.Commands
{
    public abstract class CommandDefinition : CommandDefinitionBase
    {
        public override ImageSource IconSource
        {
            get { return null; }
        }

        public sealed override bool IsList
        {
            get { return false; }
        }
    }
}