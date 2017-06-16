using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Voltaire.Models
{
    public class StatUser
    {
        public int id { get; set; }
        public string email { get; set; }
        public int id_niveau { get; set; }
        public int id_difficulte { get; set; }
        public int id_regle { get; set; }
        public int score_regle { get; set; }
    }

}