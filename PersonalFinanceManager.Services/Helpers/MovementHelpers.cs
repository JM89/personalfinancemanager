using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Entities.Enumerations;
using System;

namespace PersonalFinanceManager.Services.Helpers
{
    public static class MovementHelpers
    {
        public static void Debit(IHistoricMovementRepository _historicMovementRepository, decimal movementAmount, ObjectType sourceObjectType, int sourceId, decimal sourceAmount)
        {
            var historicMovement = new HistoricMovementModel();
            historicMovement.Date = DateTime.Now;
            historicMovement.Cost = -movementAmount;
            historicMovement.SourceId = sourceId;
            historicMovement.SourceType = sourceObjectType;
            historicMovement.SourceOldAmount = sourceAmount;
            historicMovement.SourceNewAmount = sourceAmount - movementAmount;
            _historicMovementRepository.Create(historicMovement);
        }

        public static void Credit(IHistoricMovementRepository _historicMovementRepository, decimal movementAmount, ObjectType sourceObjectType, int sourceId, decimal sourceAmount)
        {
            var historicMovement = new HistoricMovementModel();
            historicMovement.Date = DateTime.Now;
            historicMovement.Cost = movementAmount;
            historicMovement.SourceId = sourceId;
            historicMovement.SourceType = sourceObjectType;
            historicMovement.SourceOldAmount = sourceAmount;
            historicMovement.SourceNewAmount = sourceAmount + movementAmount;
            _historicMovementRepository.Create(historicMovement);
        }

        public static void Debit(IHistoricMovementRepository _historicMovementRepository, decimal movementAmount, int sourceId, ObjectType sourceObjectType, decimal sourceAmount, int destinationId, ObjectType destinationObjectType, decimal destinationAmount)
        {
            var historicMovement = new HistoricMovementModel();
            historicMovement.Date = DateTime.Now;
            historicMovement.Cost = -movementAmount;
            historicMovement.SourceId = sourceId;
            historicMovement.SourceType = sourceObjectType;
            historicMovement.SourceOldAmount = sourceAmount;
            historicMovement.SourceNewAmount = sourceAmount - movementAmount;
            historicMovement.DestinationId = destinationId;
            historicMovement.DestinationType = destinationObjectType;
            historicMovement.DestinationOldAmount = destinationAmount;
            historicMovement.DestinationNewAmount = destinationAmount + movementAmount;
            _historicMovementRepository.Create(historicMovement);
        }

        public static void Credit(IHistoricMovementRepository _historicMovementRepository, decimal movementAmount, int sourceId, ObjectType sourceObjectType, decimal sourceAmount, int destinationId, ObjectType destinationObjectType, decimal destinationAmount)
        {
            var historicMovement = new HistoricMovementModel();
            historicMovement.Date = DateTime.Now;
            historicMovement.Cost = movementAmount;
            historicMovement.SourceId = sourceId;
            historicMovement.SourceType = sourceObjectType;
            historicMovement.SourceOldAmount = sourceAmount;
            historicMovement.SourceNewAmount = sourceAmount + movementAmount;
            historicMovement.DestinationId = destinationId;
            historicMovement.DestinationType = destinationObjectType;
            historicMovement.DestinationOldAmount = destinationAmount;
            historicMovement.DestinationNewAmount = destinationAmount - movementAmount;
            _historicMovementRepository.Create(historicMovement);
        }
    }
}
