using System;

namespace AutofacImplicitRelationshipTypes
{
    public class ConsoleLog : ILog
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
