
using Microsoft.EntityFrameworkCore;
using SignatureWeb.Models;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignatureWeb.Shared.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
        public DbSet<ConstCheckSignature> constCheckSignatures { get; set; }


    }
}
