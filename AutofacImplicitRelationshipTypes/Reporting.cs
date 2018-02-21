using System;

namespace AutofacImplicitRelationshipTypes
{
    class Reporting
    {
        Lazy<ConsoleLog> log;

        public Reporting(Lazy<ConsoleLog> log)
        {
            this.log = log;
            Console.WriteLine("Reporting component created");
        }

        public void Report()
        {
            log.Value.Write("Log started");
        }
    }


    class Reporting_DynamicInstantiation
    {
        private readonly Func<ConsoleLog> consoleLog;

        public Reporting_DynamicInstantiation(Func<ConsoleLog> consoleLog)
        {
            this.consoleLog = consoleLog ?? throw new ArgumentNullException(nameof(consoleLog));
        }


        public void Report()
        {
            this.consoleLog().Write("Reporting to console");
            this.consoleLog().Write("and again");
        }
    }

    class Reporting_ParameterizedInstantiation
    {
        private readonly Func<ConsoleLog> consoleLog;
        private readonly Func<string, SmsLog> smsLog;

        public Reporting_ParameterizedInstantiation
            ( Func<ConsoleLog> consoleLog
            , Func<string,SmsLog> smsLog)
        {
            this.consoleLog = consoleLog ?? throw new ArgumentNullException(nameof(consoleLog));
            this.smsLog = smsLog;
        }


        public void Report()
        {
            this.consoleLog().Write("Reporting to console");
            this.consoleLog().Write("and again");
            this.smsLog("+1234567").Write("Texting admins ,.,.");
        }
    }

}
