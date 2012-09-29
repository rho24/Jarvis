using System.Collections.Generic;

namespace Jarvis.Core
{
    public interface ISource
    {
        string Description { get; }
        IEnumerable<IItem> GetItems();
    }
}