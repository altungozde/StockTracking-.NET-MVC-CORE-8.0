#nullable disable

using System.ComponentModel;

namespace Business.Models
{
    public class ReportItemModel
    {
        #region Raporda verileri gösterebilmek için
        [DisplayName("Product Name")]
        public string ProductName{ get; set; }

        public string ProductDescription { get; set; }

        [DisplayName("Lot No")]
        public string LotNo{ get; set; }

        [DisplayName("Stock Amount")]
        public string StockAmount { get; set; }
        [DisplayName("Unit Price")]
        public string UnitPrice { get; set; }

        [DisplayName("Category")]
        public string CategoryName { get; set; }

        [DisplayName("Supplier Name")]
        public string SupplierName { get; set; }

        [DisplayName("Order")]
        public string Order { get; set; }

        [DisplayName("Order Date")]
        public string OrderDate { get; set; }

        #endregion

        #region Veriyi filtreleyebilmek için 
        //bir raporda filtreleme özelliklerinin hepsi nullable tanımlanmalı null yapmazsak ve  Left Outer Join kullanırsak null gelme ihtimali var
        public DateTime? DateValue { get; set; }
        public int? CategoryId { get; set; }
        public int? SupplierId { get; set; }
        #endregion


    }
}
