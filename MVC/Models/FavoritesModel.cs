#nullable disable

using System.ComponentModel;

namespace MVC.Models

{
    public class FavoritesModel
    {
        public int SupplierId { get; set; }
        public int UserId { get; set; }
        [DisplayName("Supplier Name")]
        public string SupplierName { get; set; }

        public FavoritesModel(int supplierId, int userId, string supplierName)
        {
            SupplierId = supplierId;
            UserId = userId;
            SupplierName = supplierName;
        }
    }
}
