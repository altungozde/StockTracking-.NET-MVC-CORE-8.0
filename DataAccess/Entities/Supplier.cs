#nullable disable

using Core.Records.Bases;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class Supplier : Record
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
        public string Address { get; set; }
        public int ContactNumber { get; set; }

        public List<Product> Products { get; set; }
    }
}

