namespace Evu.MainMenu
{
    using Evu.Common.UI;
    using TMPro;
    using UnityEngine;

    public class AlertUI : UIBase
    {
        [Header("Base")]
        [SerializeField] GameObject goRoot = null;
        [SerializeField] MenuAnim menuAnim = null;

        [Header("Messages")]
        [SerializeField] TMP_Text textHeader = null;
        [SerializeField] TMP_Text textDetail = null;

        [Header("First Button")]
        [SerializeField] TMP_Text textFirstButton = null;

        [Header("Second Button")]
        [SerializeField] GameObject goSecondButtonContiner = null;
        [SerializeField] TMP_Text textSecondButton = null;

        private void Awake()
        {
            goRoot.SetActive(false);
        }

        public void ShowController()
        {
            goRoot.SetActive(true);
            menuAnim.StartAnim(MenuAnim.MenuScaleAnimTypes.ScaleUp);
        }

        public void HideController()
        {
            if (!goRoot.activeInHierarchy)
                return;

            menuAnim.StartAnim(MenuAnim.MenuScaleAnimTypes.ScaleDown);
        }

        public void UpdateInfo(string header, string detail)
        {
            textHeader.text = header;
            textDetail.text = detail;
        }

        public void UpdateFirstButtonText(string text)
        {
            textFirstButton.text = text;
        }

        public void ShowSecondButton(string text)
        {
            goSecondButtonContiner.gameObject.SetActive(false);
            textSecondButton.text = text;
        }

        public void HideSecondButton()
        {
            goSecondButtonContiner.gameObject.SetActive(false);
        }

        #region Button Events

        public void OnButtonFirstClick()
        {
            AlertController.Instance.HideWithFirstButton();
        }

        public void OnButtonSecondClick()
        {
            AlertController.Instance.HideWithSecondButton();
        }

        #endregion

    }

}