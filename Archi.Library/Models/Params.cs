using System;
using System.Collections.Generic;
using System.Text;

namespace Archi.Library.Models
{
    public class Params
    {
        public string Asc { get; set; }
        public string Desc { get; set; }
        public string Range { get; set; }
        public bool HasAscOrder()
        {
            return !string.IsNullOrWhiteSpace(Asc);
        }

        public bool HasDescOrder()
        {
           return !string.IsNullOrWhiteSpace(Desc);
        }
        public bool HasRange()
        {
            return !string.IsNullOrWhiteSpace(Range);
        }

    }
}