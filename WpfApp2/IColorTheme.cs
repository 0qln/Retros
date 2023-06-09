﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Retros {
    // 1 indexed
    public interface IColorTheme {
        /// <summary>
        /// Has to be the same as the class/type name!
        /// </summary>
        public string Name { get; }

        // Foreground/Text color
        public Brush FC1 { get; }
        public Brush FC2 { get; }

        // Background color
        public Brush BG1 { get; }
        public Brush BG2 { get; }
        public Brush BG3 { get; }
        public Brush BG4 { get; }
        public Brush BG5 { get; }
        public Brush BG6 { get; }

        // Accent color
        public Brush AC1 { get; }

        // Background color on hover
        public Brush BGh1 { get; } 
        public Brush BGh2 { get; }
        public Brush BGh3 { get; }

        // Border color
        public Brush BC1 { get; } 
        public Brush BC2 { get; }
        public Brush BC3 { get; }

        // Border color on hover
        public Brush BCh1 { get; }
        public Brush BCh2 { get; }
        public Brush BCh3 { get; }
    }
}
