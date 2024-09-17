using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarParty.Models
{
    [Table("TagXRec")]
    public class TagXRec
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public int TagID { get; set; }
        public int RecID { get; set; }
    }
}
