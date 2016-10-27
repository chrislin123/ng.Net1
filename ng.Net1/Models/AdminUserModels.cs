using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ng.Net1.Models
{
    public class AdminUser
    {
        public AdminUser()
        {
            Roles = new HashSet<Role>();
        }

        [Key]
        public int id { get; set; }
        [Required]
        public string account { get; set; }
        public string password { get; set; }
        
        [Required]
        [StringLength(50)]
        [MinLength(2, ErrorMessage = "長度不可小於 2")]
        [MaxLength(50, ErrorMessage = "長度不可超過 50")]
        public string name { get; set; }

        public string email { get; set; }
        public bool deleted { get; set; }
        public string create_user { get; set; }
        public DateTime? create_datetime { get; set; }
        public string edit_user { get; set; }
        public DateTime? edit_datetime { get; set; }

        public ICollection<Role> Roles { get; set; }

    }

    public class Role
    {
        public Role()
        {
            AdminUsers = new HashSet<AdminUser>();
            Menus = new HashSet<Menu>();
        }

        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public bool IsEnable { get; set; }

        public ICollection<AdminUser> AdminUsers { get; set; }
        public ICollection<Menu> Menus { get; set; }
    }

    public class Menu
    {
        public Menu()
        {
            Roles = new HashSet<Role>();
            isVisible = true;
            icon = "fa fa-file";
        }

        [Key]
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        public string path { get; set; }
        public string icon { get; set; }
        public bool isVisible { get; set; }
        public int ? ParentId { get; set; }
        public int ? orderSerial { get; set; }

        public ICollection<Role> Roles { get; set; }
    }
}