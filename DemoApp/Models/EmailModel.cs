using Newtonsoft.Json;

namespace EmailService.Models
{
    public class EmailModel
    {
        [JsonProperty(Required = Required.Always)]
        public string ToEmails { get; set; }
       
        [JsonProperty]
        public string Comments { get; set; }
  }
}