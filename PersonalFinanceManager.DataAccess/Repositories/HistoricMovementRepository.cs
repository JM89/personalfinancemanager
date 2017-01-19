using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Entities.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.DataAccess.Repositories
{
    public class HistoricMovementRepository : BaseRepository<HistoricMovementModel>, IHistoricMovementRepository
    {
        public HistoricMovementRepository(ApplicationDbContext db) : base(db)
        {

        }

        //public void SaveDebitMovement(int targetId, decimal cost, TargetOptions targetOption, MovementType movementType)
        //{
        //    var historicMovement = new HistoricMovementModel()
        //    {
        //        Cost = -cost,
        //        Date = DateTime.Now
        //    };

        //    if (targetOption == TargetOptions.Account)
        //    {
        //        historicMovement.AccountId = targetId;
        //    }
        //    else
        //    {
        //        historicMovement.AtmWithdrawId = targetId;
        //    }

        //    Create(historicMovement);
        //}

        //public void SaveCreditMovement(int targetId, decimal cost, TargetOptions targetOption, MovementType movementType)
        //{

        //    var historicMovement = new HistoricMovementModel()
        //    {
        //        Cost = cost,
        //        Date = DateTime.Now
        //    };

        //    if (targetOption == TargetOptions.Account)
        //    {
        //        historicMovement.AccountId = targetId;
        //    }
        //    else
        //    {
        //        historicMovement.AtmWithdrawId = targetId;
        //    }

        //    Create(historicMovement);
        //}
    }
}
