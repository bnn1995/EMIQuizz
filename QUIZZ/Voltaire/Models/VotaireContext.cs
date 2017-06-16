using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Voltaire.Models
{
    public class VoltaireContext : DbContext
    {
        public DbSet<Question> Questions { get; set; }
        public DbSet<Niveau> Niveaux { get; set; }
        public DbSet<Difficulte> Difficultes { get; set; }
        public DbSet<StatUser> StatUsers { get; set; }
        public DbSet<ScoreNiveau> ScoreNiveaus { get; set; }


    }
}