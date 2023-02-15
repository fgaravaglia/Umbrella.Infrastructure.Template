using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using Umbrella.Infrastructure.Templating.Razor;

namespace Umbrella.Infrastructure.Templating.Tests.Razor
{
    public class RazorTests
    {
        ITemplateRenderer _Renderer;

        [SetUp]
        public void Setup()
        {
            this._Renderer = new RazorRenderer();
        }

        [Test]
        public void TextTemplateWithList_IsRendered_AsMatch()
        {
            //******* GIVEN
            string template = "sup @Model.Name here is a list @foreach(var i in Model.Numbers) { @i }";
            var model = new ViewModelWithViewBag() { Name = "LUKE", Numbers = new string[] { "1", "2", "3" } };

            //******* WHEN
            var body = this._Renderer.Parse<ViewModelWithViewBag>(template, model);

            //******* ASSERT
            Assert.That(body, Is.Not.Null);
            Assert.That(body, Is.EqualTo("sup LUKE here is a list 123"));
        }

        [Test]
        public void TextTemplateWithList_WithForEach_IsRendered_AsMatch()
        {
            //******* GIVEN
            string template = "sup @Model.Name here is a list @foreach(var i in Model.Numbers) { @i }";
            var model = new ViewModelWithViewBag() { Name = "LUKE", Numbers = new string[] { "1", "2", "3" } };            
            
            //******* WHEN
            var body = this._Renderer.Parse<ViewModelWithViewBag>(template, model);

            //******* ASSERT
            Assert.That(body, Is.Not.Null);
            Assert.That(body, Is.EqualTo("sup LUKE here is a list 123"));
        }


        [Test]
        public void TemplateWithLayoutAndViewBag_ISRendered_AsMAtch()
        {
            //******* GIVEN
            var projectRoot = Directory.GetCurrentDirectory();
            this._Renderer = new RazorRenderer(projectRoot, true);
            string template = @"
@{
    Layout = ""./Razor/Shared/_Layout.cshtml"";
}
sup @Model.Name here is a list @foreach(var i in Model.Numbers) { @i }";
            dynamic viewBag = new ExpandoObject();
            viewBag.Title = "Hello!";
            var model = new ViewModelWithViewBag { Name = "LUKE", Numbers = new[] { "1", "2", "3" }, ViewBag = viewBag };

            //******* WHEN
            var body = this._Renderer.Parse<ViewModelWithViewBag>(template, model);

            //******* ASSERT
            Assert.That(body, Is.Not.Null);
            Assert.That(body, Is.EqualTo($"<h1>Hello!</h1>{Environment.NewLine}<div>{Environment.NewLine}sup LUKE here is a list 123</div>"));
        }

        [Test]
        public void TemplateWithEmbeddedLayoutAndViewBag_ISRendered_AsMAtch_UsingTypeAtSameLevelOfEmbeddedLayout()
        {
            //******* GIVEN
            string template = @"
@{
    Layout = ""_EmbeddedLayout.cshtml"";
}
sup @Model.Name here is a list @foreach(var i in Model.Numbers) { @i }";
            dynamic viewBag = new ExpandoObject();
            viewBag.Title = "Hello!";
            var model = new ViewModelWithViewBag { Name = "LUKE", Numbers = new[] { "1", "2", "3" }, ViewBag = viewBag };
            this._Renderer = new RazorRenderer(typeof(RazorTests), isVerbose: true);

            //******* WHEN
            var body = this._Renderer.Parse<ViewModelWithViewBag>(template, model);

            //******* ASSERT
            Assert.That(body, Is.Not.Null);
            Assert.That(body, Is.EqualTo($"<h2>Hello!</h2>{Environment.NewLine}<div>{Environment.NewLine}sup LUKE here is a list 123</div>"));
        }

        [Test]
        public void TemplateWithEmbeddedLayoutAndViewBag_ThrowsEx_UsingTypeAtRootOfAssembly()
        {
            //******* GIVEN
            string template = @"
@{
    Layout = ""Razor/_EmbeddedLayout.cshtml"";
}
sup @Model.Name here is a list @foreach(var i in Model.Numbers) { @i }";
            dynamic viewBag = new ExpandoObject();
            viewBag.Title = "Hello!";
            var model = new ViewModelWithViewBag { Name = "LUKE", Numbers = new[] { "1", "2", "3" }, ViewBag = viewBag };
            this._Renderer = new RazorRenderer(typeof(StringReplaceRendererTests), isVerbose: true);

            //******* WHEN
            TestDelegate testcode = () => this._Renderer.Parse<ViewModelWithViewBag>(template, model);

            //******* ASSERT
            Assert.Throws<RazorLight.TemplateNotFoundException>(testcode);
        }
    }

    public class ViewModelWithViewBag : IViewBagModel
    {
        public ExpandoObject ViewBag { get; set; }
        public string Name { get; set; }
        public string[] Numbers { get; set; }
    }
}
