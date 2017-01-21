using PersonalFinanceManager.DataAccess;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Entities.Enumerations;
using PersonalFinanceManager.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services.MovementStrategy
{
    public class CommonMovementStrategy : MovementStrategy
    {
        public CommonMovementStrategy(Movement movement, IBankAccountRepository _bankAccountRepository, IHistoricMovementRepository _historicMovementRepository, IIncomeRepository _incomeRepository)
            : base(movement, _bankAccountRepository, _historicMovementRepository, _incomeRepository)
        { }

        public override void Debit()
        {
            
        }

        public override void Credit()
        {
            
        }

        public override void UpdateDebit(Movement newMovement)
        {
            
        }
    }
}
