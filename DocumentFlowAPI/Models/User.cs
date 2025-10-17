using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentFlowAPI.Models
{
    public class User : EntityBase
    {
        public string FullName { get; set; }
        [EmailAddress]
        [MaxLength(63)]
        public required string Email { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
        public int DepartmentId { get; set; }
        public bool IsActive { get; set; }
    }
}