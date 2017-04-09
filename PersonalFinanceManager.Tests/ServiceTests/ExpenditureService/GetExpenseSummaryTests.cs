using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Models.BudgetPlan;
using PersonalFinanceManager.Models.ExpenditureType;
using PersonalFinanceManager.Tests.Helpers;
using PersonalFinanceManager.Utils.Utils;

namespace PersonalFinanceManager.Tests.ServiceTests.ExpenditureService
{
    [TestClass]
    public class GetExpenseSummaryTests : BaseTests
    {
        [TestMethod]
        public void WithExpenses_Test()
        {
            // Arrange

            var account = AccountHelper.CreateAccountModel(1);
            var typeEntities = new List<ExpenditureTypeModel>
            {
                ExpenditureTypeHelper.CreateExpenditureTypeModel(1, "Food"),
                ExpenditureTypeHelper.CreateExpenditureTypeModel(2, "Energy")
            };
            var i = 1;
            var now = DateTime.Now;
            var oneMonthAgo = DateTime.Now.AddMonths(-1);
            var twoMonthsAgo = DateTime.Now.AddMonths(-2);
            var nowName = DateTimeFormatHelper.GetMonthNameAndYear(now);
            var oneMonthAgoName = DateTimeFormatHelper.GetMonthNameAndYear(oneMonthAgo);
            var twoMonthsAgoName = DateTimeFormatHelper.GetMonthNameAndYear(twoMonthsAgo);
            var threeMonthsAgoName = DateTimeFormatHelper.GetMonthNameAndYear(DateTime.Now.AddMonths(-3));
            var expenses = new List<ExpenditureModel>()
            {
                ExpenditureHelper.CreateExpenditureModel(i++, now, 100, 1),
                ExpenditureHelper.CreateExpenditureModel(i++, now, 200, 2),
                ExpenditureHelper.CreateExpenditureModel(i++, now, 300, 2),
                ExpenditureHelper.CreateExpenditureModel(i++, oneMonthAgo, 200, 1),
                ExpenditureHelper.CreateExpenditureModel(i++, oneMonthAgo.AddDays(-1), 100, 2),
                ExpenditureHelper.CreateExpenditureModel(i++, oneMonthAgo.AddDays(-2), 150, 2),
                ExpenditureHelper.CreateExpenditureModel(i++, twoMonthsAgo, 300, 2),
                ExpenditureHelper.CreateExpenditureModel(i++, twoMonthsAgo.AddDays(-2), 100, 1),
                ExpenditureHelper.CreateExpenditureModel(i, twoMonthsAgo.AddDays(-1), 600, 2)
            };
            var incomes = new List<IncomeModel>();
            var savings = new List<SavingModel>();

            var service = SetupExpenditureService(account, typeEntities, expenses, incomes, savings);

            // Act

            var result = service.GetExpenseSummary(1, null);

            // Assert
            Assert.IsTrue(result.HasExpenses);
            Assert.IsFalse(result.HasCurrentBudgetPlan);

            Assert.AreEqual(1450, result.TotalExpensesOver12Months);
            Assert.AreEqual(725, result.AverageExpenses);
            Assert.AreEqual(0, result.AverageIncomes);
            Assert.AreEqual(0, result.AverageSavings);

            Assert.AreEqual(13, result.DetailedExpensesOver12Months.Count);
            Assert.AreEqual(600, result.DetailedExpensesOver12Months[nowName].ExpenseValue);
            Assert.AreEqual(450, result.DetailedExpensesOver12Months[oneMonthAgoName].ExpenseValue);
            Assert.AreEqual(1000, result.DetailedExpensesOver12Months[twoMonthsAgoName].ExpenseValue);
            Assert.AreEqual(0, result.DetailedExpensesOver12Months[threeMonthsAgoName].ExpenseValue);

            Assert.AreEqual(7, result.DetailedMovementsOver6Months.Count);
            Assert.AreEqual(600, result.DetailedMovementsOver6Months[nowName].ExpenseValue);
            Assert.AreEqual(450, result.DetailedMovementsOver6Months[oneMonthAgoName].ExpenseValue);
            Assert.AreEqual(1000, result.DetailedMovementsOver6Months[twoMonthsAgoName].ExpenseValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[threeMonthsAgoName].ExpenseValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[nowName].IncomeValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[oneMonthAgoName].IncomeValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[twoMonthsAgoName].IncomeValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[threeMonthsAgoName].IncomeValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[nowName].SavingValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[oneMonthAgoName].SavingValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[twoMonthsAgoName].SavingValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[threeMonthsAgoName].SavingValue);

            Assert.AreEqual(2, result.ExpensesByCategory.Count);

            var c1 = result.ExpensesByCategory.Single(x => x.CategoryId == 1);
            Assert.AreEqual(150, c1.AverageCostOver12Months);
            Assert.AreEqual(100, c1.CostCurrentMonth);
            Assert.AreEqual(300, c1.CostOver12Month);
            Assert.AreEqual(0, c1.CostPlannedMonthly);
            Assert.AreEqual(200, c1.CostPreviousMonth);
            Assert.AreEqual(13, c1.ExpensesByMonth.Count);
            Assert.AreEqual(100, c1.ExpensesByMonth[nowName].CategoryExpenses);
            Assert.AreEqual(200, c1.ExpensesByMonth[oneMonthAgoName].CategoryExpenses);
            Assert.AreEqual(100, c1.ExpensesByMonth[twoMonthsAgoName].CategoryExpenses);
            Assert.AreEqual(0, c1.ExpensesByMonth[threeMonthsAgoName].CategoryExpenses);

            var c2 = result.ExpensesByCategory.Single(x => x.CategoryId == 2);
            Assert.AreEqual(575, c2.AverageCostOver12Months);
            Assert.AreEqual(500, c2.CostCurrentMonth);
            Assert.AreEqual(1150, c2.CostOver12Month);
            Assert.AreEqual(0, c2.CostPlannedMonthly);
            Assert.AreEqual(250, c2.CostPreviousMonth);
            Assert.AreEqual(13, c2.ExpensesByMonth.Count);
            Assert.AreEqual(500, c2.ExpensesByMonth[nowName].CategoryExpenses);
            Assert.AreEqual(250, c2.ExpensesByMonth[oneMonthAgoName].CategoryExpenses);
            Assert.AreEqual(900, c2.ExpensesByMonth[twoMonthsAgoName].CategoryExpenses);
            Assert.AreEqual(0, c2.ExpensesByMonth[threeMonthsAgoName].CategoryExpenses);
        }

