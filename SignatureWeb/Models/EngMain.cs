using System.ComponentModel.DataAnnotations;

namespace SignatureWeb.Shared.Models
{
    public class EngMain
    {
        [Key]
        public int Seq { get; set; }

        public  string? EngName { get; set; }    

        public string? EngNo { get; set;}

        public byte? SupervisorExecType  { get; set; }
    }
}
