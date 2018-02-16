using Autofac;
using System;
using System.Collections.Generic;

namespace AutofacSimple
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

        static void SimpleRegistration()
        {
            _builder = new ContainerBuilder();

            // Register EmailLog via both interfaces
            _builder.RegisterType<EmailLog>().As<ILog>().As<IConsole>()
                .AsSelf(); // Make EmailLog resolvable via container.Resolve<EmailLog>()

            _builder.RegisterType<ConsoleLog>().As<ILog>()
                // EMail is used as default. Autofac normally sets the last registered type
                // as default one. With below command we can change this behaviour
                .PreserveExistingDefaults();

            _builder.RegisterType<Engine>();
            _builder.RegisterType<Car>();
        }


        static void SimpleRegistration_UseSpecialCarCTOR()
        {
            _builder = new ContainerBuilder();

            // Register EmailLog via both interfaces
            _builder.RegisterType<EmailLog>().As<ILog>().As<IConsole>()
                .AsSelf(); // Make EmailLog resolvable via container.Resolve<EmailLog>()

            _builder.RegisterType<ConsoleLog>().As<ILog>();
            _builder.RegisterType<Engine>();
            _builder.RegisterType<Car>()
                // We want that the CTOR with one parameter is called @ construction.
                // By default Autofac resolves the CTOR with the MOST parameters
                .UsingConstructor(typeof(Engine));
        }

        static void RegisterInstancesOfComponents()
        {
            _builder = new ContainerBuilder();
            _builder.RegisterType<EmailLog>().As<ILog>();

            var log = new ConsoleLog();
            _builder.RegisterInstance(log); // Always return this instance
            _builder.RegisterType<ConsoleLog>().As<ILog>();

            _builder.RegisterType<Engine>();

            _builder.RegisterType<Car>()
               // We want that the CTOR with one parameter is called @ construction.
               // By default Autofac resolves the CTOR with the MOST parameters
               .UsingConstructor(typeof(Engine));
        }

        static void CallEngineCtorWithSpecificIdNumber()
        {
            _builder = new ContainerBuilder();

            // Register EmailLog via both interfaces
            _builder.RegisterType<ConsoleLog>().As<ILog>();

            // context is able to resolve type -> as the container
            _builder.Register(context => new Engine(context.Resolve<ILog>(), 42));
            _builder.RegisterType<Car>();
        }

        static void RegisterGenerics()
        {
            _builder = new ContainerBuilder();
            // Register an open generic (= <>) @ the container
            _builder.RegisterGeneric(typeof(List<>)).As(typeof(IList<>));

            // Resolve it via container.Resolve<IList<int>>();
        }

        static void Main(string[] args)
        {
            var registrationMethod = nameof(CallEngineCtorWithSpecificIdNumber);


            switch (registrationMethod)
            {
                case nameof(SimpleRegistration):
                    SimpleRegistration();
                    break;

                case nameof(SimpleRegistration_UseSpecialCarCTOR):
                    SimpleRegistration_UseSpecialCarCTOR();
                    break;

                case nameof(RegisterInstancesOfComponents):
                    RegisterInstancesOfComponents();
                    break;

                case nameof(CallEngineCtorWithSpecificIdNumber):
                    CallEngineCtorWithSpecificIdNumber();
                    break;

                default:
                    throw new NotImplementedException();
            }

            // Use the container to create the objects
            IContainer container = _builder.Build();
            var car = container.Resolve<Car>();
            car.Go();
        }
    }
}
