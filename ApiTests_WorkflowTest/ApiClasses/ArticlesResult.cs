using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

#pragma warning disable CA2227 // Collection properties should be read only

namespace ApiClasses
{
    public class ArticlesResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        [JsonPropertyName("result")]
        public List<Article> Articles { get; set; }

        public List<object> Errors { get; set; }

        public int Status { get; set; }
    }
}
