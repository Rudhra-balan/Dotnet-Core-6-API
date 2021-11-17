using System.ComponentModel;

namespace SecurityAPI.Model;

public delegate void OnCompletedEventHandler(object sender, CompletedEventArgs e);

public class CompletedEventArgs : AsyncCompletedEventArgs
{
    public CompletedEventArgs(Exception ex, bool canceled, object response)
        : base(ex, canceled, response)
    {
    }
}