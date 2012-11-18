using System.Collections.Generic;

namespace Jarvis.Core
{
    public interface IJarvisService
    {
        IEnumerable<IOption> GetOptions(string term);
        string StudioUrl { get; }
        IEnumerable<IOption> GetSubOptions(IOption option);
    }
}