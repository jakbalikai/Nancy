namespace Nancy.Conventions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bootstrapper;

    /// <summary>
    /// Registers the static contents hook at the 
    /// </summary>
    public class StaticContentStartup : IStartup
    {
        private readonly IRootPathProvider rootPathProvider;
        private readonly StaticContentsConventions conventions;

        public StaticContentStartup(IRootPathProvider rootPathProvider, StaticContentsConventions conventions)
        {
            this.rootPathProvider = rootPathProvider;
            this.conventions = conventions;
        }

        /// <summary>
        /// Gets the type registrations to register for this startup task`
        /// </summary>
        public IEnumerable<TypeRegistration> TypeRegistrations
        {
            get { return Enumerable.Empty<TypeRegistration>(); }
        }

        /// <summary>
        /// Gets the collection registrations to register for this startup task
        /// </summary>
        public IEnumerable<CollectionTypeRegistration> CollectionTypeRegistrations
        {
            get { return Enumerable.Empty<CollectionTypeRegistration>(); }
        }

        /// <summary>
        /// Gets the instance registrations to register for this startup task
        /// </summary>
        public IEnumerable<InstanceRegistration> InstanceRegistrations
        {
            get { return Enumerable.Empty<InstanceRegistration>(); }
        }

        /// <summary>
        /// Perform any initialisation tasks
        /// </summary>
        public void Initialize(IApplicationPipelines pipelines)
        {
            var item = new PipelineItem<Func<NancyContext, Response>>("Static content", ctx =>
            {
                return conventions
                    .Select(convention => convention.Invoke(ctx, rootPathProvider.GetRootPath()))
                    .FirstOrDefault(response => response != null);
            });
            
            pipelines.BeforeRequest.AddItemToEndOfPipeline(item);
        }
    }
}