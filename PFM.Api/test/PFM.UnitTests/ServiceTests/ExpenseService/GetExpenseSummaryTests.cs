using System;
using System.Collections.Generic;
using System.Linq;
using PFM.DataAccessLayer.Entities;
using PFM.Api.Contracts.ExpenseType;
using PFM.Services.Utils.Helpers;
using PFM.UnitTests.Fixture;
using PFM.UnitTests.Helpers;
using Xunit;

namespace PFM.UnitTests.ServiceTests.ExpenseService
{
    public class GetExpenseSummaryTests : BaseTests, IClassFixture<AutoMapperFixture>
    {
        [Fact]
        public void WithExpenses_Test()
        {
            // Arrange

            var account = AccountHelper.CreateAccountModel(1);
            var typeEntities = new List<ExpenseType>
            {
                ExpenseTypeHelper.CreateExpenseTypeModel(1, "Food"),
                ExpenseTypeHelper.CreateExpenseTypeModel(2, "Energy")
            };
            var i = 1;
            var now = DateTime.Now;
            var oneMonthAgo = DateTime.Now.AddMonths(-1);
            var twoMonthsAgo = DateTime.Now.AddMonths(-2);
            var nowName = DateTimeFormatHelper.GetMonthNameAndYear(now);
            var oneMonthAgoName = DateTimeFormatHelper.GetMonthNameAndYear(oneMonthAgo);
            var twoMonthsAgoName = DateTimeFormatHelper.GetMonthNameAndYear(twoMonthsAgo);
            var threeMonthsAgoName = DateTimeFormatHelper.GetMonthNameAndYear(DateTime.Now.AddMonths(-3));
            var expenses = new List<Expense>()
            {
                ExpenseHelper.CreateExpenseModel(i++, now, 100, 1),
                ExpenseHelper.CreateExpenseModel(i++, now, 200, 2),
                ExpenseHelper.CreateExpenseModel(i++, now, 300, 2),
                ExpenseHelper.CreateExpenseModel(i++, oneMonthAgo, 200, 1),
                ExpenseHelper.CreateExpenseModel(i++, oneMonthAgo.AddDays(-1), 100, 2),
                ExpenseHelper.CreateExpenseModel(i++, oneMonthAgo.AddDays(-2), 150, 2),
                ExpenseHelper.CreateExpenseModel(i++, twoMonthsAgo, 300, 2),
                ExpenseHelper.CreateExpenseModel(i++, twoMonthsAgo.AddDays(-2), 100, 1),
                ExpenseHelper.CreateExpenseModel(i, twoMonthsAgo.AddDays(-1), 600, 2)
            };
            var incomes = new List<Income>();
            var savings = new List<Saving>();

            var service = SetupExpenseService(account, typeEntities, expenses, incomes, savings);

            // Act

            var result = service.GetExpenseSummary(1, null);

            // Assert
            Assert.True(result.HasExpenses);
            Assert.False(result.HasCurrentBudgetPlan);

            Assert.Equal(1450, result.TotalExpensesOver12Months);
            Assert.Equal(725, result.AverageExpenses);
            Assert.Equal(0, result.AverageIncomes);
            Assert.Equal(0, result.AverageSavings);

            Assert.Equal(13, result.DetailedExpensesOver12Months.Count);
            Assert.Equal(600, result.DetailedExpensesOver12Months[nowName].ExpenseValue);
            Assert.Equal(450, result.DetailedExpensesOver12Months[oneMonthAgoName].ExpenseValue);
            Assert.Equal(1000, result.DetailedExpensesOver12Months[twoMonthsAgoName].ExpenseValue);
            Assert.Equal(0, result.DetailedExpensesOver12Months[threeMonthsAgoName].ExpenseValue);

            Assert.Equal(7, result.DetailedMovementsOver6Months.Count);
            Assert.Equal(600, result.DetailedMovementsOver6Months[nowName].ExpenseValue);
            Assert.Equal(450, result.DetailedMovementsOver6Months[oneMonthAgoName].ExpenseValue);
            Assert.Equal(1000, result.DetailedMovementsOver6Months[twoMonthsAgoName].ExpenseValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[threeMonthsAgoName].ExpenseValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[nowName].IncomeValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[oneMonthAgoName].IncomeValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[twoMonthsAgoName].IncomeValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[threeMonthsAgoName].IncomeValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[nowName].SavingValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[oneMonthAgoName].SavingValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[twoMonthsAgoName].SavingValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[threeMonthsAgoName].SavingValue);

            Assert.Equal(2, result.ExpensesByCategory.Count);

            var c1 = result.ExpensesByCategory.Single(x => x.CategoryId == 1);
            Assert.Equal(150, c1.AverageCostOver12Months);
            Assert.Equal(100, c1.CostCurrentMonth);
            Assert.Equal(300, c1.CostOver12Month);
            Assert.Equal(0, c1.CostPlannedMonthly);
            Assert.Equal(200, c1.CostPreviousMonth);
            Assert.Equal(13, c1.ExpensesByMonth.Count);
            Assert.Equal(100, c1.ExpensesByMonth[nowName].CategoryExpenses);
            Assert.Equal(200, c1.ExpensesByMonth[oneMonthAgoName].CategoryExpenses);
            Assert.Equal(100, c1.ExpensesByMonth[twoMonthsAgoName].CategoryExpenses);
            Assert.Equal(0, c1.ExpensesByMonth[threeMonthsAgoName].CategoryExpenses);

            var c2 = result.ExpensesByCategory.Single(x => x.CategoryId == 2);
            Assert.Equal(575, c2.AverageCostOver12Months);
            Assert.Equal(500, c2.CostCurrentMonth);
            Assert.Equal(1150, c2.CostOver12Month);
            Assert.Equal(0, c2.CostPlannedMonthly);
            Assert.Equal(250, c2.CostPreviousMonth);
            Assert.Equal(13, c2.ExpensesByMonth.Count);
            Assert.Equal(500, c2.ExpensesByMonth[nowName].CategoryExpenses);
            Assert.Equal(250, c2.ExpensesByMonth[oneMonthAgoName].CategoryExpenses);
            Assert.Equal(900, c2.ExpensesByMonth[twoMonthsAgoName].CategoryExpenses);
            Assert.Equal(0, c2.ExpensesByMonth[threeMonthsAgoName].CategoryExpenses);
        }

