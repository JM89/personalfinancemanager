using PFM.MovementAggregator.Persistence.Entities;

namespace PFM.MovementAggregator.Persistence
{
	public interface IMovementAggregatorRepository
	{
        Task<int> Merge(MovementAggregation movementAggregation);
    }
}

