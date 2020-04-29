using DFC.App.Account.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace DFC.App.Account.UnitTests.Helpers
{
    class HyphenControllerTrasnformerTests
    {
        [Test]
        public void WhenHyphenatedRoute_ThenAddCleanHyphenatedRouteToApplication()
        {
            var sut = new HyphenControllerTransformer();
            var result = sut.TransformOutbound("your-details");
            result.Should().Be("your-details");
        }

        [Test] public void WhenHyphenatedRouteNull_ThenReturnNull()
        {
            var sut = new HyphenControllerTransformer();
            var result = sut.TransformOutbound(null);
            result.Should().BeNull();
        }
    }
}
