using System;

namespace AutofacImplicitRelationshipTypes
{
    class ConsoleLog : ILog
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
