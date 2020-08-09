using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace SolExB2BApiDemo
{
    public class Product
    {
        public string name { get; set; }
        public string code { get; set; }
        public string ean { get; set; }
        public decimal qty { get; set; }
        public string description { get; set; }
    }


    public class Products
    {
        public int count { get; set; }
        public Product[] items { get; set; }
    }

}
