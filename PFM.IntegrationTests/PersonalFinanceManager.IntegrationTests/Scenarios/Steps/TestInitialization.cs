using System;
using NUnit.Framework;
using PersonalFinanceManager.IntegrationTests.Infrastructure;

[SetUpFixture]
public class TestInitialization
{
    [OneTimeSetUp]
    public void Setup()
    {
        BrowserHelper.Initialize();
    }

    [OneTimeTearDown]
    public void Teardown()
    {
        BrowserHelper.StopTest();
    }
}