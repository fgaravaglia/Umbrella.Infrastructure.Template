using Umbrella.Infrastructure.Templating;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Umbrella.Infrastructure.Templating.Tests
{
    public class StringReplaceRendererTests
    {
        ITemplateRenderer _Renderer;

        [SetUp]
        public void Setup()
        {
            this._Renderer = new StringReplaceRenderer();
        }

        [Test]
        public void TextTemplate_IsRendered_AsMatch()
        {
            //******* GIVEN
            string template = "sup ##Name## here is a number ##Number##";
            var model = new ViewModel() { Name = "LUKE", Number = 123 };

            //******* WHEN
            var body = this._Renderer.Parse<ViewModel>(template, model);

            //******* ASSERT
            Assert.That(body, Is.Not.Null);
            Assert.That(body, Is.EqualTo("sup LUKE here is a number 123"));
        }
    }

    public class ViewModel
    {
        public string Name { get; set; }
        public int Number { get; set; }
    }
}
