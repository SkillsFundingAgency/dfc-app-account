using System;
using System.Collections.Generic;
using System.Text;
using DFC.App.Account.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace DFC.App.Account.UnitTests.Helpers
{
    class HyphenControllerTrasnformerTests
    {
        [Test]
        public void WhenHyphenatedRoute_ThenReturnNonHyphenated()
        {
            var sut = new HyphenControllerTransformer();
            var result = sut.TransformOutbound("your-details");
            result.Should().Be("your-details");
        }
    }
}