        [Fact]
        public void WithExpensesIncomesAndSavings_Test()
        {
            // Arrange

            var account = AccountHelper.CreateAccountModel(1);
            var typeEntities = new List<ExpenseType>
            {
                ExpenseTypeHelper.CreateExpenseTypeModel(1, "Food"),
                ExpenseTypeHelper.CreateExpenseTypeModel(2, "Energy")
            };
            var i = 1;
            var now = DateTime.Now;
            var oneMonthAgo = DateTime.Now.AddMonths(-1);
            var twoMonthsAgo = DateTime.Now.AddMonths(-2);
            var nowName = DateTimeFormatHelper.GetMonthNameAndYear(now);
            var oneMonthAgoName = DateTimeFormatHelper.GetMonthNameAndYear(oneMonthAgo);
            var twoMonthsAgoName = DateTimeFormatHelper.GetMonthNameAndYear(twoMonthsAgo);
            var threeMonthsAgoName = DateTimeFormatHelper.GetMonthNameAndYear(DateTime.Now.AddMonths(-3));
            var expenses = new List<Expense>()
            {
                ExpenseHelper.CreateExpenseModel(i++, now, 100, 1),
                ExpenseHelper.CreateExpenseModel(i++, now, 200, 2),
                ExpenseHelper.CreateExpenseModel(i++, now, 300, 2),
                ExpenseHelper.CreateExpenseModel(i++, oneMonthAgo, 200, 1),
                ExpenseHelper.CreateExpenseModel(i++, oneMonthAgo.AddDays(-1), 100, 2),
                ExpenseHelper.CreateExpenseModel(i++, oneMonthAgo.AddDays(-2), 150, 2),
                ExpenseHelper.CreateExpenseModel(i++, twoMonthsAgo, 300, 2),
                ExpenseHelper.CreateExpenseModel(i++, twoMonthsAgo.AddDays(-2), 100, 1),
                ExpenseHelper.CreateExpenseModel(i, twoMonthsAgo.AddDays(-1), 600, 2)
            };
            var incomes = new List<Income>()
            {
                IncomeHelper.CreateIncomeModel(1, now, 1000, 1),
                IncomeHelper.CreateIncomeModel(2, now.AddDays(-1), 300, 1),
                IncomeHelper.CreateIncomeModel(3, oneMonthAgo, 1200, 1),
                IncomeHelper.CreateIncomeModel(4, twoMonthsAgo, 1400, 1)
            };
            var savings = new List<Saving>()
            {
                SavingHelper.CreateSavingModel(1, now, 500, 1),
                SavingHelper.CreateSavingModel(2, oneMonthAgo, 700, 1),
                SavingHelper.CreateSavingModel(3, twoMonthsAgo, 600, 1)
            };

            var service = SetupExpenseService(account, typeEntities, expenses, incomes, savings);

            // Act

            var result = service.GetExpenseSummary(1, null);

            // Assert
            Assert.True(result.HasExpenses);
            Assert.False(result.HasCurrentBudgetPlan);

            Assert.Equal(1450, result.TotalExpensesOver12Months);
            Assert.Equal(725, result.AverageExpenses);
            Assert.Equal(1300, result.AverageIncomes);
            Assert.Equal(650, result.AverageSavings);

            Assert.Equal(13, result.DetailedExpensesOver12Months.Count);
            Assert.Equal(600, result.DetailedExpensesOver12Months[nowName].ExpenseValue);
            Assert.Equal(450, result.DetailedExpensesOver12Months[oneMonthAgoName].ExpenseValue);
            Assert.Equal(1000, result.DetailedExpensesOver12Months[twoMonthsAgoName].ExpenseValue);
            Assert.Equal(0, result.DetailedExpensesOver12Months[threeMonthsAgoName].ExpenseValue);

            Assert.Equal(7, result.DetailedMovementsOver6Months.Count);
            Assert.Equal(600, result.DetailedMovementsOver6Months[nowName].ExpenseValue);
            Assert.Equal(450, result.DetailedMovementsOver6Months[oneMonthAgoName].ExpenseValue);
            Assert.Equal(1000, result.DetailedMovementsOver6Months[twoMonthsAgoName].ExpenseValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[threeMonthsAgoName].ExpenseValue);
            Assert.Equal(1300, result.DetailedMovementsOver6Months[nowName].IncomeValue);
            Assert.Equal(1200, result.DetailedMovementsOver6Months[oneMonthAgoName].IncomeValue);
            Assert.Equal(1400, result.DetailedMovementsOver6Months[twoMonthsAgoName].IncomeValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[threeMonthsAgoName].IncomeValue);
            Assert.Equal(500, result.DetailedMovementsOver6Months[nowName].SavingValue);
            Assert.Equal(700, result.DetailedMovementsOver6Months[oneMonthAgoName].SavingValue);
            Assert.Equal(600, result.DetailedMovementsOver6Months[twoMonthsAgoName].SavingValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[threeMonthsAgoName].SavingValue);
        }

