using System;
using System.Collections.Generic;

namespace SolExB2BApiDemo.Model;

public class DocumentApi
{
    public long? B2BId { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool DownloadedByClient { get; set; }

    public string ForeignName { get; set; }

    public bool HasPdf { get; set; }

    public string Id { get; set; }

    public string Name { get; set; }

    public string NameFromClient { get; set; }

    public bool Paid { get; set; }

    public string Status { get; set; }

    public string Type { get; set; }

    /// <summary>
    /// key - waybill number, value - link
    /// </summary>
    public List<KeyValuePairApi> Waybills { get; set; }
}