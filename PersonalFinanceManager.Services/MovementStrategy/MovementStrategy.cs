using PersonalFinanceManager.DataAccess;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services.MovementStrategy
{
    public abstract class MovementStrategy
    {
        protected readonly IBankAccountRepository _bankAccountRepository;
        protected readonly IHistoricMovementRepository _historicMovementRepository;
        protected readonly IIncomeRepository _incomeRepository;

        protected Movement _currentMovement;

        protected MovementStrategy(Movement movement, IBankAccountRepository bankAccountRepository, IHistoricMovementRepository historicMovementRepository, IIncomeRepository incomeRepository)
        { 
            _currentMovement = movement;

            this._bankAccountRepository = bankAccountRepository;
            this._historicMovementRepository = historicMovementRepository;
            this._incomeRepository = incomeRepository;
        }

        public abstract void Debit();

        public abstract void Credit();

        public abstract void UpdateDebit(Movement newMovement);
    }
}
