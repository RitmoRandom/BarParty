using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace BarParty.Models
{
    [Table("Ingredients")]
    public class Ingredients
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int Unit { get; set; }
    }
}
