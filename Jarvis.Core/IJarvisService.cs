using System.Collections.Generic;

namespace Jarvis.Core
{
    public interface IJarvisService
    {
        IEnumerable<IItem> Items();
        IEnumerable<ISource> Sources { get; }
    }
}