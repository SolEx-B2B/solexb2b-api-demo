using System.Collections.Generic;

namespace SolExB2BApiDemo.Model;

public class PostOrderApi
{
    public List<OrderAdditionalPropertyApi> AdditionalProperties { get; set; }

    public AddressApi Address { get; set; }

    public string Comment { get; set; }

    public long? DeliveryId { get; set; }

    public string DeliveryName { get; set; }

    public string InpostPaczkomatCode { get; set; }

    public PostOrderLinesApi OrderLines { get; set; }

    public long? PaymentId { get; set; }
}