        [Fact]
        public void WithExpensesForOneCategory_Test()
        {
            // Arrange

            var account = AccountHelper.CreateAccountModel(1);
            var typeEntities = new List<ExpenseType>
            {
                ExpenseTypeHelper.CreateExpenseTypeModel(1, "Food"),
                ExpenseTypeHelper.CreateExpenseTypeModel(2, "Energy")
            };
            var i = 1;
            var now = DateTime.Now;
            var oneMonthAgo = DateTime.Now.AddMonths(-1);
            var twoMonthsAgo = DateTime.Now.AddMonths(-2);
            var nowName = DateTimeFormatHelper.GetMonthNameAndYear(now);
            var oneMonthAgoName = DateTimeFormatHelper.GetMonthNameAndYear(oneMonthAgo);
            var twoMonthsAgoName = DateTimeFormatHelper.GetMonthNameAndYear(twoMonthsAgo);
            var threeMonthsAgoName = DateTimeFormatHelper.GetMonthNameAndYear(DateTime.Now.AddMonths(-3));
            var expenses = new List<Expense>()
            {
                ExpenseHelper.CreateExpenseModel(i++, now, 100, 1),
                ExpenseHelper.CreateExpenseModel(i++, oneMonthAgo, 200, 1),
                ExpenseHelper.CreateExpenseModel(i, twoMonthsAgo.AddDays(-2), 100, 1)
            };
            var incomes = new List<Income>();
            var savings = new List<Saving>();

            var service = SetupExpenseService(account, typeEntities, expenses, incomes, savings);

            // Act

            var result = service.GetExpenseSummary(1, null);

            // Assert
            Assert.True(result.HasExpenses);
            Assert.False(result.HasCurrentBudgetPlan);

            Assert.Equal(300, result.TotalExpensesOver12Months);
            Assert.Equal(150, result.AverageExpenses);
            Assert.Equal(0, result.AverageIncomes);
            Assert.Equal(0, result.AverageSavings);

            Assert.Equal(13, result.DetailedExpensesOver12Months.Count);
            Assert.Equal(100, result.DetailedExpensesOver12Months[nowName].ExpenseValue);
            Assert.Equal(200, result.DetailedExpensesOver12Months[oneMonthAgoName].ExpenseValue);
            Assert.Equal(100, result.DetailedExpensesOver12Months[twoMonthsAgoName].ExpenseValue);
            Assert.Equal(0, result.DetailedExpensesOver12Months[threeMonthsAgoName].ExpenseValue);

            Assert.Equal(7, result.DetailedMovementsOver6Months.Count);
            Assert.Equal(100, result.DetailedMovementsOver6Months[nowName].ExpenseValue);
            Assert.Equal(200, result.DetailedMovementsOver6Months[oneMonthAgoName].ExpenseValue);
            Assert.Equal(100, result.DetailedMovementsOver6Months[twoMonthsAgoName].ExpenseValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[threeMonthsAgoName].ExpenseValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[nowName].IncomeValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[oneMonthAgoName].IncomeValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[twoMonthsAgoName].IncomeValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[threeMonthsAgoName].IncomeValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[nowName].SavingValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[oneMonthAgoName].SavingValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[twoMonthsAgoName].SavingValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[threeMonthsAgoName].SavingValue);

            Assert.Single(result.ExpensesByCategory);

            var c1 = result.ExpensesByCategory.Single(x => x.CategoryId == 1);
            Assert.Equal(150, c1.AverageCostOver12Months);
            Assert.Equal(100, c1.CostCurrentMonth);
            Assert.Equal(300, c1.CostOver12Month);
            Assert.Equal(0, c1.CostPlannedMonthly);
            Assert.Equal(200, c1.CostPreviousMonth);
            Assert.Equal(13, c1.ExpensesByMonth.Count);
            Assert.Equal(100, c1.ExpensesByMonth[nowName].CategoryExpenses);
            Assert.Equal(200, c1.ExpensesByMonth[oneMonthAgoName].CategoryExpenses);
            Assert.Equal(100, c1.ExpensesByMonth[twoMonthsAgoName].CategoryExpenses);
            Assert.Equal(0, c1.ExpensesByMonth[threeMonthsAgoName].CategoryExpenses);
        }

