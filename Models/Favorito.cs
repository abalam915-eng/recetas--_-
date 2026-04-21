using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;

namespace RecetasApp.Models
{
    [Table("favoritos")]
    public class Favorito : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Column("usuarioid")]
        public Guid UsuarioId { get; set; }

        [Column("recetaid")]
        public Guid RecetaId { get; set; }
    }
}
