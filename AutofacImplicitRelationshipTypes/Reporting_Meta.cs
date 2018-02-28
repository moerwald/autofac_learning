using Autofac.Features.Metadata;
using System;

namespace AutofacImplicitRelationshipTypes
{

    

    public class Reporting_Meta
    {
        public class Setttings
        {
            public string LogMode { get; set; }

            public int MyProperty { get; set; }
        }


        private Meta<ConsoleLog, Setttings> log;

        public Reporting_Meta(Meta<ConsoleLog, Setttings> log)
        {
            this.log = log;
        }

        public void Report()
        {
            log.Value.Write("Starting report");
            
            if(log.Metadata.LogMode == "verbose")
            {
                log.Value.Write($"VERBOSE MODE: Logger started on {DateTime.Now}");
            }
        }
    }
}
