using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SignatureWeb.Models
{
    [Keyless]
    public class UserUnitPosition
    {
        public  int UnitSeq { get; set; }   

        public int PositionSeq { get; set; }

        [Column("UserMainSeq")]
        public int UserMainId { get; set; }
    }
}
