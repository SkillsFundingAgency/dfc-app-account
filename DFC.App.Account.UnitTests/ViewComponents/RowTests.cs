﻿using DFC.App.Account.ViewComponents.Row;
using FluentAssertions;
using NUnit.Framework;

namespace DFC.App.Account.UnitTests.ViewComponents
{
    
    class RowTests
    {
        [Test]
        public void WhenModel_Then_ShouldReturnHtml()
        {
            var rm = new RowModel(){
                Label="Name",
                LabelValue = "Robin"
            };
            var viewComponent = new Row();
            var results = viewComponent.Invoke(rm);
            results.Should().NotBeNull();
        }


        [Test]
        public void WhenModelLabelValueNull_Then_ShouldReturnHtml()
        {
            var rm = new RowModel();
            rm.Label = "Name";
            rm.LabelValue = null;
            var label = rm.Label;
            var labelValue = rm.LabelValue;
            var viewComponent = new Row();
            var results = viewComponent.Invoke(rm);
            results.Should().NotBeNull();
        }
    }
}
