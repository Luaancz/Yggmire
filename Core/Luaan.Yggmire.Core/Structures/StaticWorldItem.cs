﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luaan.Yggmire.Core.Structures
{
    [Serializable]
    public class StaticWorldItem : WorldItem
    {
        public Position2 Position { get; set; }
    }
}