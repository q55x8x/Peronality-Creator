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
        private string value;
        private string lineText;
        private ISearchable searchable;

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

        public string Value
        {
            get
            {
                return value;
            }
        }

        public ISearchable Searchable
        {
            get
            {
                return searchable;
            }
        }

        public string LineText
        {
            get
            {
                return lineText;
            }

            set
            {
                lineText = value;
            }
        }

        public SearchResult(int line, int index, int length, string value, string lineText, ISearchable searchable)
        {
            this.line = line;
            this.index = index;
            this.length = length;
            this.value = value;
            this.lineText = lineText;
            this.searchable = searchable;
        }
    }
}
