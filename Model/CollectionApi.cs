using System.Collections.Generic;

namespace SolExB2BApiDemo.Model;

public class CollectionApi<T>
{
    public List<T> Items { get; set; } = new List<T>();
}