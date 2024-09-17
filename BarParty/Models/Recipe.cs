using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace BarParty.Models
{
    [Table("Recipe")]
    public class Recipe
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Glass { get; set; }
        public int Method { get; set; }
        public string Prep { get; set; }
        public string Note { get; set; }
        public string Photo { get; set; }
        public int Calif { get; set; }
    }
}
