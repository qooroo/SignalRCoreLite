using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IClientPublisher
    {
        Task Send(string connectionId, string message);
        Task Broadcast(string message);
        
    }
}