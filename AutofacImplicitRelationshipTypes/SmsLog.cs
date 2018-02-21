using System;

namespace AutofacImplicitRelationshipTypes
{
    class SmsLog : ILog
    {
        private readonly string phoneNumber;

        public SmsLog(string phoneNumber)
        {
            this.phoneNumber = phoneNumber;
        }
        public void Dispose()
        {
            Console.WriteLine($"[{nameof(SmsLog)}]: disposed");
        }

        public void Write(string message)
        {
            Console.WriteLine($"[{nameof(SmsLog)}]: sent {message} to {this.phoneNumber}");
        }
    }
}
