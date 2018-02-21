using Autofac;

namespace AutofacImplicitRelationshipTypes
{

    class Program
    {
        static void Main(string[] args)
        {
            //LazyInstantiation();
            //ControlledInstantiation();
            //DynamicInstantiation();
            ParameterizedInstantiation();
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
