using Autofac;
using Autofac.Core;
using System;
using System.Reflection;

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

    public class SmsLog : ILog
    {

        private string phoneNumber;


        public SmsLog(string phoneNumber)
        {
            this.phoneNumber = phoneNumber;
        }

        public void Write(string message)
        {
            Console.WriteLine($"[{nameof(SmsLog)}]: SMS to {phoneNumber}: {message}");
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

    class Parent
    {
        public override string ToString()
        {
            return "I am your father";
        }
    }

    class Child
    {
        public string Name { get; set; }
        public Parent Parent { get; set; }

        public void SetParent(Parent prnt)
        {
            Parent = prnt;
        }
    }


    class Program
    {
        static ContainerBuilder _builder;

        static void ProvideDynamicArguments()
        {
            _builder = new ContainerBuilder();

            // Option 1: named parameter
            //_builder.RegisterType<SmsLog>().As<ILog>()
            //    .WithParameter("phoneNumber", "+123456789");

            // Option 2: typed parameter
            //_builder.RegisterType<SmsLog>().As<ILog>()
            //    .WithParameter(new TypedParameter(typeof(string), "+123456789"));

            // Option 3: resolved parameter
            //_builder.RegisterType<SmsLog>().As<ILog>()
            //    .WithParameter(
            //        new ResolvedParameter(
            //            // predicate
            //            (propertyInfo, context) => propertyInfo.ParameterType == typeof(string) && propertyInfo.Name == "phoneNumber"
            //            // value accessor
            //            , (pi, ctx) => "+123456789"
            //        )
            //    );

            Random random = new Random();
            var paramName = "phoneNumber";
            _builder.Register((c, p) => new SmsLog(p.Named<string>(paramName)))
                .As<ILog>();

            var container = _builder.Build();
            var log = container.Resolve<ILog>(new NamedParameter(paramName, random.Next().ToString()));
            log.Write("TEST");
        }

        static void InjectParentDependencyViaProperty(uint option = 0)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Parent>();


            switch (option)
            {
                case 0:
                    // Container automatically sets depending properties
                    builder.RegisterType<Child>().PropertiesAutowired();
                    break;

                case 1:
                    builder.RegisterType<Child>().WithProperty(nameof(Child.Parent), new Parent());
                    break;

                case 2:
                    // Method injection -> Inject dependency @ resolve time
                    builder.Register(ctx => {
                        var child = new Child();
                        child.SetParent(ctx.Resolve<Parent>());
                        return child;
                    });
                    break;

                case 3:
                    // Activating event handler -> OnActivated is called when
                    // someone call container.Resolve<Child>();
                    builder.RegisterType<Child>()
                        .OnActivated(e => {
                            var p = e.Context.Resolve<Parent>();
                            e.Instance  // Child instance to be build
                            .SetParent(p);
                        });

                    break;

                default:
                    break;
            }

            var container = builder.Build();

            var parent = container.Resolve<Child>().Parent;
            Console.WriteLine(parent);
        }

        static void BulkRegistration()
        {
            // Get the assembly
            var assembly = Assembly.GetExecutingAssembly();
            var builder = new ContainerBuilder();


            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith("Log"))  // Get all log classes
                .Except<SmsLog>()
                .Except<ConsoleLog>(c => c.As<ILog>().SingleInstance())
                .AsSelf();

            // Get all classes (except SmsLog) that end with Log 
            // and register the first log-class
            builder.RegisterAssemblyTypes(assembly)
                .Except<SmsLog>()
                .Where(t => t.Name.EndsWith("Log"))
                .As(testc => testc.GetInterfaces()[0]);
        }


        class ParentChildeModule : Autofac.Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterType<Parent>();
                builder.Register(c => new Child() { Parent = c.Resolve<Parent>() });
            }
        }

        static void ModuleRegistration()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules<ParentChildeModule>(typeof(Program).Assembly);

            var container = builder.Build();
            Console.WriteLine(container.Resolve<Child>().Parent);
        }


        static void Main(string[] args)
        {
            var registrationMethod = nameof(ModuleRegistration);

            switch (registrationMethod)
            {
                case nameof(ProvideDynamicArguments):
                    ProvideDynamicArguments();
                    break;

                case nameof(InjectParentDependencyViaProperty):
                    InjectParentDependencyViaProperty(3);
                    break;

                case nameof(BulkRegistration):
                    BulkRegistration();
                    break;


                case nameof(ModuleRegistration):
                    ModuleRegistration();
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
