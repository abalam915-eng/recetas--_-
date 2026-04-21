using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;

namespace RecetasApp.Models
{
    [Table("recetas")]
    public class Receta : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Column("usuarioid")]
        public Guid UsuarioId { get; set; }

        [Column("titulo")]
        public string? Titulo { get; set; }

        [Column("ingredientes")]
        public string? Ingredientes { get; set; }

        [Column("pasos")]
        public string? Pasos { get; set; }

        [Column("imagen")]
        public string? Imagen { get; set; }

        [Column("autororiginal")]
        public string? AutorOriginal { get; set; }

        [Column("estatus")]
        public string Estatus { get; set; } = "Borrador";

        [Column("fechacreacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [Column("fechapublicacion")]
        public DateTime? FechaPublicacion { get; set; }
    }
}
