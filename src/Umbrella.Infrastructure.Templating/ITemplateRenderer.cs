using System.Threading.Tasks;

namespace Umbrella.Infrastructure.Templating
{
    /// <summary>
    /// Abstraction for Template management
    /// </summary>
    public interface ITemplateRenderer
    {
        /// <summary>
        /// Parses the template actualizing it to get the concrete email body
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="template"></param>
        /// <param name="model"></param>
        /// <param name="isHtml"></param>
        /// <returns>the actualized template content</returns>
        string Parse<T>(string template, T model, bool isHtml = true);
        /// <summary>
        /// PArses the template actualizing it to get the concrete email body
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="template"></param>
        /// <param name="model"></param>
        /// <param name="isHtml"></param>
        /// <returns>the actualized template content</returns>
        Task<string> ParseAsync<T>(string template, T model, bool isHtml = true);
    }
}
