using System;

namespace Jarvis.Core.Events
{
    public interface IJarvisHandle<in TMessage>
    {
        void Handle(TMessage message);
    }
}