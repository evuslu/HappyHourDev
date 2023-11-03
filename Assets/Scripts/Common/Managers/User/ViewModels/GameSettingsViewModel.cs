namespace Evu.Common{

    using Evu.Common.Model.GameConfig;
    using UnityEngine;

    public class GameSettingsViewModel
    {
        public int PlayerCharacterCount => model.playerCharacterCount;

        public int RoomPlayerLimit => model.roomPlayerLimit;

        private GameSettingsConfigModel model;
        public void InitViewModel(GameSettingsConfigModel model)
        {
            this.model = model;
        }
    }

}