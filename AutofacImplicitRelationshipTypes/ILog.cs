using System;

namespace AutofacImplicitRelationshipTypes
{
    interface ILog : IDisposable
    {
        void Write(string message);
    }
}
