namespace ScopeAndLifetime
{

    partial class Program
    {
        class BadChild :Child
        {
            public override string ToString()
            {
                return "I hate you";
            }
        }
    }
}
