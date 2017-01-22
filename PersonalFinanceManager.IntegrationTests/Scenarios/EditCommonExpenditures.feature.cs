﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.1.0.0
//      SpecFlow Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace PersonalFinanceManager.IntegrationTests.Scenarios
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.1.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("EditCommonExpenditures")]
    public partial class EditCommonExpendituresFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "EditCommonExpenditures.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "EditCommonExpenditures", "\tEdit expenditures with payment method Common", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Edit the amount of a Common expenditures")]
        [NUnit.Framework.CategoryAttribute("editcommonexpenditure")]
        public virtual void EditTheAmountOfACommonExpenditures()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Edit the amount of a Common expenditures", new string[] {
                        "editcommonexpenditure"});
#line 5
this.ScenarioSetup(scenarioInfo);
#line 6
 testRunner.Given("I have accessed the Expenditures List page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 7
 testRunner.And("I have at least one expenditure with this payment method in the list", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 8
 testRunner.When("I click on edit for this expenditure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 9
 testRunner.And("I edit the Cost", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 10
 testRunner.And("I click on the Save button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
 testRunner.Then("the expenditure has been updated", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 12
 testRunner.Then("the source account is updated", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 13
 testRunner.Then("a mouvement entry has been saved", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Edit the amount and the payment method to Cash Expenditures")]
        [NUnit.Framework.CategoryAttribute("editcommonexpenditure")]
        public virtual void EditTheAmountAndThePaymentMethodToCashExpenditures()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Edit the amount and the payment method to Cash Expenditures", new string[] {
                        "editcommonexpenditure"});
#line 16
this.ScenarioSetup(scenarioInfo);
#line 17
 testRunner.Given("I have accessed the Expenditures List page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 18
 testRunner.And("I have at least one expenditure with this payment method in the list", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 19
 testRunner.When("I click on edit for this expenditure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 20
 testRunner.And("I edit the Cost", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 21
 testRunner.And("I change the payment method to Cash Expenditures", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 22
 testRunner.And("I select an ATM withdraw", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 23
 testRunner.And("I click on the Save button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 24
 testRunner.Then("the expenditure has been updated", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 25
 testRunner.Then("the old source account is updated", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 26
 testRunner.Then("the target atm withdraw is updated", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 27
 testRunner.Then("a mouvement entry has been saved", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Edit the amount and the payment method to Internal Transfer Expenditures")]
        [NUnit.Framework.CategoryAttribute("editcommonexpenditure")]
        public virtual void EditTheAmountAndThePaymentMethodToInternalTransferExpenditures()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Edit the amount and the payment method to Internal Transfer Expenditures", new string[] {
                        "editcommonexpenditure"});
#line 30
this.ScenarioSetup(scenarioInfo);
#line 31
 testRunner.Given("I have accessed the Expenditures List page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 32
 testRunner.And("I have at least one expenditure with this payment method in the list", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 33
 testRunner.When("I click on edit for this expenditure", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 34
 testRunner.And("I edit the Cost", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 35
 testRunner.And("I change the payment method to Internal Transfer Expenditures", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 36
 testRunner.And("I select a target account", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 37
 testRunner.And("I click on the Save button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 38
 testRunner.Then("the expenditure has been updated", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 39
 testRunner.Then("the source account is updated", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 40
 testRunner.Then("the target account is updated", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 41
 testRunner.Then("an income has been updated", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 42
 testRunner.Then("a mouvement entry has been saved", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
