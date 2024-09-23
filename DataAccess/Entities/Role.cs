#nullable disable

using Core.Records.Bases;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class Role : Record
    {
        [Required]
        [StringLength(30)]
        public string RoleName { get; set; }


        public List<User> Users { get; set; }


    }
}

