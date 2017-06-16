using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Voltaire.Models
{
    public class Difficulte
    {
        public int id { get; set; }
        public string module { get; set; }
        public string description_diff { get; set; }


        public ICollection<Niveau> Niveaux { get; set; }


    }
}
