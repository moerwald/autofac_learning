using Autofac;
using System;

namespace AutofacAdvanced
{
    public interface ILog
    {
        void Write(string message);
    }

    public class ConsoleLog : ILog
    {
        public void Write(string message)
        {
            Console.WriteLine($"[{nameof(ConsoleLog)}] :{message}");
        }
    }

    internal interface IConsole
    {
    }

    public class EmailLog : ILog, IConsole
    {
        public void Write(string message)
        {
            Console.WriteLine($"[{nameof(EmailLog)}] :{message}");
        }
    }

    public class Engine
    {
        private ILog log;
        private int id;

        public Engine(ILog log)
        {
            this.log = log;
            this.id = new Random().Next();
        }

        public Engine(ILog log, int id)
        {
            this.log = log;
            this.id = id;
        }

        public void Ahead(int power)
        {
            this.log.Write($"Engine [{id}] ahead {power}");
        }
    }


    public class Car
    {
        private Engine engine;
        private ILog log;


        public Car(Engine engine)
        {
            this.engine = engine;
            this.log = new EmailLog();
        }

        public Car(Engine engine, ILog log)
        {
            this.engine = engine;
            this.log = log;
        }

        public void Go()
        {
            this.engine.Ahead(100);
            this.log.Write("Car going forward ...");
        }
    }

    class Program
    {
        static ContainerBuilder _builder;

     

        static void Main(string[] args)
        {
            //var registrationMethod = nameof(CallEngineCtorWithSpecificIdNumber);


            //switch (registrationMethod)
            //{
     

            //    default:
            //        throw new NotImplementedException();
            //}

            // Use the container to create the objects
            IContainer container = _builder.Build();
            var car = container.Resolve<Car>();
            car.Go();
        }
    }
}
