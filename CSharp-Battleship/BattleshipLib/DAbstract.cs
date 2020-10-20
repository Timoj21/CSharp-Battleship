using Newtonsoft.Json;
using System;


namespace BattleshipLib
{
    public abstract class DAbstract
    {
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
