#nullable disable

using Core.Records.Bases;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class User : Record
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        public string Password { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
