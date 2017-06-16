using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Voltaire.Models
{
    public class Question
    {
        public int id { get; set; }
        public string enonce { get; set; }
        public string correction { get; set; }
        public int id_regle { get; set; }
        public string regle { get; set; }
        public string explication { get; set; }
        public string phrasecorrecte { get; set; }

        public Niveau Niveau { get; set; }
        public int NiveauId { get; set; }

    }
}