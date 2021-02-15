﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.4.0.0
//      SpecFlow Generator Version:3.4.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace DFC.App.Accounts.UI.FunctionalTests.Features
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.4.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class LoginToCitizenAccountFeature : object, Xunit.IClassFixture<LoginToCitizenAccountFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = ((string[])(null));
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "LoginToCitizenAccount.feature"
#line hidden
        
        public LoginToCitizenAccountFeature(LoginToCitizenAccountFeature.FixtureData fixtureData, DFC_App_Accounts_UI_FunctionalTests_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-GB"), "Features", "Login to Citizen account", null, ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        public virtual void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        void System.IDisposable.Dispose()
        {
            this.TestTearDown();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Login to citizen account and edit phone number")]
        [Xunit.TraitAttribute("FeatureTitle", "Login to Citizen account")]
        [Xunit.TraitAttribute("Description", "Login to citizen account and edit phone number")]
        [Xunit.TraitAttribute("Category", "Accounts")]
        [Xunit.TraitAttribute("Category", "Smoke")]
        public virtual void LoginToCitizenAccountAndEditPhoneNumber()
        {
            string[] tagsOfScenario = new string[] {
                    "Accounts",
                    "Smoke"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Login to citizen account and edit phone number", null, tagsOfScenario, argumentsOfScenario);
#line 4
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 5
 testRunner.Given("I am on the action plans landing page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 6
 testRunner.When("I click the Go to your action plans button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 7
 testRunner.And("I enter the email address in the Email address field", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 8
 testRunner.And("I enter the password in the Password field", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 9
 testRunner.And("I click the Sign in button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 10
 testRunner.Then("I am taken to the Your account page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 11
 testRunner.When("I click the Your details link", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 12
 testRunner.Then("I am taken to the Your details page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 13
 testRunner.When("I click the Edit your details button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 14
 testRunner.Then("I am taken to the Edit your details page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 15
 testRunner.When("I enter a valid phone number in the Home number field", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 16
 testRunner.When("I click the Save my changes button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 17
 testRunner.Then("I am taken to the Your details page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 18
 testRunner.And("the Home number field contains the updated details", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.4.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                LoginToCitizenAccountFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                LoginToCitizenAccountFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
