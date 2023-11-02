namespace Evu.MainMenu{
    using Evu.Common.UI;
    using TMPro;
    using UnityEngine;

    public class JoinRoomUI : MonoBehaviour
    {

        [SerializeField] GameObject goRoot = null;
        [SerializeField] MenuAnim menuAnim = null;
        [SerializeField] TMP_Text textInfo = null;

        private void Awake()
        {
            goRoot.SetActive(false);
        }

        public void ShowUI()
        {
            goRoot.SetActive(true);
            menuAnim.StartAnim(MenuAnim.MenuSlideAnimTypes.RightToCenter);
        }

        public void HideUI(MenuAnim.MenuSlideAnimTypes slideAnimType)
        {
            if (!goRoot.activeInHierarchy)
                return;

            menuAnim.StartAnim(slideAnimType, 0, () => goRoot.SetActive(false));
        }

        public void UpdateInfoText(string info)
        {
            textInfo.text = info;
        }

    }

}