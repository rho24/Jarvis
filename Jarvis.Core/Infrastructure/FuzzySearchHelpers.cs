using System.Collections.Generic;
using System.Linq;
using Jarvis.Core.Options;

namespace Jarvis.Core.Infrastructure
{
    public static class FuzzySearchHelpers
    {
        public static IEnumerable<IOption> FuzzySearch(this IEnumerable<IOption> options, string term) {
            return options.Where(o => o.Name.ToLowerInvariant().Contains(term.ToLowerInvariant()));
        }
    }
}