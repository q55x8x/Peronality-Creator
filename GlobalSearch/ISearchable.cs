using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalSearch
{
    public interface ISearchable
    {
        IList<SearchResult> Search(string input);

        string GetSearchContent();     
    }
}
