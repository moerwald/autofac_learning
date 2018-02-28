using Autofac;

namespace AutofacImplicitRelationshipTypes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //LazyInstantiation();
            //ControlledInstantiation();
            //DynamicInstantiation();
            // ParameterizedInstantiation();
            //EnumerationInstantiation();
            //MetaInstiation();
            KeyServiceLookupInstantiation();
        }

        private static void KeyServiceLookupInstantiation()
        {
            var builder = new ContainerBuilder();

            // We register ConsoleLog with strongly typed Metadata
            builder.RegisterType<ConsoleLog>().Keyed<ILog>("console");

            builder.Register( c => new SmsLog("+123234")).Keyed<ILog>("sms");

            builder.RegisterType<Reporting_KeyServiceLookup>();

            using (var c = builder.Build())
            {
                c.Resolve<Reporting_KeyServiceLookup>().Report();
            }
        }

        private static void MetaInstiation()
        {
            var builder = new ContainerBuilder();

            // We register ConsoleLog with strongly typed Metadata
            builder
                .RegisterType<ConsoleLog>()
                .WithMetadata<Reporting_Meta.Setttings>
                    (configAction => configAction.For(x => x.LogMode
                                                     , "verbose"));
            builder.RegisterType<Reporting_Meta>();

            using (var c = builder.Build())
            {
                c.Resolve<Reporting_Meta>().Report();
            }
        }


        private static void EnumerationInstantiation()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleLog>().As<ILog>();
            builder.Register(ctx => new SmsLog("+123456789")).As<ILog>();
            builder.RegisterType<Reporting_Enumeration>();

            using (var c = builder.Build())
            {
                c.Resolve<Reporting_Enumeration>().Report();
            }
        }


        private static void ControlledInstantiation()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleLog>();
            builder.RegisterType<Reporting_ControlledInstantiation>();

            using (var c = builder.Build())
            {
                c.Resolve<Reporting_ControlledInstantiation>().ReportOnce();
            }
        }

        private static void LazyInstantiation()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleLog>();
            builder.RegisterType<Reporting>();

            using (var c = builder.Build())
            {
                c.Resolve<Reporting>().Report();
            }
        }

        private static void DynamicInstantiation()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleLog>();
            builder.RegisterType<Reporting_DynamicInstantiation>();

            using (var c = builder.Build())
            {
                c.Resolve<Reporting_DynamicInstantiation>().Report();
            }
        }

        private static void ParameterizedInstantiation()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleLog>();
            builder.RegisterType<SmsLog>();
            builder.RegisterType<Reporting_ParameterizedInstantiation>();

            using (var c = builder.Build())
            {
                c.Resolve<Reporting_ParameterizedInstantiation>().Report();
            }
        }
    }
}
