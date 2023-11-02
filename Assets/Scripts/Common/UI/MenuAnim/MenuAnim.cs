namespace Evu.Common.UI
{
    using UnityEngine;
    using DG.Tweening;
    using System.Collections;

    public class MenuAnim : MonoBehaviour
    {

        #region UI

        public const float MENU_ANIM_DURATION = 0.2f;

        #endregion

        #region Public Vars

        public enum MenuSlideAnimTypes
        {
            None
            , CenterToLeft, RightToCenter, BottomToCenter, CenterToTop, CenterToRight, LeftToCenter
            , TopToCenter, CenterToBottom
        };

        public enum MenuScaleAnimTypes
        {
            None
            , ScaleUp, ScaleDown
        }

        #endregion

        #region Private Vars

        bool isGameObjectActiveOnAnimEnd = false;

        RectTransform rectTr = null;

        private Vector3 vectorFrom;
        private Vector3 vectorTo;

        private TweenCallback onComplete;

        private float defaultY;
        private float defaultX;

        MenuSlideAnimTypes lastMenuSlideAnim;
        MenuScaleAnimTypes lastMenuScaleAnim;

        float lastAnimTimeTotal;

        #endregion

        private void Awake()
        {
            defaultX = transform.position.x;
            defaultY = transform.position.y;
        }

        #region Public Functions

        #region Slide Anim Functions

        public void StartLastSlideAnimReverse(float delay = 0f, TweenCallback onComplete = null)
        {
            StartAnim(GetReverseSlideAnimType(lastMenuSlideAnim), lastAnimTimeTotal, delay, onComplete);
        }
        public void StartAnim(MenuSlideAnimTypes menuAnimType, float delay = 0f, TweenCallback onComplete = null)
        {
            StartAnim(menuAnimType, MENU_ANIM_DURATION, delay, onComplete);
        }
        public void StartAnim(MenuSlideAnimTypes menuAnimType, float animTimeTotal, float delay = 0f, TweenCallback onComplete = null)
        {
            this.onComplete = onComplete;

            lastMenuSlideAnim = menuAnimType;
            lastAnimTimeTotal = animTimeTotal;

            gameObject.SetActive(true);

            if (rectTr == null)
                rectTr = GetComponent<RectTransform>();

            vectorFrom = Vector3.zero;
            vectorTo = Vector3.zero;

            bool moveX = false;
            bool moveY = false;

            switch (menuAnimType)
            {
                case MenuSlideAnimTypes.None:
                    delay = 0f;
                    animTimeTotal = 0f;

                    isGameObjectActiveOnAnimEnd = true;

                    moveX = true;
                    moveY = true;
                    break;

                case MenuSlideAnimTypes.BottomToCenter:
                    vectorFrom.y = -Screen.height * 1.1f;
                    vectorTo.y = 0f;

                    isGameObjectActiveOnAnimEnd = true;

                    moveY = true;
                    break;
                case MenuSlideAnimTypes.TopToCenter:
                    vectorFrom.y = Screen.height * 1.1f;
                    vectorTo.y = 0f;

                    isGameObjectActiveOnAnimEnd = true;

                    moveY = true;
                    break;
                case MenuSlideAnimTypes.CenterToLeft:
                    vectorFrom.x = 0f;
                    vectorTo.x = -Screen.width * 1.1f;

                    isGameObjectActiveOnAnimEnd = false;

                    moveX = true;
                    break;
                case MenuSlideAnimTypes.CenterToTop:
                    vectorFrom.y = 0f;
                    vectorTo.y = Screen.height * 1.1f;

                    isGameObjectActiveOnAnimEnd = false;

                    moveY = true;
                    break;
                case MenuSlideAnimTypes.CenterToBottom:
                    vectorFrom.y = 0f;
                    vectorTo.y = -Screen.height * 1.1f;

                    isGameObjectActiveOnAnimEnd = false;

                    moveY = true;
                    break;
                case MenuSlideAnimTypes.RightToCenter:
                    vectorFrom.x = Screen.width * 1.1f;
                    vectorTo.x = 0f;

                    isGameObjectActiveOnAnimEnd = true;

                    moveX = true;
                    break;
                case MenuSlideAnimTypes.CenterToRight:
                    vectorFrom.x = 0f;
                    vectorTo.x = Screen.width * 1.1f;

                    isGameObjectActiveOnAnimEnd = false;

                    moveX = true;
                    break;
                case MenuSlideAnimTypes.LeftToCenter:
                    vectorFrom.x = -Screen.width * 1.1f;
                    vectorTo.x = 0f;

                    isGameObjectActiveOnAnimEnd = true;

                    moveX = true;
                    break;

                default:
                    Debug.Log("MenuAnim:StartAnim : mising anim type " + menuAnimType.ToString());
                    break;
            }//switch (menuAnimType) 

            float multX = (rectTr.anchorMin.x + rectTr.anchorMax.x) / 2.0f;
            float multY = (rectTr.anchorMin.y + rectTr.anchorMax.y) / 2.0f;

            if (moveX)
            {
                vectorFrom.x += Screen.width * multX;
                vectorTo.x += Screen.width * multX;
            }
            else
            {
                vectorFrom.x = defaultX;
                vectorTo.x = defaultX;
            }


            if (moveY)
            {
                vectorFrom.y += Screen.height * multY;
                vectorTo.y += Screen.height * multY;
            }
            else
            {
                vectorFrom.y = defaultY;
                vectorTo.y = defaultY;
            }




            StartCoroutine(StartSlideAnim(animTimeTotal, delay));

            /*
            animX = animStartX;
            animY = animStartY;
            */
            /*
            SetPos (animX, animY);
            */
        }
        private IEnumerator StartSlideAnim(float animTimeTotal, float delay)
        {

            rectTr.DOMove(vectorFrom, 0f).From(vectorFrom);

            yield return new WaitForSeconds(delay);

            rectTr.DOMove(vectorTo, animTimeTotal).From(vectorFrom)/*.SetDelay(delay)*/.onComplete = OnSlideAnimComplete;
        }
        public void OnSlideAnimComplete()
        {
            gameObject.SetActive(isGameObjectActiveOnAnimEnd);
            onComplete?.Invoke();
        }
        public static MenuSlideAnimTypes GetReverseSlideAnimType(MenuSlideAnimTypes animType)
        {
            switch (animType)
            {
                case MenuSlideAnimTypes.None:
                    return MenuSlideAnimTypes.None;

                case MenuSlideAnimTypes.CenterToBottom:
                    return MenuSlideAnimTypes.BottomToCenter;
                case MenuSlideAnimTypes.BottomToCenter:
                    return MenuSlideAnimTypes.CenterToBottom;

                case MenuSlideAnimTypes.CenterToLeft:
                    return MenuSlideAnimTypes.LeftToCenter;
                case MenuSlideAnimTypes.LeftToCenter:
                    return MenuSlideAnimTypes.CenterToLeft;

                case MenuSlideAnimTypes.CenterToTop:
                    return MenuSlideAnimTypes.TopToCenter;
                case MenuSlideAnimTypes.TopToCenter:
                    return MenuSlideAnimTypes.CenterToTop;


                case MenuSlideAnimTypes.CenterToRight:
                    return MenuSlideAnimTypes.RightToCenter;
                case MenuSlideAnimTypes.RightToCenter:
                    return MenuSlideAnimTypes.CenterToRight;

                default:
                    Debug.LogError("MenuAnim : GetReverseAnimType => unhanled MenuAnimTypes : " + animType);
                    return MenuSlideAnimTypes.RightToCenter;
            }
        }

        #endregion

        #region Scale Anim Functions

        public void StartLastScaleAnimReverse(float delay = 0f, TweenCallback onComplete = null)
        {
            StartAnim(GetReverseScaleAnimType(lastMenuScaleAnim), lastAnimTimeTotal, delay, onComplete);
        }

        public void StartAnim(MenuScaleAnimTypes menuAnimType, float delay = 0f, TweenCallback onComplete = null)
        {
            StartAnim(menuAnimType, MENU_ANIM_DURATION, delay, onComplete);
        }

        public void StartAnim(MenuScaleAnimTypes menuAnimType, float animTimeTotal, float delay = 0f, TweenCallback onComplete = null)
        {
            this.onComplete = onComplete;

            lastMenuScaleAnim = menuAnimType;
            lastAnimTimeTotal = animTimeTotal;

            gameObject.SetActive(true);

            if (rectTr == null)
                rectTr = GetComponent<RectTransform>();

            vectorFrom = Vector3.zero;
            vectorTo = Vector3.zero;

            switch (menuAnimType)
            {
                case MenuScaleAnimTypes.ScaleUp:
                    vectorFrom = Vector3.one * 0.01f;
                    vectorTo = Vector3.one;
                    break;
                case MenuScaleAnimTypes.ScaleDown:
                    vectorFrom = Vector3.one;
                    vectorTo = Vector3.one * 0.01f;
                    break;
                default:
                    Debug.Log("MenuAnim:StartAnim : mising anim type " + menuAnimType.ToString());
                    break;
            }

            StartCoroutine(StartScaleAnim(animTimeTotal, delay));
        }
        private IEnumerator StartScaleAnim(float animTimeTotal, float delay)
        {
            rectTr.DOScale(vectorFrom, 0f).From(vectorFrom);

            yield return new WaitForSeconds(delay);

            rectTr.DOScale(vectorTo, animTimeTotal).From(vectorFrom).OnComplete(() => this.onComplete?.Invoke());
        }
        public static MenuScaleAnimTypes GetReverseScaleAnimType(MenuScaleAnimTypes animType)
        {
            switch (animType)
            {
                case MenuScaleAnimTypes.ScaleUp:
                    return MenuScaleAnimTypes.ScaleDown;
                case MenuScaleAnimTypes.ScaleDown:
                    return MenuScaleAnimTypes.ScaleUp;
                default:
                    Debug.LogError("MenuAnim : GetReverseScaleAnimType => unhanled MenuScaleAnimTypes : " + animType);
                    return MenuScaleAnimTypes.ScaleDown;
            }
        }

        #endregion

        /*
        public void SetCenterPos()
        {
            float multX = (rectTr.anchorMin.x + rectTr.anchorMax.x) / 2.0f;
            float multY = (rectTr.anchorMin.y + rectTr.anchorMax.y) / 2.0f;

            SetPos(Screen.width * 0.5f * multX, Screen.height * 0.5f * multY);
        }
        */
        #endregion
        

        //#region Private Functions

        //void SetPos(float fX, float fY)
        //{
        //    if(rectTr == null)
        //        rectTr = GetComponent<RectTransform> ();

        //    rectTr.position = new Vector3 (fX - (isScreenSpaceCamera ? Screen.width / 2 : 0)
        //        , fY - (isScreenSpaceCamera ? Screen.height / 2 : 0)
        //        , 0f);
        //}

        //#endregion
    }


}