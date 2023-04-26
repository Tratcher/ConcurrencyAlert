using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ConcurrencyAlert;

public class ConcurrencyCheck
{
    private readonly ThreadLocal<bool> _sameThread = new ThreadLocal<bool>();
    private TaskCompletionSource? _currentOperation;

    public void Do(Action work, [CallerMemberName] string callerName = "")
    {
        var aquired = Enter(callerName);
        try
        {
            work();
        }
        finally
        {
            if (aquired) Cleanup(callerName);
        }
    }

    public T Do<T>(Func<T> work, [CallerMemberName] string callerName = "")
    {
        var aquired = Enter(callerName);
        try
        {
            return work();
        }
        finally
        {
            if (aquired) Cleanup(callerName);
        }
    }

    public async Task DoAsync(Func<Task> work, [CallerMemberName] string callerName = "")
    {
        var aquired = Enter(callerName);
        try
        {
            await work();
        }
        finally
        {
            if (aquired) Cleanup(callerName);
        }
    }

    private bool Enter(string callerName)
    {
        // Ignore nested calls
        if (_sameThread.Value)
        {
            return false;
        }
        lock (this)
        {
            var current = _currentOperation;
            if (current != null)
            {
                current.TrySetException(new InvalidOperationException("Concurrency Detected with " + callerName + "\r\n" + new StackTrace(2)));
                throw new Exception("Concurrency Detected on enter: " + callerName);
            }
            _currentOperation = new TaskCompletionSource();
            _sameThread.Value = true;
        }
        return true;
    }

    private void Cleanup(string callerName)
    {
        _sameThread.Value = false;
        lock (this)
        {
            var current = _currentOperation;
            _currentOperation = null;
            if (current!.Task.IsFaulted)
            {
                throw new InvalidOperationException("Concurrency Detected on exit: " + callerName, current.Task.Exception.GetBaseException());
            }
        }
    }
}
