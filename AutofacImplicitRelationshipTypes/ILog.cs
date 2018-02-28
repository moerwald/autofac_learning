using System;

namespace AutofacImplicitRelationshipTypes
{
    public interface ILog : IDisposable
    {
        void Write(string message);
    }
}
