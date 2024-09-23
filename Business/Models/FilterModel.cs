#nullable disable

using System.ComponentModel;

namespace Business.Models
{
    public class FilterModel
    {
        
        [DisplayName("Product Name")]
        public string ProductName { get; set; }

        [DisplayName("Date")]

        public DateTime? DateBegin { get; set; }

        public DateTime? DateEnd { get; set; }

        [DisplayName("Categories")]
        public int? CategoryId { get; set; }

        [DisplayName("Suppliers")]
        public int? SupplierId { get; set; }

    }
}
