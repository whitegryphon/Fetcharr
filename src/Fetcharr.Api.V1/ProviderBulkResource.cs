using System.Collections.Generic;
using NzbDrone.Core.ThingiProvider;

namespace Fetcharr.Api.V1
{
    public class ProviderBulkResource<T>
    {
        public List<int> Ids { get; set; }
        public List<int> Tags { get; set; }
        public ApplyTags ApplyTags { get; set; }

        public ProviderBulkResource()
        {
            Ids = new List<int>();
        }
    }

    public enum ApplyTags
    {
        Add,
        Remove,
        Replace
    }

    public class ProviderBulkResourceMapper<TProviderBulkResource, TProviderDefinition>
        where TProviderBulkResource : ProviderBulkResource<TProviderBulkResource>, new()
        where TProviderDefinition : ProviderDefinition, new()
    {
        public virtual List<TProviderDefinition> UpdateModel(TProviderBulkResource resource, List<TProviderDefinition> existingDefinitions)
        {
            if (resource == null)
            {
                return new List<TProviderDefinition>();
            }

            return existingDefinitions;
        }
    }
}
