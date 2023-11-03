using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Serialization;

namespace Evu.Common.Model.GameConfig
{
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    public class GameSettingsConfigModel
    {
        [JsonProperty("rpl")]
        public int roomPlayerLimit = 2;

        [JsonProperty("pcc")]
        public int playerCharacterCount = 3;
    }
}