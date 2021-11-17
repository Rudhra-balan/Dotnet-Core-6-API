using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using Common.Lib.Exceptions;
using Common.Lib.ResponseHandler.Resources;
using SecurityAPI.BusinessManager.Interfaces;
using SecurityAPI.Model;

namespace SecurityAPI.BusinessManager;

public class AsynchronousSignalRBM : IAsynchronousSignalRBM
{
    #region private variable

    private const int MaximumConcurrentOperations = 1;

    //delegate will execute main worker method asynchronously
    private delegate Task WorkerEventHandler(Guid nxAdapterId, AsyncOperation asyncOp);

    //This delegate raise the event post completing the async operation.
    private readonly SendOrPostCallback _onCompletedDelegate;

    //To allow async method to call multiple time, We need to store tasks in the list
    //so we can send back the proper value back to main thread
    private readonly HybridDictionary _tasks = new();

    private static readonly SemaphoreSlim _semaphoreSlim =
        new(MaximumConcurrentOperations, MaximumConcurrentOperations);

    //Event will we captured by the main thread.
    public event OnCompletedEventHandler OnCompleted;

    #endregion

    #region Constructor

    public AsynchronousSignalRBM()
    {
        _onCompletedDelegate = CompletedDelegateFunc;
    }

    #endregion

    public void Execute(Guid requestId)
    {
        var asyncOp = AsyncOperationManager.CreateOperation(requestId);

        //Multiple threads will access the task dictionary, so it must be locked to serialze access
        lock (_tasks.SyncRoot)
        {
            if (_tasks.Contains(requestId))
                throw new ConflictException(ResponseMessage.RequestIDConflictException.message);

            _tasks[requestId] = asyncOp;
        }

        WorkerEventHandler worker = RepositoryWorker;

        //Execute process Asynchronously
        Task.Run(() => { worker.Invoke(requestId, asyncOp); });
    }

    #region Private member

    /// <summary>
    ///     This function will be called by SendOrPostCallback to raise Method1Completed Event
    /// </summary>
    /// <param name="operationState">Method1CompletedEventArgs object</param>
    private void CompletedDelegateFunc(object operationState)
    {
        var completedEventArgs = operationState as CompletedEventArgs;

        OnCompleted?.Invoke(this, completedEventArgs);
    }


    /// <summary>
    ///     This method does the actual work
    /// </summary>
    /// <param name="asyncOp"></param>
    /// <param name="request"></param>
    /// <param name="requestId"></param>
    private async Task RepositoryWorker(Guid requestId, AsyncOperation asyncOp)
    {
        try
        {
            var response = await GetSystemInfo();
            _tasks.Remove(requestId);
            var eventArgs = new CompletedEventArgs(null, false, response);
            asyncOp.PostOperationCompleted(_onCompletedDelegate, eventArgs);
        }
        catch (Exception exception)
        {
            _semaphoreSlim.Release();

            var eventArgs = new CompletedEventArgs(exception, false, exception.GetBaseException().Message);
            asyncOp.PostOperationCompleted(_onCompletedDelegate, eventArgs);
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    private async Task<string> GetSystemInfo()
    {
        Thread.Sleep(100);
        var systemInfo = new BasicInfo
        {
            OsVersion = Environment.OSVersion.ToString(),
            Os64 = Environment.Is64BitOperatingSystem,
            PcName = Environment.MachineName,
            NumberOfCpus = Environment.ProcessorCount,
            WindowsFolder = Environment.SystemDirectory,
            LogicalDrives = Environment.GetLogicalDrives()
        };

        return await Task.FromResult(systemInfo.ToString());
    }

    #endregion
}

public class BasicInfo
{
    public string OsVersion { get; set; }
    public bool Os64 { get; set; }
    public string PcName { get; set; }
    public int NumberOfCpus { get; set; }
    public string WindowsFolder { get; set; }
    public string[] LogicalDrives { get; set; }

    public override string ToString()
    {
        var output = new StringBuilder();
        output.AppendFormat("Windows version: {0}\n", OsVersion);
        output.AppendFormat("64 Bit operating system? : {0}\n",
            Os64 ? "Yes" : "No");
        output.AppendFormat("PC Name : {0}\n", PcName);
        output.AppendFormat("Number of CPUS : {0}\n", NumberOfCpus);
        output.AppendFormat("Windows folder : {0}\n", WindowsFolder);
        output.AppendFormat("Logical Drives Available : {0}\n",
            string.Join(", ", LogicalDrives)
                .Replace("\\", string.Empty));
        return output.ToString();
    }
}