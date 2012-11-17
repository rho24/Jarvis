using System.Collections.Generic;

namespace Jarvis.Core
{
    public interface IJarvisService
    {
        IEnumerable<IItem> Items(string term);
        IEnumerable<ISource> Sources { get; }
        string StudioUrl { get; }
    }
}