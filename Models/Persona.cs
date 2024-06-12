using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebAPI_Users.Models
{
    [Table("PERSONA")]
    public class Persona
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPersona { get; set; }

        [StringLength(50)]
        public string? Nombre { get; set; }

        [StringLength(50)]
        public string? Apellido { get; set; }

        [StringLength(50)]
        public string? Correo { get; set; }

        [StringLength(500)]
        public string? Clave { get; set; }

        [StringLength(50)]
        public string? Codigo { get; set; }

        public int? IdTipoPersona { get; set; }

        public bool? Estado { get; set; } = true;

        public DateTime? FechaCreacion { get; set; } = DateTime.Now;

        [ForeignKey("IdTipoPersona")]
        public virtual TipoPersona? TipoPersona { get; set; }
    }
}
