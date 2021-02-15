// <copyright file="NavigationSteps.cs" company="National Careers Service">
// Copyright (c) National Careers Service. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using DFC.App.Accounts.Model;
using DFC.App.Accounts.UI.FunctionalTests.Pages;
using DFC.TestAutomation.UI.Extension;
using OpenQA.Selenium;
using System;
using System.Globalization;
using TechTalk.SpecFlow;

namespace DFC.App.Accounts.UI.FunctionalTests.StepDefinitions
{
    [Binding]
    internal class NavigationSteps
    {
        public NavigationSteps(ScenarioContext context)
        {
            this.Context = context;
        }

        private ScenarioContext Context { get; set; }

        [Given(@"I am on the (.*) page")]
        public void GivenIAmOnThePage(string pageName)
        {
            switch (pageName.ToLower(CultureInfo.CurrentCulture))
            {
                case "action plans landing":
                    var actionplansHomePage = new ActionPlansLandingPage(this.Context);
                    actionplansHomePage.NavigateToActionPlansPage();
                    var pageHeadingLocator = By.ClassName("govuk-heading-xl");
                    this.Context.GetHelperLibrary<AppSettings>().WebDriverWaitHelper.WaitForElementToContainText(pageHeadingLocator, "Action plans");
                    break;

                default:
                    throw new OperationCanceledException($"Unable to perform the step: {this.Context.StepContext.StepInfo.Text}. The page name provided was not recognised.");
            }
        }
    }
}
