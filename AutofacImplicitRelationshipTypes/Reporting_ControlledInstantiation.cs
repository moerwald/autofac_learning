using Autofac.Features.OwnedInstances;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutofacImplicitRelationshipTypes
{
    public class Reporting_ControlledInstantiation
    {
        private readonly Owned<ConsoleLog> log;

        public Reporting_ControlledInstantiation(Owned<ConsoleLog> log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            Console.WriteLine($"[{nameof(Reporting_ControlledInstantiation)}]: Created");
        }

        public void ReportOnce()
        {
            log.Value.Write("Log started");
            log.Dispose(); // Call it on owned type
        }
    }
}