        [TestMethod]
        public void WithExpensesIncomesAndSavings_Test()
        {
            // Arrange

            var account = AccountHelper.CreateAccountModel(1);
            var typeEntities = new List<ExpenditureTypeModel>
            {
                ExpenditureTypeHelper.CreateExpenditureTypeModel(1, "Food"),
                ExpenditureTypeHelper.CreateExpenditureTypeModel(2, "Energy")
            };
            var i = 1;
            var now = DateTime.Now;
            var oneMonthAgo = DateTime.Now.AddMonths(-1);
            var twoMonthsAgo = DateTime.Now.AddMonths(-2);
            var nowName = DateTimeFormatHelper.GetMonthNameAndYear(now);
            var oneMonthAgoName = DateTimeFormatHelper.GetMonthNameAndYear(oneMonthAgo);
            var twoMonthsAgoName = DateTimeFormatHelper.GetMonthNameAndYear(twoMonthsAgo);
            var threeMonthsAgoName = DateTimeFormatHelper.GetMonthNameAndYear(DateTime.Now.AddMonths(-3));
            var expenses = new List<ExpenditureModel>()
            {
                ExpenditureHelper.CreateExpenditureModel(i++, now, 100, 1),
                ExpenditureHelper.CreateExpenditureModel(i++, now, 200, 2),
                ExpenditureHelper.CreateExpenditureModel(i++, now, 300, 2),
                ExpenditureHelper.CreateExpenditureModel(i++, oneMonthAgo, 200, 1),
                ExpenditureHelper.CreateExpenditureModel(i++, oneMonthAgo.AddDays(-1), 100, 2),
                ExpenditureHelper.CreateExpenditureModel(i++, oneMonthAgo.AddDays(-2), 150, 2),
                ExpenditureHelper.CreateExpenditureModel(i++, twoMonthsAgo, 300, 2),
                ExpenditureHelper.CreateExpenditureModel(i++, twoMonthsAgo.AddDays(-2), 100, 1),
                ExpenditureHelper.CreateExpenditureModel(i, twoMonthsAgo.AddDays(-1), 600, 2)
            };
            var incomes = new List<IncomeModel>()
            {
                IncomeHelper.CreateIncomeModel(1, now, 1000, 1),
                IncomeHelper.CreateIncomeModel(2, now.AddDays(-1), 300, 1),
                IncomeHelper.CreateIncomeModel(3, oneMonthAgo, 1200, 1),
                IncomeHelper.CreateIncomeModel(4, twoMonthsAgo, 1400, 1)
            };
            var savings = new List<SavingModel>()
            {
                SavingHelper.CreateSavingModel(1, now, 500, 1),
                SavingHelper.CreateSavingModel(2, oneMonthAgo, 700, 1),
                SavingHelper.CreateSavingModel(3, twoMonthsAgo, 600, 1)
            };

            var service = SetupExpenditureService(account, typeEntities, expenses, incomes, savings);

            // Act

            var result = service.GetExpenseSummary(1, null);

            // Assert
            Assert.IsTrue(result.HasExpenses);
            Assert.IsFalse(result.HasCurrentBudgetPlan);

            Assert.AreEqual(1450, result.TotalExpensesOver12Months);
            Assert.AreEqual(725, result.AverageExpenses);
            Assert.AreEqual(1300, result.AverageIncomes);
            Assert.AreEqual(650, result.AverageSavings);

            Assert.AreEqual(13, result.DetailedExpensesOver12Months.Count);
            Assert.AreEqual(600, result.DetailedExpensesOver12Months[nowName].ExpenseValue);
            Assert.AreEqual(450, result.DetailedExpensesOver12Months[oneMonthAgoName].ExpenseValue);
            Assert.AreEqual(1000, result.DetailedExpensesOver12Months[twoMonthsAgoName].ExpenseValue);
            Assert.AreEqual(0, result.DetailedExpensesOver12Months[threeMonthsAgoName].ExpenseValue);

            Assert.AreEqual(7, result.DetailedMovementsOver6Months.Count);
            Assert.AreEqual(600, result.DetailedMovementsOver6Months[nowName].ExpenseValue);
            Assert.AreEqual(450, result.DetailedMovementsOver6Months[oneMonthAgoName].ExpenseValue);
            Assert.AreEqual(1000, result.DetailedMovementsOver6Months[twoMonthsAgoName].ExpenseValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[threeMonthsAgoName].ExpenseValue);
            Assert.AreEqual(1300, result.DetailedMovementsOver6Months[nowName].IncomeValue);
            Assert.AreEqual(1200, result.DetailedMovementsOver6Months[oneMonthAgoName].IncomeValue);
            Assert.AreEqual(1400, result.DetailedMovementsOver6Months[twoMonthsAgoName].IncomeValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[threeMonthsAgoName].IncomeValue);
            Assert.AreEqual(500, result.DetailedMovementsOver6Months[nowName].SavingValue);
            Assert.AreEqual(700, result.DetailedMovementsOver6Months[oneMonthAgoName].SavingValue);
            Assert.AreEqual(600, result.DetailedMovementsOver6Months[twoMonthsAgoName].SavingValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[threeMonthsAgoName].SavingValue);
        }

