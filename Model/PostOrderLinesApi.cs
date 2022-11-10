using System.Collections.Generic;

namespace SolExB2BApiDemo.Model;

public class PostOrderLinesApi
{
    public ApiProductKeyType KeyType { get; set; }

    public List<PostOrderLineApi> Lines { get; set; }
}