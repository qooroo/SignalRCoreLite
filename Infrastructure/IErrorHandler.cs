using System;

namespace Infrastructure
{
    public interface IErrorHandler
    {
        bool OnError(string agentName, Exception exception);
    }
}
