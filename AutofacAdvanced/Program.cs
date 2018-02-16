using Autofac;
using Autofac.Core;
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


        static void Main(string[] args)
        {
            var registrationMethod = nameof(ProvideDynamicArguments);

            switch (registrationMethod)
            {
                case nameof(ProvideDynamicArguments):
                    ProvideDynamicArguments();
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
