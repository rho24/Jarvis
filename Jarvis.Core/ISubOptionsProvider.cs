using System.Collections.Generic;

namespace Jarvis.Core
{
    public interface ISubOptionsProvider
    {
        bool CanSupport(IOption option);
        IEnumerable<IOption> CreateSubOptions(IOption option);
    }
}