        [TestMethod]
        public void WithExpensesForOneCategory_Test()
        {
            // Arrange

            var account = AccountHelper.CreateAccountModel(1);
            var typeEntities = new List<ExpenditureTypeModel>
            {
                ExpenditureTypeHelper.CreateExpenditureTypeModel(1, "Food"),
                ExpenditureTypeHelper.CreateExpenditureTypeModel(2, "Energy")
            };
            var i = 1;
            var now = DateTime.Now;
            var oneMonthAgo = DateTime.Now.AddMonths(-1);
            var twoMonthsAgo = DateTime.Now.AddMonths(-2);
            var nowName = DateTimeFormatHelper.GetMonthNameAndYear(now);
            var oneMonthAgoName = DateTimeFormatHelper.GetMonthNameAndYear(oneMonthAgo);
            var twoMonthsAgoName = DateTimeFormatHelper.GetMonthNameAndYear(twoMonthsAgo);
            var threeMonthsAgoName = DateTimeFormatHelper.GetMonthNameAndYear(DateTime.Now.AddMonths(-3));
            var expenses = new List<ExpenditureModel>()
            {
                ExpenditureHelper.CreateExpenditureModel(i++, now, 100, 1),
                ExpenditureHelper.CreateExpenditureModel(i++, oneMonthAgo, 200, 1),
                ExpenditureHelper.CreateExpenditureModel(i, twoMonthsAgo.AddDays(-2), 100, 1)
            };
            var incomes = new List<IncomeModel>();
            var savings = new List<SavingModel>();

            var service = SetupExpenditureService(account, typeEntities, expenses, incomes, savings);

            // Act

            var result = service.GetExpenseSummary(1, null);

            // Assert
            Assert.IsTrue(result.HasExpenses);
            Assert.IsFalse(result.HasCurrentBudgetPlan);

            Assert.AreEqual(300, result.TotalExpensesOver12Months);
            Assert.AreEqual(150, result.AverageExpenses);
            Assert.AreEqual(0, result.AverageIncomes);
            Assert.AreEqual(0, result.AverageSavings);

            Assert.AreEqual(13, result.DetailedExpensesOver12Months.Count);
            Assert.AreEqual(100, result.DetailedExpensesOver12Months[nowName].ExpenseValue);
            Assert.AreEqual(200, result.DetailedExpensesOver12Months[oneMonthAgoName].ExpenseValue);
            Assert.AreEqual(100, result.DetailedExpensesOver12Months[twoMonthsAgoName].ExpenseValue);
            Assert.AreEqual(0, result.DetailedExpensesOver12Months[threeMonthsAgoName].ExpenseValue);

            Assert.AreEqual(7, result.DetailedMovementsOver6Months.Count);
            Assert.AreEqual(100, result.DetailedMovementsOver6Months[nowName].ExpenseValue);
            Assert.AreEqual(200, result.DetailedMovementsOver6Months[oneMonthAgoName].ExpenseValue);
            Assert.AreEqual(100, result.DetailedMovementsOver6Months[twoMonthsAgoName].ExpenseValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[threeMonthsAgoName].ExpenseValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[nowName].IncomeValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[oneMonthAgoName].IncomeValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[twoMonthsAgoName].IncomeValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[threeMonthsAgoName].IncomeValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[nowName].SavingValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[oneMonthAgoName].SavingValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[twoMonthsAgoName].SavingValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[threeMonthsAgoName].SavingValue);

            Assert.AreEqual(1, result.ExpensesByCategory.Count);

            var c1 = result.ExpensesByCategory.Single(x => x.CategoryId == 1);
            Assert.AreEqual(150, c1.AverageCostOver12Months);
            Assert.AreEqual(100, c1.CostCurrentMonth);
            Assert.AreEqual(300, c1.CostOver12Month);
            Assert.AreEqual(0, c1.CostPlannedMonthly);
            Assert.AreEqual(200, c1.CostPreviousMonth);
            Assert.AreEqual(13, c1.ExpensesByMonth.Count);
            Assert.AreEqual(100, c1.ExpensesByMonth[nowName].CategoryExpenses);
            Assert.AreEqual(200, c1.ExpensesByMonth[oneMonthAgoName].CategoryExpenses);
            Assert.AreEqual(100, c1.ExpensesByMonth[twoMonthsAgoName].CategoryExpenses);
            Assert.AreEqual(0, c1.ExpensesByMonth[threeMonthsAgoName].CategoryExpenses);
        }

