using RazorLight.Razor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Umbrella.Infrastructure.Templating.Razor
{
    /// <summary>
    /// CLass to extend  <see cref="RazorLightProject"/> in order to run razor tempalte in memory
    /// </summary>
    public class InMemoryRazorLightProject : RazorLightProject
    {
        public override Task<RazorLightProjectItem> GetItemAsync(string templateKey)
        {
            return Task.FromResult<RazorLightProjectItem>(new TextSourceRazorProjectItem(templateKey, templateKey));
        }

        public override Task<IEnumerable<RazorLightProjectItem>> GetImportsAsync(string templateKey)
        {
            return Task.FromResult<IEnumerable<RazorLightProjectItem>>(new List<RazorLightProjectItem>());
        }
    }
}
