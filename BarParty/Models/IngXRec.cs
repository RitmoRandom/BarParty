using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace BarParty.Models
{
    [Table("IngXRec")]
    public class IngXRec
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public int RecID { get; set; }
        public int IngID { get; set; }
        public double Cant { get; set; }
        public string Note { get; set; }
        
}
}
