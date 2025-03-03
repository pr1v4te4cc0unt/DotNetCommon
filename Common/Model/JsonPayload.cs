using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class JsonPayload
    {
        public string Data { get; set; }

        public JsonPayload() { }

        public JsonPayload(string data) { 
            this.Data = data;
        }
    }
}
