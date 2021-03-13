using System.Threading;

namespace ToolsLib.Interfaces
{
    public interface IRunnableServer
    {
        void Run(IMessageHandler handler, CancellationToken cancellationToken);
    }
}
