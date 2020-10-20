using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleshipLib
{
    public class DataPacket<T> : DAbstract where T : DAbstract
    {
        public string type;
        public T data;
    }

    public class DataPacket : DAbstract
    {
        public string type;
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        private JObject data;

        public DataPacket<T> GetData<T>() where T : DAbstract
        {
            return new DataPacket<T>
            {
                type = this.type,
                data = this.data.ToObject<T>()
            };
        }
    }
}
