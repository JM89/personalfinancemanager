using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Entities.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.DataAccess.Repositories.Interfaces
{
    public interface IHistoricMovementRepository : IBaseRepository<HistoricMovementModel>
    {
        //void SaveDebitMovement(int targetId, decimal cost, TargetOptions targetOption, MovementType movementType);

        //void SaveCreditMovement(int targetId, decimal cost, TargetOptions targetOption, MovementType movementType);
    }
}
