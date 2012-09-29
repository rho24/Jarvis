using System.Collections.Generic;
using System.Linq;

namespace Jarvis.Core
{
    public static class ItemsExtensions
    {
        public static IEnumerable<IItem> FuzzySearch(this IEnumerable<IItem> items, string term) {
            return items.Where(i => i.Name.ToLowerInvariant().Contains(term.ToLowerInvariant()));
        }
    }
}