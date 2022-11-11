﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArthurExam
{
    public class Result
    {
        public string? imageUrl { private get; set; }
        public string? sourceUrl { private get; set; }
        public int depth { private get; set; }

        public override string ToString()
        {
            return "imageUrl: " + imageUrl + " sourceUrl: " + sourceUrl + " depth: " + depth + "\n";
        }

    }
}
