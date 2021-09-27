using System.Text.Json.Serialization;

namespace WebStorageSystem.Models.DataTables
{
    public class DataTableResponse<T>
    {
        // Because option "JsonSerializerOptions.PropertyNamingPolicy = null" JsonProperty has to be set to camelCase name

        [JsonPropertyName("draw")]
        public int Draw { get; set; }

        [JsonPropertyName("recordsTotal")]
        public int RecordsTotal { get; set; }

        [JsonPropertyName("recordsFiltered")]
        public int RecordsFiltered { get; set; }

        [JsonPropertyName("data")]
        public T[] Data { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }
    }
}
