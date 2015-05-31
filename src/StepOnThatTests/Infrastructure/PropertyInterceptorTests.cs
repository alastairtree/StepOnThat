using NUnit.Framework;

namespace StepOnThat.Infrastructure.Tests
{
    [TestFixture]
    public class PropertyInterceptorTests
    {
        [Test]
        public void ApplyPropertiesDoesMultipleSubstitutionsUsingBothSyntaxes()
        {
            var props = new PropertyCollection();
            var sut = new PropertyInterceptor(props);
            props.Add("test1", "world");
            props.Add("test2", "lovely");
            const string value = "hello ${test1} ain't this {{test2}}!";

            const string expected = "hello world ain't this lovely!";
            var actual = sut.ApplyPropertiesToValue(value);

            Assert.AreEqual(expected, actual);
        }
    }
}