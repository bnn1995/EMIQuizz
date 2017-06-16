using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Voltaire.Models
{
    public class Niveau
    {
        public int id { get; set; }
        public string titre_niv { get; set; }
        public string description_niv { get; set; }
        public Difficulte Difficulte { get; set; }
        public int DifficulteId { get; set; }

        public ICollection<Question> Questions { get; set; }


    }
}