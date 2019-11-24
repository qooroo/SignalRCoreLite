using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IClientResponseGateway
    {
        Task Send(string connectionId, string message);
        Task Broadcast(string message);
        
    }
}