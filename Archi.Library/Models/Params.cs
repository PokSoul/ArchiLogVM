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
        public string SearchKey { get; set; }
        public string SearchValue { get; set; }

        public bool HasSearch() {
            return !string.IsNullOrEmpty(SearchKey) && !string.IsNullOrEmpty(SearchValue);
        }
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