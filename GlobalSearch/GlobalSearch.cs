using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalSearch
{
    public class GlobalSearch
    {
        public static Dictionary<ISearchable, List<SearchResult>> Search(List<ISearchable> searchables, string input, SearchCriteria criteria)
        {
            Dictionary<ISearchable, List<SearchResult>> results = new Dictionary<ISearchable, List<SearchResult>>();

            foreach (ISearchable searchable in searchables)
            {
                List<SearchResult> fileResults = searchable.Search(input, criteria);

                if (results.Count > 0)
                {
                    results.Add(searchable, fileResults);
                }
            }

            return results;
        }
    }
}
