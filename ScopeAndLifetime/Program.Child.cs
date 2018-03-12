namespace ScopeAndLifetime
{
    partial class Program
    {
        class Child
        {
            public string Name { get; set; }
            public Parent Parent { get; set; }

            public Child()
            {
                System.Console.WriteLine("Child is being created");
            }

            public override string ToString()
            {
                return "Hello there";
            }
        }
    }
}
