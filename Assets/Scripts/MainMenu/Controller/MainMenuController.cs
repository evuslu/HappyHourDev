using System.Collections;

namespace Evu.MainMenu
{
    using UnityEngine;
    using Common;
    using Evu.MainMenu.MainMenuStateMachine;
    using StateIds = MainMenuStateMachine.StateBase.StateIds;
    using UnityEngine.UI;
    using Evu.Common.UI;

    [RequireComponent(typeof(MainMenuUI))]
    public class MainMenuController : Singleton<MainMenuController>
    {

        [SerializeField] MainMenuStateMachine.StateInfo stateInfo = new StateInfo();
        private StateMachine stateMachine = null;

        [SerializeField] private MainMenuUI ui = null;

        public StateIds CurrentState => stateMachine?.State?.StateId() ?? StateIds.None;

        #region Mono - Init

        protected override void Awake()
        {
            base.Awake();

            stateMachine = gameObject.AddComponent<StateMachine>();
            stateMachine.InitStateMachine(stateInfo);
        }
        
        private void Start()
        {
            ui.ShowUI();

#if UNITY_EDITOR
            // in game, this scene could not load directly
            // for editor show loading controller
            LoadingController.Instance.ShowController(MenuAnim.MenuSlideAnimTypes.None, () => HideLoading() );
#else
            HideLoading();
#endif
        }

        private void HideLoading()
        {
            LoadingController.Instance?.HideController(MenuAnim.MenuSlideAnimTypes.None, () =>
            {
                stateMachine.ChangeState(StateIds.Welcome);
            });
        }

        #endregion

        public void ChangeState(StateIds stateNew) => stateMachine.ChangeState(stateNew);
    }

}