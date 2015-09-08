using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalSearch
{
    public class SearchResult
    {
        private int line;
        private int index;
        private int length;
        private int value;

        public int Line
        {
            get
            {
                return line;
            }
        }

        public int Index
        {
            get
            {
                return index;
            }
        }

        public int Length
        {
            get
            {
                return length;
            }
        }

        public int Value
        {
            get
            {
                return value;
            }
        }

        public SearchResult(int line, int index, int length, int value)
        {
            this.line = line;
            this.index = index;
            this.length = length;
            this.value = value;
        }
    }
}
