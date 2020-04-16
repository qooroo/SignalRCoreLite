using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IServicePublisher
    {
        Task Send(string serviceId, string message);
    }
}