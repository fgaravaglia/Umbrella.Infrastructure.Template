using RazorLight;
using RazorLight.Razor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Umbrella.Infrastructure.Templating.Razor
{
	/// <summary>
	/// Implementation of <see cref="ITemplateRenderer"/> based on Razor view engine of MVC
	/// </summary>
    public class RazorRenderer : ITemplateRenderer
    {
		private readonly RazorLightEngine _engine;

        #region Constructors
		/// <summary>
		/// Default Constr			
		/// </summary>
		/// <param name="root"></param>
		/// <param name="isVerbose"></param>
        public RazorRenderer(string root = "", bool isVerbose = false)
		{
			var rootDirectory = String.IsNullOrEmpty(root) ? Directory.GetCurrentDirectory() : root;
			if (isVerbose)
			{
				Console.WriteLine("Root Folder: " + rootDirectory);
				Console.WriteLine("     Exists: " + Directory.Exists(rootDirectory));
			}
			var builder = new RazorLightEngineBuilder()
						.EnableDebugMode(isVerbose)
						.UseFileSystemProject(rootDirectory)
						.UseMemoryCachingProvider();

			this._engine = builder.Build();
		}
		/// <summary>
		/// Constructor using the project itself
		/// </summary>
		/// <param name="project"></param>
		/// <param name="isVerbose"></param>
		public RazorRenderer(RazorLightProject project, bool isVerbose = false)
		{
			if (project is null)
				throw new ArgumentNullException(nameof(project));

			_engine = new RazorLightEngineBuilder()
				.EnableDebugMode(isVerbose)
				.UseProject(project)
				.UseMemoryCachingProvider()
				.Build();
		}
		/// <summary>
		/// Constructor using reflection
		/// </summary>
		/// <param name="embeddedResRootType"></param>
		/// <param name="isVerbose"></param>
		public RazorRenderer(Type embeddedResRootType, bool isVerbose = false)
		{
			if(embeddedResRootType is null)
				throw new ArgumentNullException(nameof(embeddedResRootType));

			if (isVerbose)
			{
				Console.WriteLine("Root Assembly: " + embeddedResRootType.AssemblyQualifiedName);
				Console.WriteLine("         Type: " + embeddedResRootType.Name);
			}
			_engine = new RazorLightEngineBuilder()
				.EnableDebugMode(isVerbose)
				.UseEmbeddedResourcesProject(embeddedResRootType)
				.UseMemoryCachingProvider()
				.Build();
		}

        #endregion
        /// <summary>
        /// PArses the template actualizing it to get the concrete email body
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="template"></param>
        /// <param name="model"></param>
        /// <param name="isHtml"></param>
        /// <returns>the actualized template content</returns>
        public Task<string> ParseAsync<T>(string template, T model, bool isHtml = true)
		{
			if(model == null)
				throw new ArgumentNullException(nameof(model));
			if (String.IsNullOrEmpty(template))
				throw new ArgumentNullException(nameof(template));

			dynamic? viewBag = (model as IViewBagModel)?.ViewBag;
			return _engine.CompileRenderStringAsync<T>(GetHashString(template), template, model, viewBag);
		}
        /// <summary>
        /// Parses the template actualizing it to get the concrete email body
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="template"></param>
        /// <param name="model"></param>
        /// <param name="isHtml"></param>
        /// <returns>the actualized template content</returns>
		string ITemplateRenderer.Parse<T>(string template, T model, bool isHtml)
		{
			return ParseAsync(template, model, isHtml).GetAwaiter().GetResult();
		}
		/// <summary>
		/// Gets the hash from string
		/// </summary>
		/// <param name="inputString"></param>
		/// <returns></returns>
		public static string GetHashString(string inputString)
		{
			var sb = new StringBuilder();
			var hashbytes = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(inputString));
			foreach (byte b in hashbytes)
			{
				sb.Append(b.ToString("X2"));
			}

			return sb.ToString();
		}
	}
}