        [Fact]
        public void WithExpensesAndBudgetPlan_Test()
        {
            // Arrange

            var account = AccountHelper.CreateAccountModel(1);
            var typeEntities = new List<ExpenseType>
            {
                ExpenseTypeHelper.CreateExpenseTypeModel(1, "Food"),
                ExpenseTypeHelper.CreateExpenseTypeModel(2, "Energy")
            };
            var typesModels = new List<ExpenseTypeList>()
            {
                ExpenseTypeHelper.CreateExpenseTypeListModel(1, "Food"),
                ExpenseTypeHelper.CreateExpenseTypeListModel(2, "Energy")
            };
            var i = 1;
            var now = DateTime.Now;
            var oneMonthAgo = DateTime.Now.AddMonths(-1);
            var twoMonthsAgo = DateTime.Now.AddMonths(-2);
            var expenses = new List<Expense>()
            {
                ExpenseHelper.CreateExpenseModel(i++, now, 100, 1),
                ExpenseHelper.CreateExpenseModel(i++, now, 200, 2),
                ExpenseHelper.CreateExpenseModel(i++, now, 300, 2),
                ExpenseHelper.CreateExpenseModel(i++, oneMonthAgo, 200, 1),
                ExpenseHelper.CreateExpenseModel(i++, oneMonthAgo.AddDays(-1), 100, 2),
                ExpenseHelper.CreateExpenseModel(i++, oneMonthAgo.AddDays(-2), 150, 2),
                ExpenseHelper.CreateExpenseModel(i++, twoMonthsAgo, 300, 2),
                ExpenseHelper.CreateExpenseModel(i++, twoMonthsAgo.AddDays(-2), 100, 1),
                ExpenseHelper.CreateExpenseModel(i, twoMonthsAgo.AddDays(-1), 600, 2)
            };
            var incomes = new List<Income>();
            var savings = new List<Saving>();

            var budgetPlan = BudgetPlanHelper.CreateBudgetPlanEditModel(typesModels);

            var service = SetupExpenseService(account, typeEntities, expenses, incomes, savings);

            // Act

            var result = service.GetExpenseSummary(1, budgetPlan);

            // Assert
            Assert.True(result.HasExpenses);
            Assert.True(result.HasCurrentBudgetPlan);

            var c1 = result.ExpensesByCategory.Single(x => x.CategoryId == 1);
            Assert.Equal(100, c1.CostPlannedMonthly);

            var c2 = result.ExpensesByCategory.Single(x => x.CategoryId == 2);
            Assert.Equal(100, c2.CostPlannedMonthly);
        }

