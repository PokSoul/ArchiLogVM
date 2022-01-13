using Archi.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace Archi.Api.Models
{
    //[Table("Client")]
    public class Customer : BaseModel
    {
        //[Key]
        //public int ID { get; set; }
        private string Lastname { get; set; }
        //[StringLength(30)]
        //[Column("Prenom")]
        public string Firstname { get; set; }
        public string Email { get; set; }
        //[Required]
        public string Phone { get; set; }
    }
}
