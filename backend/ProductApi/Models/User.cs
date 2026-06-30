using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductApi.Models
{
    [Table("Users")] // ผูกกับตารางชื่อ Users
    public class Users
    {
        [Key]
        [Column("ID")] // แมตช์กับ ID ใน SQL
        public int UserId { get; set; }

        [Column("username")] // แมตช์กับ username ใน SQL
        public string Username { get; set; } = string.Empty;

        [Column("password")] // แมตช์กับ password ใน SQL
        public string Password { get; set; } = string.Empty;

        [Column("name")] // แมตช์กับ name ใน SQL
        public string Name { get; set; } = string.Empty;

        [Column("role")] // แมตช์กับ role ใน SQL
        public string Role { get; set; } = string.Empty;

        [Column("contact_info")] // แมตช์กับ contact_info ใน SQL
        public string ContactInfo { get; set; } = string.Empty;
    }
}