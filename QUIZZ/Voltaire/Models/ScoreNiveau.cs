using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Voltaire.Models
{
    public class ScoreNiveau
    {
        public int id { get; set; }
        public string usr_email { get; set; }
        public int id_niveau { get; set; }
        public int id_diff { get; set; }
        public int score_niveau{ get; set; }
    }
}