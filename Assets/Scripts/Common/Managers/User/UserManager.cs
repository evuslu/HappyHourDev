namespace Evu.Common{

    using UnityEngine;
    using Evu.Common.Model;

    public class UserManager : EternalSingleton<UserManager>
    {
        public static GameSettingsViewModel GameSettingsVM => Instance?.gameSettingsVM;
        [SerializeField] GameSettingsConfigSo gameSettingsConfigSo = null;
        private GameSettingsViewModel gameSettingsVM = null;

        protected override void Awake()
        {
            base.Awake();

            if (Instance != this)
                return;

            gameSettingsVM = new GameSettingsViewModel();
            gameSettingsVM.InitViewModel(gameSettingsConfigSo.config);
        }
        
    }

}