using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Umbrella.Infrastructure.Templating
{
    /// <summary>
    /// Implementation of <see cref="ITemplateRenderer"/> that replaces variables with thier values
    /// </summary>
    public class StringReplaceRenderer : ITemplateRenderer
    {
        /// <summary>
        /// Parses the template actualizing it to get the concrete email body
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="template"></param>
        /// <param name="model"></param>
        /// <param name="isHtml"></param>
        /// <returns>the actualized template content</returns>
        public string Parse<T>(string template, T model, bool isHtml = true)
        {
            if (String.IsNullOrEmpty(template))
                throw new ArgumentNullException(nameof(model));

            if (model is null)
                throw new ArgumentNullException(nameof(model));

            foreach (PropertyInfo pi in model.GetType().GetRuntimeProperties())
            {
                var value = pi.GetValue(model, null);
                if (value != null)
                    template = template.Replace($"##{pi.Name}##", value.ToString());
            }

            return template;
        }
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
            return Task.FromResult(Parse(template, model, isHtml));
        }
    }
}
