namespace PFM.Models
{
	public class PagedModel<T>
	{
        public IList<T> List { get; }
        public int Count { get; }

        public PagedModel(IList<T> list, int count)
        {
            this.List = list;
            this.Count = count;
        }
    }
}

