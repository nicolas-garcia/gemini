﻿using System;
using System.Windows.Media;

namespace Gemini.Framework.Commands
{
    public abstract class CommandDefinitionBase
    {
        public abstract string Name { get; }
        public abstract string Text { get; }
        public abstract string ToolTip { get; }
        public abstract ImageSource IconSource { get; }
        public abstract bool IsList { get; }
    }
}