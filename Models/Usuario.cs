using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.Text.Json.Serialization;

namespace RecetasApp.Models
{
    [Table("usuarios")]
    public class Usuario : BaseModel
    {
        [PrimaryKey("id", true)]
        public Guid Id { get; set; }

        [Column("githubid")]
        public string? GitHubId { get; set; }

        [Column("nombre")]
        public string? Nombre { get; set; }

        [Column("email")]
        public string? Email { get; set; }
    }
}
