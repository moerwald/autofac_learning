using System.Collections.Generic;

namespace ScopeAndLifetime
{
    class ResourceManager
    {
        public ResourceManager(IEnumerable<IResource> resources)
        {
            Resources = resources;
        }

        public IEnumerable<IResource> Resources { get; }
    }
}
