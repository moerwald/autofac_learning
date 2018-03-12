using Autofac;

namespace ScopeAndLifetime
{

    partial class Program
    {
        class ParentChildModule : Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterType<Parent>();
                builder.Register(c => new Child { Parent = c.Resolve<Parent>() });
            }
        }
    }
}
