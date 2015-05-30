using Autofac;
using NUnit.Framework;
using StepOnThat.Browser;
using StepOnThat.Http;
using StepOnThat.Infrastructure;
using StepOnThat.Tests.Browser;

namespace StepOnThat.Tests
{
    [TestFixture]
    public class ExMethodsTests
    {
        [Test]
        public void IsTypeOfReturnsFalseWhenTypesDontMatch()
        {
            Assert.IsFalse(new HttpStep(new HttpClient()).IsTypeOf<Step>());
            Assert.IsFalse(new HttpStep(new HttpClient()).IsTypeOf<BrowserStep>());
        }

        [Test]
        public void IsTypeOfReturnsTrueWhenTypesMatch()
        {
            Assert.IsTrue(new HttpStep(new HttpClient()).IsTypeOf<HttpStep>());
            Assert.IsTrue(new BrowserStep(new WebBrowser()).IsTypeOf<BrowserStep>());
            Assert.IsTrue(new Step().IsTypeOf<Step>());
        }

        [Test]
        public void IsTypeOfReturnsTrueWhenTypesMatchIGgnoringProxy()
        {
            using (var scope = new DependencyResolver().Container.BeginLifetimeScope())
            {
                var sut = scope.Resolve<BrowserStep>();
                Assert.IsTrue(sut.IsTypeOf<BrowserStep>());
                Assert.IsFalse(sut.IsTypeOf<Step>());
            }
        }
    }
}