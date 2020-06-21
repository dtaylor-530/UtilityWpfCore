﻿using System;
using System.Collections.Generic;
using System.Text;

namespace UtilityWpf.ViewModel
{
    public enum Interaction
    {
        Select, Include, Expand, Check
    }

    public enum UserCommand
    {
        Delete
    }

    public class InteractionArgs : EventArgs
    {
        public Interaction Interaction { get; set; }
        public object Value { get; set; }
    }

    public class UserCommandArgs : EventArgs
    {
        public UserCommand UserCommand { get; set; }
        public object Parameter { get; set; }
    }
}