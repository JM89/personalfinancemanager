namespace PFM.Models;

public class PagedModel<T>(IList<T> list, int count)
{
    public IList<T> List { get; } = list;
    public int Count { get; } = count;
}