using System.Collections.Generic;

namespace SolExB2BApiDemo.Model;

public class StockInfoApi
{
    public string Ean { get; set; }

    public decimal Qty { get; set; }

    public string Sku { get; set; }

    public List<UnitApi> Units { get; set; }
}