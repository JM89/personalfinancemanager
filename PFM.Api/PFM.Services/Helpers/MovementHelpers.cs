using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.Enumerations;
using PFM.DataAccessLayer.Repositories.Interfaces;
using System;

namespace PFM.Services.Helpers
{
    public static class MovementHelpers
    {
        public static void Debit(IHistoricMovementRepository historicMovementRepository, decimal movementAmount, int sourceId, ObjectType sourceObjectType, decimal sourceAmount)
        {
            var historicMovement = new HistoricMovement
            {
                Date = DateTime.Now,
                Cost = -movementAmount,
                SourceId = sourceId,
                SourceType = sourceObjectType,
                SourceOldAmount = sourceAmount,
                SourceNewAmount = sourceAmount - movementAmount
            };
            historicMovementRepository.Create(historicMovement);
        }

        public static void Credit(IHistoricMovementRepository historicMovementRepository, decimal movementAmount, int sourceId, ObjectType sourceObjectType, decimal sourceAmount)
        {
            var historicMovement = new HistoricMovement
            {
                Date = DateTime.Now,
                Cost = movementAmount,
                SourceId = sourceId,
                SourceType = sourceObjectType,
                SourceOldAmount = sourceAmount,
                SourceNewAmount = sourceAmount + movementAmount
            };
            historicMovementRepository.Create(historicMovement);
        }

        public static void Debit(IHistoricMovementRepository historicMovementRepository, decimal movementAmount, int sourceId, ObjectType sourceObjectType, decimal sourceAmount, int destinationId, ObjectType destinationObjectType, decimal destinationAmount)
        {
            var historicMovement = new HistoricMovement
            {
                Date = DateTime.Now,
                Cost = -movementAmount,
                SourceId = sourceId,
                SourceType = sourceObjectType,
                SourceOldAmount = sourceAmount,
                SourceNewAmount = sourceAmount - movementAmount,
                DestinationId = destinationId,
                DestinationType = destinationObjectType,
                DestinationOldAmount = destinationAmount,
                DestinationNewAmount = destinationAmount + movementAmount
            };
            historicMovementRepository.Create(historicMovement);
        }

        public static void Credit(IHistoricMovementRepository historicMovementRepository, decimal movementAmount, int sourceId, ObjectType sourceObjectType, decimal sourceAmount, int destinationId, ObjectType destinationObjectType, decimal destinationAmount)
        {
            var historicMovement = new HistoricMovement
            {
                Date = DateTime.Now,
                Cost = movementAmount,
                SourceId = sourceId,
                SourceType = sourceObjectType,
                SourceOldAmount = sourceAmount,
                SourceNewAmount = sourceAmount + movementAmount,
                DestinationId = destinationId,
                DestinationType = destinationObjectType,
                DestinationOldAmount = destinationAmount,
                DestinationNewAmount = destinationAmount - movementAmount
            };
            historicMovementRepository.Create(historicMovement);
        }
    }
}
