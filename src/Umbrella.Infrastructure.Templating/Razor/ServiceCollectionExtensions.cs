using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RazorLight.Razor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Umbrella.Infrastructure.Templating.Razor
{
    public static  class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add razor renderer with project views and layouts
        /// </summary>
        /// <param name="services"></param>
        /// <param name="templateRootFolder"></param>
        public static void AddRazorRenderer(this IServiceCollection services, string templateRootFolder)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.TryAdd(ServiceDescriptor.Singleton<ITemplateRenderer, RazorRenderer>(_ => new RazorRenderer(templateRootFolder)));
        }
        /// <summary>
		/// Add razor renderer with embedded views and layouts
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="embeddedResourceRootType"></param>
		/// <returns></returns>
	    public static void AddRazorRenderer(this IServiceCollection services, Type embeddedResourceRootType)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.TryAdd(ServiceDescriptor.Singleton<ITemplateRenderer, RazorRenderer>(_ => new RazorRenderer(embeddedResourceRootType)));
        }
        /// <summary>
	    /// Add razor renderer with a RazorLightProject to support views and layouts
	    /// </summary>
	    /// <param name="builder"></param>
	    /// <param name="razorLightProject"></param>
	    /// <returns></returns>
	    public static void AddRazorRenderer(this IServiceCollection services, RazorLightProject razorLightProject)
        {
            services.TryAdd(ServiceDescriptor.Singleton<ITemplateRenderer, RazorRenderer>(_ => new RazorRenderer(razorLightProject)));
        }
    }
}
