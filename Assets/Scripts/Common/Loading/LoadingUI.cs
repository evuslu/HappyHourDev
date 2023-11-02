namespace Evu.Common
{

    using System.Collections;
    using System.Collections.Generic;
    using Evu.Common.UI;
    using UnityEngine;

    public class LoadingUI : UIBase
    {
        public MenuAnim MenuAnim => menuAnim;
        [SerializeField] MenuAnim menuAnim = null;

        [SerializeField] GameObject goMainCanvas = null;

        public bool IsMainCanvasActive => goMainCanvas.activeSelf;

        public void ShowMainCanvas() => goMainCanvas.SetActive(true);

        public void HideMainCanvas() => goMainCanvas.SetActive(false);
    }

}