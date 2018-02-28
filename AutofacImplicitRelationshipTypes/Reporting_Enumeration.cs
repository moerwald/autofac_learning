using System.Collections.Generic;

namespace AutofacImplicitRelationshipTypes
{
    public class Reporting_Enumeration
    {
        private IList<ILog> allLogs;

        public Reporting_Enumeration(IList<ILog> logs)
        {
            // Autofac will inject a list with all registered ILogs
            // If no ILog is regeisterd the list will be empty
            this.allLogs = logs;
        }


        public void Report()
        {
            foreach (var l in this.allLogs)
            {
                l.Write($"This is {l.GetType().Name}");
            }
        }
    }
}
