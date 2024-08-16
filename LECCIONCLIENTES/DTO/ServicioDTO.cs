using System.Text.Json.Serialization;

namespace LECCIONCLIENTES.DTO
{
    public class ServicioDTO
    {
        [JsonPropertyName("idServicio")]
        public int Ids { get; set; }

        [JsonPropertyName("Descripcion")]
        public string? Descripcion { get; set; }


    }
}
