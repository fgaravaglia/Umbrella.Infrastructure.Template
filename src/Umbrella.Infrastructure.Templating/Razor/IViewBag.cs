using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Umbrella.Infrastructure.Templating.Razor
{
	/// <summary>
	/// Viewbag implementation to emulate MVC context
	/// </summary>
	public interface IViewBagModel
	{
		/// <summary>
		/// Emulated Viewbag of MVC
		/// </summary>
		ExpandoObject ViewBag { get; }
	}
}
