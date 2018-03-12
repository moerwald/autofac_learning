using Autofac;

namespace ScopeAndLifetime
{

    partial class Program
    {
        static void Main(string[] args)
        {
            var name = nameof(RunningCodeAtStartup);
            switch (name)
            {
                case nameof(InstancePerLifeTimeScope):
                    InstancePerLifeTimeScope();
                    break;

                case nameof(InstancePerMatchingLifeTimeScope):
                    InstancePerMatchingLifeTimeScope();
                    break;

                case nameof(CaptiveDependencies):
                    CaptiveDependencies();
                    break;

                case nameof(Disposal):
                    Disposal();
                    break;

                case nameof(LifeTimeEvents):
                    LifeTimeEvents();
                    break;

                case nameof(RunningCodeAtStartup):
                    RunningCodeAtStartup();
                    break;
            }
        }


        class MyClass : IStartable
        {
            public MyClass()
            {
                System.Console.WriteLine("MyClass CTOR");
            }
            public void Start()
            {
                System.Console.WriteLine("Start");
            }
        }

        private static void RunningCodeAtStartup()
        {
            var builder = new ContainerBuilder();
            builder
                .RegisterType<MyClass>()
                .AsSelf()
                .As<IStartable>()
                .SingleInstance();

            var container = builder.Build();
            container.Resolve<IStartable>();
        }

        private static void LifeTimeEvents ()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Parent>();
            builder.RegisterType<Child>()
                .OnActivating(a => 
                {
                    System.Console.WriteLine("Child activating");
                   // a.ReplaceInstance(new BadChild());
                    a.Instance.Parent = a.Context.Resolve<Parent>();
                })
                .OnActivated(a => 
                {
                    System.Console.WriteLine("Child activated");
                })
                .OnRelease(a=>
                {
                    System.Console.WriteLine("Child is removed");
                });

            using (var scope = builder.Build().BeginLifetimeScope())
            {
                var child = scope.Resolve<Child>();
                var parent = child.Parent;
                System.Console.WriteLine(child);
                System.Console.WriteLine(parent);
            }
        }

        private static void Disposal()
        {
            var builder = new ContainerBuilder();
            //
            // builder.RegisterType<ConsoleLog>()
            //        .ExternallyOwned(); // Autoface wont call dispose on ConsoleLog instance

            builder.RegisterInstance(new ConsoleLog());


            using (var container = builder.Build())
            {
                using (var scope = container.BeginLifetimeScope())
                {
                    scope.Resolve<ConsoleLog>();
                }
            }
        }

        private static void CaptiveDependencies()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ResourceManager>().SingleInstance();
            builder.RegisterType<SingletonResource>().As<IResource>().SingleInstance();
            builder.RegisterType<InstancePerDependencyResource>().As<IResource>();

            using (var container = builder.Build())
            {
                using (var scope = container.BeginLifetimeScope())
                {
                    // InstancePerDependencyResource is only created once
                    scope.Resolve<ResourceManager>();
                }
            }
        }

        private static void InstancePerMatchingLifeTimeScope()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleLog>().As<ILog>()
                .InstancePerMatchingLifetimeScope("foo");

            var container = builder.Build();

            using (var scope1 = container.BeginLifetimeScope("foo"))
            {
                for (int i = 0; i < 3; i++)
                {
                    // Only one instance of ILog is created since we defined
                    // InstancePerLifetimeScope at the builder
                    scope1.Resolve<ILog>();
                }

                using (var scope2 = scope1.BeginLifetimeScope())
                {
                    // the above created instance is returned
                    for (int i = 0; i < 3; i++)
                    {
                        scope2.Resolve<ILog>();
                    }
                }
            }
        }

        private static void InstancePerLifeTimeScope()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleLog>().As<ILog>().InstancePerLifetimeScope();

            var container = builder.Build();

            using (var scope1 = container.BeginLifetimeScope())
            {
                for (int i = 0; i < 3; i++)
                {
                    // Only one instance of ILog is created since we defined
                    // InstancePerLifetimeScope at the builder
                    scope1.Resolve<ILog>();
                }
            }


            using (var scope2 = container.BeginLifetimeScope())
            {
                for (int i = 0; i < 3; i++)
                {
                    scope2.Resolve<ILog>();
                }
            }
        }
    }
}