        [TestMethod]
        public void WithExpensesAndBudgetPlan_Test()
        {
            // Arrange

            var account = AccountHelper.CreateAccountModel(1);
            var typeEntities = new List<ExpenditureTypeModel>
            {
                ExpenditureTypeHelper.CreateExpenditureTypeModel(1, "Food"),
                ExpenditureTypeHelper.CreateExpenditureTypeModel(2, "Energy")
            };
            var typesModels = new List<ExpenditureTypeListModel>()
            {
                ExpenditureTypeHelper.CreateExpenditureTypeListModel(1, "Food"),
                ExpenditureTypeHelper.CreateExpenditureTypeListModel(2, "Energy")
            };
            var i = 1;
            var now = DateTime.Now;
            var oneMonthAgo = DateTime.Now.AddMonths(-1);
            var twoMonthsAgo = DateTime.Now.AddMonths(-2);
            var expenses = new List<ExpenditureModel>()
            {
                ExpenditureHelper.CreateExpenditureModel(i++, now, 100, 1),
                ExpenditureHelper.CreateExpenditureModel(i++, now, 200, 2),
                ExpenditureHelper.CreateExpenditureModel(i++, now, 300, 2),
                ExpenditureHelper.CreateExpenditureModel(i++, oneMonthAgo, 200, 1),
                ExpenditureHelper.CreateExpenditureModel(i++, oneMonthAgo.AddDays(-1), 100, 2),
                ExpenditureHelper.CreateExpenditureModel(i++, oneMonthAgo.AddDays(-2), 150, 2),
                ExpenditureHelper.CreateExpenditureModel(i++, twoMonthsAgo, 300, 2),
                ExpenditureHelper.CreateExpenditureModel(i++, twoMonthsAgo.AddDays(-2), 100, 1),
                ExpenditureHelper.CreateExpenditureModel(i, twoMonthsAgo.AddDays(-1), 600, 2)
            };
            var incomes = new List<IncomeModel>();
            var savings = new List<SavingModel>();

            var budgetPlan = BudgetPlanHelper.CreateBudgetPlanEditModel(typesModels);

            var service = SetupExpenditureService(account, typeEntities, expenses, incomes, savings);

            // Act

            var result = service.GetExpenseSummary(1, budgetPlan);

            // Assert
            Assert.IsTrue(result.HasExpenses);
            Assert.IsTrue(result.HasCurrentBudgetPlan);

            var c1 = result.ExpensesByCategory.Single(x => x.CategoryId == 1);
            Assert.AreEqual(100, c1.CostPlannedMonthly);

            var c2 = result.ExpensesByCategory.Single(x => x.CategoryId == 2);
            Assert.AreEqual(100, c2.CostPlannedMonthly);
        }