        /// <summary>
        /// Happens if a category has been created after the budget plan definition. 
        /// </summary>
        [Fact]
        public void WithExpensesAndBudgetPlanWithMissingCategory_Test()
        {
            // Arrange

            var account = AccountHelper.CreateAccountModel(1);
            var typeEntities = new List<ExpenseType>
            {
                ExpenseTypeHelper.CreateExpenseTypeModel(1, "Food"),
                ExpenseTypeHelper.CreateExpenseTypeModel(2, "Energy")
            };
            var typesModels = new List<ExpenseTypeList>()
            {
                ExpenseTypeHelper.CreateExpenseTypeListModel(1, "Food")
            };
            var i = 1;
            var now = DateTime.Now;
            var oneMonthAgo = DateTime.Now.AddMonths(-1);
            var twoMonthsAgo = DateTime.Now.AddMonths(-2);
            var expenses = new List<Expense>()
            {
                ExpenseHelper.CreateExpenseModel(i++, now, 100, 1),
                ExpenseHelper.CreateExpenseModel(i++, now, 200, 2),
                ExpenseHelper.CreateExpenseModel(i++, now, 300, 2),
                ExpenseHelper.CreateExpenseModel(i++, oneMonthAgo, 200, 1),
                ExpenseHelper.CreateExpenseModel(i++, oneMonthAgo.AddDays(-1), 100, 2),
                ExpenseHelper.CreateExpenseModel(i++, oneMonthAgo.AddDays(-2), 150, 2),
                ExpenseHelper.CreateExpenseModel(i++, twoMonthsAgo, 300, 2),
                ExpenseHelper.CreateExpenseModel(i++, twoMonthsAgo.AddDays(-2), 100, 1),
                ExpenseHelper.CreateExpenseModel(i, twoMonthsAgo.AddDays(-1), 600, 2)
            };
            var incomes = new List<Income>();
            var savings = new List<Saving>();

            var budgetPlan = BudgetPlanHelper.CreateBudgetPlanEditModel(typesModels);

            var service = SetupExpenseService(account, typeEntities, expenses, incomes, savings);

            // Act

            var result = service.GetExpenseSummary(1, budgetPlan);

            // Assert
            Assert.True(result.HasExpenses);
            Assert.True(result.HasCurrentBudgetPlan);

            var c1 = result.ExpensesByCategory.Single(x => x.CategoryId == 1);
            Assert.Equal(100, c1.CostPlannedMonthly);

            var c2 = result.ExpensesByCategory.Single(x => x.CategoryId == 2);
            Assert.Equal(0, c2.CostPlannedMonthly);
        }

        [Fact]
        public void NoCategories_Test()
        {
            // Arrange

            var account = AccountHelper.CreateAccountModel(1);
            var typeEntities = new List<ExpenseType>();
            var expenses = new List<Expense>();
            var incomes = new List<Income>();
            var savings = new List<Saving>();

            var service = SetupExpenseService(account, typeEntities, expenses, incomes, savings);

            // Act

            var result = service.GetExpenseSummary(1, null);

            // Assert
            Assert.False(result.HasCategories);
            Assert.False(result.HasCurrentBudgetPlan);

            Assert.Equal(0, result.TotalExpensesOver12Months);
            Assert.Equal(0, result.AverageExpenses);
            Assert.Equal(0, result.AverageIncomes);
            Assert.Equal(0, result.AverageSavings);

            Assert.Equal(13, result.DetailedExpensesOver12Months.Count);
            Assert.Equal(7, result.DetailedMovementsOver6Months.Count);
            Assert.Empty(result.ExpensesByCategory);
        }

