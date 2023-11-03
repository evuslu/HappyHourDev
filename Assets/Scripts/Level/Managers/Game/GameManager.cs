namespace Evu.Level
{
    using System;
    using System.Collections.Generic;
    using Evu.Common;
    using Evu.Common.UI;
    using Evu.Network;
    using UnityEngine;

    public class GameManager : Singleton<GameManager>
    {
        public static Action OnLevelLoad;

        
        [SerializeField] Transform[] playerSpawnPositions = null;
        [SerializeField] float spawnDistanceBeetwenCharacters = 2f;
        [SerializeField] Color[] playerColors = null;

        public enum States { None, LevelLoad, Game}
        public States State { get; private set; } = States.None;

        public bool IsLevelLoadCompleted { get; private set; } = false;
        public bool IsPaused { get; private set; } = false;
        public bool IsGameRunning => State == States.Game && !IsPaused;
        public bool IsInputActive => IsGameRunning;

        public static Action OnGamePaused;
        public static Action OnGameResumed;

        
        #region Mono Behaviour

        private void Start()
        {
            LoadingController.Instance.ShowController(MenuAnim.MenuSlideAnimTypes.None
                , () =>
                {
                    if (!PhotonManager.Instance.IsConnected)
                    {
                        LevelSceneManager.LoadSplash();
                        return;
                    }

                    ChangeState(States.LevelLoad);
                } );
        }

        #endregion

        #region Public Functions

        public Vector3 CharacterSpawnPosition(int playerIndex, int characterIndex)
        {
            int spawnIndex = playerIndex % playerSpawnPositions.Length;

            return playerSpawnPositions[spawnIndex].position + Vector3.forward * (spawnDistanceBeetwenCharacters * characterIndex);
        }

        public Color PlayerColor(int playerIndex)
        {
            int spawnIndex = playerIndex % playerSpawnPositions.Length;

            return playerColors[spawnIndex];
        }

        #endregion

        #region State Machine

        private bool isChangeStateLocked = false;
        private List<States> changeStates = new List<States>(4);

        public void ChangeState(States stateNew)
        {
            if (isChangeStateLocked)
            {
                // ChangeState called inside change state funcion(OnExit,OnEnter etc)
                // wait for other ChangeState function execution
                changeStates.Add(stateNew);
                return;
            }

            if (stateNew != State)
                HandleStateUpdate(stateNew);

            if (changeStates.Count > 0)
            {
                //we have a new ChangeState request during execution -> handle it
                States changeState = changeStates[0];
                changeStates.RemoveAt(0);
                ChangeState(changeState);
            }
        }

        private void HandleStateUpdate(States stateNew)
        {
            isChangeStateLocked = true;

            //None, LevelLoad, Game, PauseScreen, Fail, Success
            switch (stateNew)
            {
                case States.LevelLoad:
                    InitStateLevelLoad();
                    break;
                case States.Game:
                    InitStateGame();
                    break;
            }

            State = stateNew;

            bool isPausedPrev = IsPaused;
            IsPaused = State != States.Game;

            if (isPausedPrev != IsPaused)
            {
                if (IsPaused)
                    OnGamePaused?.Invoke();
                else
                    OnGameResumed?.Invoke();
            }

            isChangeStateLocked = false;
        }

        #endregion

        #region States

        #region State Level Load

        private void InitStateLevelLoad()
        {
            CameraManager.Instance.InitManager();

            LoadingController.Instance.HideController(MenuAnim.MenuSlideAnimTypes.CenterToLeft, () =>
            {
                ChangeState(States.Game);

                IsLevelLoadCompleted = true;
                OnLevelLoad?.Invoke();
            });//LoadingController.Instance.HideController
        }

        #endregion

        #region State Game

        private void InitStateGame()
        {
            
        }

        #endregion

        #endregion
    }

}
