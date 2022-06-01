using ElasticEmailAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInput.Model
{
    public class UserInputData
    {
        public string Sender { get; set; } = string.Empty;
        public string[] Recipients { get; set; } = new string[0];
        public string Content { get; set; } = string.Empty;
        public EmailContentType ContentType { get; set; }
    }
}
