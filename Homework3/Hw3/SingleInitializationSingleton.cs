using System;
using System.Threading;

namespace Hw3.Tests;

public class SingleInitializationSingleton
{
    private static readonly object Locker = new();

    private static volatile bool _isInitialized;

    public const int DefaultDelay = 3_000;
    
    public int Delay { get; }

    private static Lazy<SingleInitializationSingleton> _instanse = new(() => new SingleInitializationSingleton());
    private SingleInitializationSingleton(int delay = DefaultDelay)
    {
        Delay = delay;
        // imitation of complex initialization logic
        Thread.Sleep(delay);
    }

    public static void Reset()
    {
        if(!_isInitialized)
            return;
        if (_isInitialized)
        {
            lock (Locker)
            {
                _instanse = new(() => new());
                _isInitialized = false;   
            }   
        }
    }

    public static void Initialize(int delay)
    {
        if (!_isInitialized)
        {
            lock (Locker)
            {
                if (!_isInitialized)
                {
                    _instanse = new(() => new (delay));
                    _isInitialized = true;
                    return;
                }
            }
        }
        throw new InvalidOperationException("Singleton has already been initialized");
    }

    public static SingleInitializationSingleton Instance
    {
        get {
            lock (Locker)
            {
                return _instanse.Value;   
            }
        }
    }
}