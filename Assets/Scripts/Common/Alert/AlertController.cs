namespace Evu.MainMenu
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(AlertUI))]
    public class AlertController : Singleton<AlertController>
    {
        [SerializeField]
        private AlertUI ui = null;

        private Action onFirstButtonClick = null;
        private Action onSecondButtonClick = null;

        public void ShowController(string header, string detail, string firstButtonText, Action onFirstButtonClick, string secondButtonText = null, Action onSecondButtonClick = null)
        {
            this.onFirstButtonClick = onFirstButtonClick;
            this.onSecondButtonClick = onSecondButtonClick;

            ui.ShowController();

            ui.UpdateInfo(header, detail);

            if (secondButtonText == null)
                ui.HideSecondButton();
            else
                ui.ShowSecondButton(secondButtonText);

            ui.UpdateFirstButtonText(firstButtonText);
        }

        public void HideWithFirstButton()
        {
            ui.HideController();

            Action action = onFirstButtonClick;

            ResetActions();

            action?.Invoke();
        }

        public void HideWithSecondButton()
        {
            ui.HideController();

            Action action = onSecondButtonClick;

            ResetActions();

            action?.Invoke();
        }

        private void ResetActions()
        {
            onFirstButtonClick = null;
            onSecondButtonClick = null;
        }

    }

}