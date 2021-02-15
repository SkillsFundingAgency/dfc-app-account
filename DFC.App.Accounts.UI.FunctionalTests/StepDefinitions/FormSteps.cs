// <copyright file="FormSteps.cs" company="National Careers Service">
// Copyright (c) National Careers Service. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using DFC.App.Accounts.Model;
using DFC.TestAutomation.UI;
using DFC.TestAutomation.UI.Extension;
using OpenQA.Selenium;
using System;
using System.Globalization;
using System.Text;
using TechTalk.SpecFlow;

namespace DFC.App.Accounts.UI.FunctionalTests.StepDefinitions
{
    [Binding]
    internal class FormSteps
    {
        public FormSteps(ScenarioContext context)
        {
            this.Context = context;
        }

        private ScenarioContext Context { get; set; }

        [When(@"I select the radio button option (.*)")]
        public void WhenISelectTheRadioButtonOption(string radioButtonLabel)
        {
            if (!this.InteractWithRadioButtonOrCheckbox(radioButtonLabel))
            {
                throw new NotFoundException($"Unable to perform the step: {this.Context.StepContext.StepInfo.Text}. The label could not be found.");
            }
        }

        [When(@"I click the checkbox option (.*)")]
        public void WhenIClickTheCheckboxOption(string checkboxLabel)
        {
            if (!this.InteractWithRadioButtonOrCheckbox(checkboxLabel))
            {
                throw new NotFoundException($"Unable to perform the step: {this.Context.StepContext.StepInfo.Text}. The label could not be found.");
            }
        }

        [When(@"I enter (.*) in the (.*) field")]
        public void WhenIEnterInTheField(string text, string fieldLabel)
        {
            By locator = null;

            switch (fieldLabel.ToLower(CultureInfo.CurrentCulture))
            {
                case "email address":
                    locator = By.Id("emailCustom");
                    text = this.Context.GetSettingsLibrary<AppSettings>().TestExecutionSettings.AppLoginEmail;
                    break;

                case "password":
                    locator = By.Id("passwordCustom");
                    text = this.Context.GetSettingsLibrary<AppSettings>().TestExecutionSettings.AppLoginPassword;
                    break;

                case "home number":
                    locator = By.Id("Identity_ContactDetails_HomeNumber");
                    text = this.GetRandomTelNo();
                    this.Context.Get<IObjectContext>().SetObject("HomeNumber", text);
                    break;

                default:
                    throw new NotFoundException($"Unable to perform the step: {this.Context.StepContext.StepInfo.Text}. The field could not be found.");
            }

            this.Context.GetWebDriver().FindElement(locator).Clear();
            this.Context.GetWebDriver().FindElement(locator).SendKeys(text);
        }

        private bool InteractWithRadioButtonOrCheckbox(string inputLabelText)
        {
            var allLabels = this.Context.GetWebDriver().FindElements(By.TagName("label"));
            foreach (var label in allLabels)
            {
                if (label.Text.Trim().Equals(inputLabelText, System.StringComparison.OrdinalIgnoreCase))
                {
                    var parentNode = this.Context.GetHelperLibrary<AppSettings>().JavaScriptHelper.GetParentElement(label);
                    var input = parentNode.FindElement(By.TagName("input"));
                    input.Click();
                    return true;
                }
            }

            return false;
        }

        private string GetRandomTelNo()
        {
            Random rand = new Random();
            StringBuilder telNo = new StringBuilder(11);
            int number;
            telNo = telNo.Append("0");
            for (int i = 0; i < 3; i++)
            {
                number = rand.Next(1, 9); // digit between 1 (incl) and 9 (excl)
                telNo = telNo.Append(number.ToString());
            }

            number = rand.Next(0, 9999999); // number between 0 (incl) and 9999999 (excl)
            telNo = telNo.Append(String.Format("{0:D7}", number));
            return telNo.ToString();
        }
    }
}
