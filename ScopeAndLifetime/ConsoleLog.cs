using System;

namespace ScopeAndLifetime
{
    public interface ILog : IDisposable
    {
        void Write(string message);
    }

    interface IConsole
    {

    }

    public class ConsoleLog : ILog, IConsole
    {
        public ConsoleLog()
        {
            Console.WriteLine($"[{nameof(ConsoleLog)}]: Created @ {DateTime.Now.Ticks}");
        }
        public void Dispose()
        {
            Console.WriteLine($"[{nameof(ConsoleLog)}]: disposed");
        }

        public void Write(string message)
        {
            Console.WriteLine($"[{nameof(ConsoleLog)}]: {message}");
        }
    }
}
