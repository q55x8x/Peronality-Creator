using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalSearch
{
    public interface ISearchable
    {
        List<SearchResult> Search(string input, SearchCriteria operation);

        string GetSearchContent();     
    }

    [Flags]
    public enum SearchCriteria
    {
        Full_Word = 1,
        CaseSensitive = 2,
        RegularExpression = 4
    }
}
