using System;
using DFC.App.Account.Application.Common.CustomAttributes;
using NUnit.Framework;

namespace DFC.App.Account.Application.UnitTests
{
    public class AcceptAttributeTests
    {
        [Test]
        public void When_IsValid_IsCalledWithNonBoolean_ThenThrowInvalidOperationException()
        {
            var att = new AcceptAttribute();
            var val = 1;

            Assert.Throws<InvalidOperationException>(() => att.IsValid(val));
        }

        [Test]
        public void When_IsValid_IsCalledWithNull_ThenReturnFalse()
        {
            var att = new AcceptAttribute();
            Assert.False(att.IsValid(null));
        }

        [Test]
        public void When_IsValid_IsCalledWithTrue_ThenReturnTrue()
        {
            var att = new AcceptAttribute();
            Assert.True(att.IsValid(1 == 1));
        }
    }
}
