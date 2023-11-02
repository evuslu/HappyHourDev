using DG.Tweening;

namespace Evu.Common
{
    using Evu.Common.UI;
    using UnityEngine;

    [RequireComponent(typeof(LoadingUI))]
    public class LoadingController : EternalSingleton<LoadingController>
    {
        [SerializeField] LoadingUI ui = null;

        private static System.Action OnShown = null;
        private static System.Action OnHide = null;
        
        #region Public Functions

        public void ShowController(MenuAnim.MenuSlideAnimTypes menuAnimType = MenuAnim.MenuSlideAnimTypes.None, System.Action onShown = null)
        {
            OnShown += onShown;

            ui.ShowMainCanvas();

            //menuAnimType = MenuAnim.MenuSlideAnimTypes.None;
            
            ui.MenuAnim.StartAnim(menuAnimType, 0f, () => OnShowAnimComplete());
        }
        
        public void HideController(MenuAnim.MenuSlideAnimTypes menuAnimType = MenuAnim.MenuSlideAnimTypes.CenterToLeft, System.Action onHide = null)
        {
            OnHide += onHide;

            if (!ui.IsMainCanvasActive)
            {
                OnHideAnimComplete();
                return;
            }

            //menuAnimType = MenuAnim.MenuSlideAnimTypes.None;
            
            ui.MenuAnim.StartAnim(menuAnimType, 0f, () => OnHideAnimComplete());
        }

        #endregion

        #region Private Functions

        private void OnShowAnimComplete()
        {
            // delay : wait for any animation or action !!!

            DOVirtual.DelayedCall(0.001f, () =>
            {
                OnShown?.Invoke();
                OnShown = null;
            });
        }
        
        private void OnHideAnimComplete()
        {
            // delay : wait for any animation or action !!!
            DOVirtual.DelayedCall(0.001f, () =>
            {
                OnHide?.Invoke();
                OnHide = null;
                ui.HideMainCanvas();
            });
        }

        #endregion

        #region Mono Behaviour

        protected override void Awake()
        {
            base.Awake();

            if (Instance != this)
                return;

            ui.HideMainCanvas();
        }

        #endregion
    }

}