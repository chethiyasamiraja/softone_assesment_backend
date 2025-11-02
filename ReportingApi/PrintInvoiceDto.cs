using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReportingApi
{
    public class PrintInvoiceDto
    {
        public string barcode { get; set; }
        public string producT_NAME { get; set; }
        public decimal cosT_PRICE { get; set; }
        public decimal iteM_PRICE { get; set; }
        public int Itm_QTY { get; set; }
        public string Cust_Name { get; set; }
        public decimal Sub_Total { get; set; }
        public decimal Transaction_Id { get; set; }
        public decimal Discount { get; set; }
    }
}