        [TestMethod]
        public void NoCategories_Test()
        {
            // Arrange

            var account = AccountHelper.CreateAccountModel(1);
            var typeEntities = new List<ExpenditureTypeModel>();
            var expenses = new List<ExpenditureModel>();
            var incomes = new List<IncomeModel>();
            var savings = new List<SavingModel>();

            var service = SetupExpenditureService(account, typeEntities, expenses, incomes, savings);

            // Act

            var result = service.GetExpenseSummary(1, null);

            // Assert
            Assert.IsFalse(result.HasCategories);
            Assert.IsFalse(result.HasCurrentBudgetPlan);

            Assert.AreEqual(0, result.TotalExpensesOver12Months);
            Assert.AreEqual(0, result.AverageExpenses);
            Assert.AreEqual(0, result.AverageIncomes);
            Assert.AreEqual(0, result.AverageSavings);

            Assert.AreEqual(13, result.DetailedExpensesOver12Months.Count);
            Assert.AreEqual(7, result.DetailedMovementsOver6Months.Count);
            Assert.AreEqual(0, result.ExpensesByCategory.Count);
        }

        [TestMethod]
        public void NoExpenses_Test()
        {
            // Arrange

            var account = AccountHelper.CreateAccountModel(1);
            var typeEntities = new List<ExpenditureTypeModel>
            {
                ExpenditureTypeHelper.CreateExpenditureTypeModel(1, "Food"),
                ExpenditureTypeHelper.CreateExpenditureTypeModel(2, "Energy")
            };
            var now = DateTime.Now;
            var oneMonthAgo = DateTime.Now.AddMonths(-1);
            var twoMonthsAgo = DateTime.Now.AddMonths(-2);
            var nowName = DateTimeFormatHelper.GetMonthNameAndYear(now);
            var oneMonthAgoName = DateTimeFormatHelper.GetMonthNameAndYear(oneMonthAgo);
            var twoMonthsAgoName = DateTimeFormatHelper.GetMonthNameAndYear(twoMonthsAgo);
            var threeMonthsAgoName = DateTimeFormatHelper.GetMonthNameAndYear(DateTime.Now.AddMonths(-3));
            var expenses = new List<ExpenditureModel>();
            var incomes = new List<IncomeModel>();
            var savings = new List<SavingModel>();

            var service = SetupExpenditureService(account, typeEntities, expenses, incomes, savings);

            // Act

            var result = service.GetExpenseSummary(1, null);

            // Assert

            Assert.IsFalse(result.HasCurrentBudgetPlan);
            Assert.IsFalse(result.HasExpenses);

            Assert.AreEqual(0, result.TotalExpensesOver12Months);
            Assert.AreEqual(0, result.AverageExpenses);
            Assert.AreEqual(0, result.AverageIncomes);
            Assert.AreEqual(0, result.AverageSavings);

            Assert.AreEqual(13, result.DetailedExpensesOver12Months.Count);
            Assert.AreEqual(0, result.DetailedExpensesOver12Months[nowName].ExpenseValue);
            Assert.AreEqual(0, result.DetailedExpensesOver12Months[oneMonthAgoName].ExpenseValue);
            Assert.AreEqual(0, result.DetailedExpensesOver12Months[twoMonthsAgoName].ExpenseValue);
            Assert.AreEqual(0, result.DetailedExpensesOver12Months[threeMonthsAgoName].ExpenseValue);

            Assert.AreEqual(7, result.DetailedMovementsOver6Months.Count);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[nowName].ExpenseValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[oneMonthAgoName].ExpenseValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[twoMonthsAgoName].ExpenseValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[threeMonthsAgoName].ExpenseValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[nowName].IncomeValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[oneMonthAgoName].IncomeValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[twoMonthsAgoName].IncomeValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[threeMonthsAgoName].IncomeValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[nowName].SavingValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[oneMonthAgoName].SavingValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[twoMonthsAgoName].SavingValue);
            Assert.AreEqual(0, result.DetailedMovementsOver6Months[threeMonthsAgoName].SavingValue);

