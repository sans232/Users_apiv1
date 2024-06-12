using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebAPI_Users.Models
{
    [Table("TIPO_PERSONA")]
    public class TipoPersona
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdTipoPersona { get; set; }

        [MaxLength(50)]
        public required string Descripcion { get; set; }
        public bool? Estado { get; set; } = true;

        public DateTime? FechaCreacion { get; set; } = DateTime.Now;
    }

}
