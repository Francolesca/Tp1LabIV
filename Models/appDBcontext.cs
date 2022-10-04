using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Tp1WebApp.Models
{
    public class appDBcontext : DbContext
    {
        public appDBcontext(DbContextOptions<appDBcontext> options)
         : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //   optionsBuilder.UseSqlite(connectionString: "Filename=PruebasSQLite.db",
        //        sqliteOptionsAction: op =>
        //        {
        //            op.MigrationsAssembly(
        //                Assembly.GetExecutingAssembly().FullName
        //                );
        //        });
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Data Source=.\sqlexpress;Initial Catalog=Tp1WebApp;Integrated Security=True; Trusted_Connection=True;MultipleActiveResultSets=True");
        //}

        public DbSet<Libro> libros { get; set; }
        public DbSet<Genero> generos { get; set; }
        public DbSet<Autor> autores { get; set; }
    }

}
