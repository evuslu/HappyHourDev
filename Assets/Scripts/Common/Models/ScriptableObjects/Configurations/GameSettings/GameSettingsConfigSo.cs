using Evu.Common.Model.GameConfig;

namespace Evu.Common.Model
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "GameSettings", menuName = "Evu/Configurations/GameSettings", order = 0)]
    public class GameSettingsConfigSo : ScriptableObject
    {
        public GameSettingsConfigModel config = null;
    }

}