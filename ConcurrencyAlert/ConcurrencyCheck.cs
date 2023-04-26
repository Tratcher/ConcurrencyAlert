using System.Diagnostics;

namespace ConcurrencyAlert;

public class ConcurrencyCheck
{
    private readonly ThreadLocal<bool> _sameThread = new ThreadLocal<bool>();
    private TaskCompletionSource? _currentOperation;

    public void Do(Action work)
    {
        var aquired = Enter();
        try
        {
            work();
        }
        finally
        {
            if (aquired) Cleanup();
        }
    }

    public T Do<T>(Func<T> work)
    {
        var aquired = Enter();
        try
        {
            return work();
        }
        finally
        {
            if (aquired) Cleanup();
        }
    }

    public async Task DoAsync(Func<Task> work)
    {
        var aquired = Enter();
        try
        {
            await work();
        }
        finally
        {
            if (aquired) Cleanup();
        }
    }

    private bool Enter()
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
                current.TrySetException(new InvalidOperationException("Concurrency Detected with\r\n" + new StackTrace(2)));
                throw new Exception("Concurrency Detected on enter");
            }
            _currentOperation = new TaskCompletionSource();
            _sameThread.Value = true;
        }
        return true;
    }

    private void Cleanup()
    {
        _sameThread.Value = false;
        lock (this)
        {
            var current = _currentOperation;
            _currentOperation = null;
            if (current!.Task.IsFaulted)
            {
                current!.Task.GetAwaiter().GetResult();
            }
        }
    }
}