            Assert.AreEqual(2, result.ExpensesByCategory.Count);

            var c1 = result.ExpensesByCategory.Single(x => x.CategoryId == 1);
            Assert.AreEqual(0, c1.AverageCostOver12Months);
            Assert.AreEqual(0, c1.CostCurrentMonth);
            Assert.AreEqual(0, c1.CostOver12Month);
            Assert.AreEqual(0, c1.CostPlannedMonthly);
            Assert.AreEqual(0, c1.CostPreviousMonth);
            Assert.AreEqual(13, c1.ExpensesByMonth.Count);
            Assert.AreEqual(0, c1.ExpensesByMonth[nowName].CategoryExpenses);
            Assert.AreEqual(0, c1.ExpensesByMonth[oneMonthAgoName].CategoryExpenses);
            Assert.AreEqual(0, c1.ExpensesByMonth[twoMonthsAgoName].CategoryExpenses);
            Assert.AreEqual(0, c1.ExpensesByMonth[threeMonthsAgoName].CategoryExpenses);

            var c2 = result.ExpensesByCategory.Single(x => x.CategoryId == 2);
            Assert.AreEqual(0, c2.AverageCostOver12Months);
            Assert.AreEqual(0, c2.CostCurrentMonth);
            Assert.AreEqual(0, c2.CostOver12Month);
            Assert.AreEqual(0, c2.CostPlannedMonthly);
            Assert.AreEqual(0, c2.CostPreviousMonth);
            Assert.AreEqual(13, c2.ExpensesByMonth.Count);
            Assert.AreEqual(0, c2.ExpensesByMonth[nowName].CategoryExpenses);
            Assert.AreEqual(0, c2.ExpensesByMonth[oneMonthAgoName].CategoryExpenses);
            Assert.AreEqual(0, c2.ExpensesByMonth[twoMonthsAgoName].CategoryExpenses);
            Assert.AreEqual(0, c2.ExpensesByMonth[threeMonthsAgoName].CategoryExpenses);
        }
    }
}