// <copyright file="ValidationSteps.cs" company="National Careers Service">
// Copyright (c) National Careers Service. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using DFC.App.Accounts.Model;
using DFC.TestAutomation.UI;
using DFC.TestAutomation.UI.Extension;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Globalization;
using TechTalk.SpecFlow;

namespace DFC.App.Accounts.UI.FunctionalTests.StepDefinitions
{
    [Binding]
    internal class ValidationSteps
    {
        public ValidationSteps(ScenarioContext context)
        {
            this.Context = context;
        }

        private ScenarioContext Context { get; set; }

        [Then(@"I am taken to the (.*) page")]
        public void ThenIAmTakenToThePage(string pageName)
        {
            By locator = null;

            switch (pageName.ToLower(CultureInfo.CurrentCulture))
            {
                case "sign in":
                    locator = By.CssSelector("h1");
                    break;

                case "your account":
                    locator = By.ClassName("govuk-heading-xl");
                    break;

                case "your details":
                    locator = By.ClassName("govuk-heading-xl");
                    break;

                case "edit your details":
                    locator = By.ClassName("govuk-heading-xl");
                    break;

                default:
                    locator = By.CssSelector("h1.govuk-fieldset__heading");
                    break;
            }

            this.Context.GetHelperLibrary<AppSettings>().WebDriverWaitHelper.WaitForElementToContainText(locator, pageName);
        }

        [Then(@"the (.*) field contains the updated details")]
        public void ThenTheHomeNumberFieldContainsTheUpdatedNumber(string fieldLabel)
        {
            bool updateSuccesful = false;

            IWebElement tableElement = this.Context.GetWebDriver().FindElement(By.XPath("//*[@id='main-content']/div/div/div/div/div/div/div[1]/div/form/div[1]/table[2]"));

            IList<IWebElement> trCollection = tableElement.FindElements(By.TagName("tr"));

            IList<IWebElement> tdCollection;

            foreach (IWebElement element in trCollection)
            {
                tdCollection = element.FindElements(By.TagName("td"));
                string column1 = tdCollection[0].Text;
                string column2 = tdCollection[1].Text;
                if (column2.Equals(this.Context.Get<IObjectContext>().GetObject("HomeNumber")))
                {
                    updateSuccesful = true;
                }
            }

            if (!updateSuccesful)
            {
                throw new NotFoundException($"Unable to perform the step: {this.Context.StepContext.StepInfo.Text}. Phone number update unsuccessful");
            }
        }
    }
}