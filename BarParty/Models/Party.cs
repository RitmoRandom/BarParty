using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarParty.Models
{
    [Table("Party")]
    public class Party
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime FechaEvento { get; set; }
        public int people { get; set; }
        public int drinks { get; set; }
    }
}
