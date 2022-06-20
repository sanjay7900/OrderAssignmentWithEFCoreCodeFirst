using Microsoft.EntityFrameworkCore;
using OrderAssignmentDll.AllTableModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAssignmentDll
{
    public class DbContextFile:DbContext
    {
        public DbSet<ItemMaster> itemMasters { get; set; }
        public DbSet<Customers> customers { get; set; }


       public DbContextFile()
       {

       }
        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            dbContextOptionsBuilder.UseSqlServer(@"Data Source=DESKTOP-AMR2CQS\MSSQLSERVER01;Initial Catalog=EfcoreCodeFirst;Integrated Security=True");
        }
    }
}
