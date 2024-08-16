using System.Text.Json.Serialization;

namespace LECCIONCLIENTES.DTO
{
    public class ClientesDTO
    {
        [JsonPropertyName("idCliente")]
        public int Idc { get; set; }

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [JsonPropertyName("apellido")]
        public string? Apellido { get; set; }


    }
}
