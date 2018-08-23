﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    public interface ISetting
    {
        int SelectedIndex { get; set; }

        string SelectedStr { get; }

        IList<string> AllOptions { get; }

        SettingState CurrentState { get; }

        string Name { get; }


    }
    public enum SettingState
    {
        Active,
        Inactive
    }
}