        [Fact]
        public void NoExpenses_Test()
        {
            // Arrange

            var account = AccountHelper.CreateAccountModel(1);
            var typeEntities = new List<ExpenseType>
            {
                ExpenseTypeHelper.CreateExpenseTypeModel(1, "Food"),
                ExpenseTypeHelper.CreateExpenseTypeModel(2, "Energy")
            };
            var now = DateTime.Now;
            var oneMonthAgo = DateTime.Now.AddMonths(-1);
            var twoMonthsAgo = DateTime.Now.AddMonths(-2);
            var nowName = DateTimeFormatHelper.GetMonthNameAndYear(now);
            var oneMonthAgoName = DateTimeFormatHelper.GetMonthNameAndYear(oneMonthAgo);
            var twoMonthsAgoName = DateTimeFormatHelper.GetMonthNameAndYear(twoMonthsAgo);
            var threeMonthsAgoName = DateTimeFormatHelper.GetMonthNameAndYear(DateTime.Now.AddMonths(-3));
            var expenses = new List<Expense>();
            var incomes = new List<Income>();
            var savings = new List<Saving>();

            var service = SetupExpenseService(account, typeEntities, expenses, incomes, savings);

            // Act

            var result = service.GetExpenseSummary(1, null);

            // Assert

            Assert.False(result.HasCurrentBudgetPlan);
            Assert.False(result.HasExpenses);

            Assert.Equal(0, result.TotalExpensesOver12Months);
            Assert.Equal(0, result.AverageExpenses);
            Assert.Equal(0, result.AverageIncomes);
            Assert.Equal(0, result.AverageSavings);

            Assert.Equal(13, result.DetailedExpensesOver12Months.Count);
            Assert.Equal(0, result.DetailedExpensesOver12Months[nowName].ExpenseValue);
            Assert.Equal(0, result.DetailedExpensesOver12Months[oneMonthAgoName].ExpenseValue);
            Assert.Equal(0, result.DetailedExpensesOver12Months[twoMonthsAgoName].ExpenseValue);
            Assert.Equal(0, result.DetailedExpensesOver12Months[threeMonthsAgoName].ExpenseValue);

            Assert.Equal(7, result.DetailedMovementsOver6Months.Count);
            Assert.Equal(0, result.DetailedMovementsOver6Months[nowName].ExpenseValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[oneMonthAgoName].ExpenseValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[twoMonthsAgoName].ExpenseValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[threeMonthsAgoName].ExpenseValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[nowName].IncomeValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[oneMonthAgoName].IncomeValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[twoMonthsAgoName].IncomeValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[threeMonthsAgoName].IncomeValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[nowName].SavingValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[oneMonthAgoName].SavingValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[twoMonthsAgoName].SavingValue);
            Assert.Equal(0, result.DetailedMovementsOver6Months[threeMonthsAgoName].SavingValue);

            Assert.Equal(2, result.ExpensesByCategory.Count);

            var c1 = result.ExpensesByCategory.Single(x => x.CategoryId == 1);
            Assert.Equal(0, c1.AverageCostOver12Months);
            Assert.Equal(0, c1.CostCurrentMonth);
            Assert.Equal(0, c1.CostOver12Month);
            Assert.Equal(0, c1.CostPlannedMonthly);
            Assert.Equal(0, c1.CostPreviousMonth);
            Assert.Equal(13, c1.ExpensesByMonth.Count);
            Assert.Equal(0, c1.ExpensesByMonth[nowName].CategoryExpenses);
            Assert.Equal(0, c1.ExpensesByMonth[oneMonthAgoName].CategoryExpenses);
            Assert.Equal(0, c1.ExpensesByMonth[twoMonthsAgoName].CategoryExpenses);
            Assert.Equal(0, c1.ExpensesByMonth[threeMonthsAgoName].CategoryExpenses);

            var c2 = result.ExpensesByCategory.Single(x => x.CategoryId == 2);
            Assert.Equal(0, c2.AverageCostOver12Months);
            Assert.Equal(0, c2.CostCurrentMonth);
            Assert.Equal(0, c2.CostOver12Month);
            Assert.Equal(0, c2.CostPlannedMonthly);
            Assert.Equal(0, c2.CostPreviousMonth);
            Assert.Equal(13, c2.ExpensesByMonth.Count);
            Assert.Equal(0, c2.ExpensesByMonth[nowName].CategoryExpenses);
            Assert.Equal(0, c2.ExpensesByMonth[oneMonthAgoName].CategoryExpenses);
            Assert.Equal(0, c2.ExpensesByMonth[twoMonthsAgoName].CategoryExpenses);
            Assert.Equal(0, c2.ExpensesByMonth[threeMonthsAgoName].CategoryExpenses);
        }
    }
}