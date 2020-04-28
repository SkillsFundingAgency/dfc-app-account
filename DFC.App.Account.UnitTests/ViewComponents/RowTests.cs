using System;
using System.Collections.Generic;
using System.Text;
using DFC.App.Account.ViewComponents.Row;
using DFC.App.Account.ViewComponents.ShowRow;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DFC.App.Account.UnitTests.ViewComponents
{
    
    class RowTests
    {
        [Test]
        public void WhenModel_Then_ShouldReturnHtml()
        {
            var viewComponent = new Row();
            var results = viewComponent.InvokeAsync(new RowModel()
            {
                Label="Name",
                LabelValue = "Robin"
            });
            results.Should().NotBeNull();
        }
    }
}
