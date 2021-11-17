using SecurityAPI.Model;

namespace SecurityAPI.BusinessManager.Interfaces;

public interface IAsynchronousSignalRBM
{
    public event OnCompletedEventHandler OnCompleted;
    void Execute(Guid requestId);
}