using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Migrations.History;
using ng.Net1.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplication6
{
    public class MySqlHistoryContext : HistoryContext
    {
        public MySqlHistoryContext(
          DbConnection existingConnection,
          string defaultSchema)
        : base(existingConnection, defaultSchema)
        {
        }
    }
}