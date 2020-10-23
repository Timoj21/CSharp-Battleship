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
        public bool isPlayer1;
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
        public bool joinedGame;
    }

    public class GameStateChangePacket : DAbstract
    {
        public string state;
    }

    public class CellPackage : DAbstract
    {
        public bool isPlayer1;
        public int cell;
    }

    public class HitMissResponse : DAbstract
    {
        public bool hit;
        public string cell;
    }
}
