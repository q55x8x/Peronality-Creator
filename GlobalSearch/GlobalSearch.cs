using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalSearch
{
    public class GlobalSearch
    {
        public static IDictionary<ISearchable, IList<SearchResult>> Search(List<ISearchable> searchables, string input)
        {
            Dictionary<ISearchable, IList<SearchResult>> results = new Dictionary<ISearchable, IList<SearchResult>>();

            foreach(ISearchable searchable in searchables)
            {
                results.Add(searchable, searchable.Search(searchable.GetSearchContent()));
            }

            return results;
        }
    }
}
