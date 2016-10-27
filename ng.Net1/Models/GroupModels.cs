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
    public class GroupModels
    {

    }

    [Table("Groups")]
    public class Group
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public int type { get; set; }
        public bool deleted { get; set; }
        public string create_user { get; set; }
        public DateTime ? create_datetime { get; set; }
        public string edit_user { get; set; }
        public DateTime ? edit_datetime { get; set; }
    }
}