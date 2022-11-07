using System.Collections.Generic;

namespace SolExB2BApiDemo.Model;

public class CollectionPagedApi<T>
{
    public int Count { get; set; }

    public bool HasMore { get; set; }

    public List<T> Items { get; set; } = new List<T>();

    public int PageNumber { get; set; }

    public int PageSize { get; set; }
}