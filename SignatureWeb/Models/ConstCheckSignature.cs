using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignatureWeb.Shared.Models
{
    public class ConstCheckSignature
    {
        [Key]
        public int Seq { get; set; }
        [Required]
        public string? SignatureImgeBase64 { get; set; }


        [Required]
        public string Token { get; set; }

        [Required]
        public string SignatureVal { get; set; }
        [Required]
        public int ConstCheckSeq { get; set; }

        [Required]
        public int EngSeq { get; set; }


        public int SignatureRole { get; set; }
        public DateTime? CreateTime{get;set;}

        public DateTime? ModifyTime{get;set;}    


    }
}
