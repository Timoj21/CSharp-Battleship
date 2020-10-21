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

    public class HostGamePacket : DAbstract
    {
        public string name;
    }

    public class HostGameResponse : DAbstract
    {
        public bool inGame;
    }

    public class JoinGamePacket : DAbstract
    {
        public string name;
    }

    public class JoinGameResponse : DAbstract
    {
        public bool inGame;
    }

    public class ChooseGridPackage : DAbstract
    {
        public string name;
        public int game;
        public Dictionary<string, bool> grid;
    }

    public class ReadyUpPacket : DAbstract
    {
        public bool isPlayer1;
        public string[] boatPositions;
    }

    public class ReadyUpResponse : DAbstract
    {
        public bool ready;
    }
}
