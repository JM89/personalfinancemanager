using PersonalFinanceManager.DataAccess;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Entities.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services.ExpenditureStrategy
{
    public static class HistoricMovementHelper
    {
        public static void SaveDebitMovement(ApplicationDbContext dbContext, int targetId, decimal cost, TargetOptions targetOption, MovementType movementType)
        {
            var historicMovement = new HistoricMovementModel()
            {
                Cost = -cost,
                Date = DateTime.Now, 
                MovementType = (int)movementType
            };

            if (targetOption == TargetOptions.Account)
            {
                historicMovement.AccountId = targetId;
            }
            else
            {
                historicMovement.AtmWithdrawId = targetId;
            }

            dbContext.HistoricMovementModels.Add(historicMovement);
        }

        public static void SaveCreditMovement(ApplicationDbContext dbContext, int targetId, decimal cost, TargetOptions targetOption, MovementType movementType)
        {

            var historicMovement = new HistoricMovementModel()
            {
                Cost = cost,
                Date = DateTime.Now,
                MovementType = (int)movementType
            };

            if (targetOption == TargetOptions.Account)
            {
                historicMovement.AccountId = targetId;
            }
            else
            {
                historicMovement.AtmWithdrawId = targetId;
            }

            dbContext.HistoricMovementModels.Add(historicMovement);
        }
    }
}
