using Autofac.Features.Indexed;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutofacImplicitRelationshipTypes
{
    public class Reporting_KeyServiceLookup
    {

        private IIndex<string, ILog> logs;

        public Reporting_KeyServiceLookup(IIndex<string, ILog> logs)
        {
            this.logs = logs;
        }

        public void Report()
        {
            logs?["sms"]?.Write("Starting out report");
        }
    }
}
