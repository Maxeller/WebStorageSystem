using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebStorageSystem.Models
{
    public class Select2AjaxResult
    {
        [JsonPropertyName(name: "results")]
        public List<Select2AjaxPartResult> Results { get; set; }

        public Select2AjaxResult()
        {
            Results = new List<Select2AjaxPartResult>();
        }
    }

    public class Select2AjaxPartResult
    {
        [JsonPropertyName(name: "id")]
        public int Id { get; set; }

        [JsonPropertyName(name: "text")]
        public string Text { get; set; }

        [JsonPropertyName(name: "selected")]
        public bool Selected { get; set; }

        [JsonPropertyName(name: "disabled")]
        public bool Disabled { get; set; }

        public Select2AjaxPartResult(int id, string text, bool selected = false, bool disabled = false)
        {
            Id = id;
            Text = text;
            Selected = selected;
            Disabled = disabled;
        }
    }
}
