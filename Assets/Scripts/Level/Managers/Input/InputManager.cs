namespace Evu.Level
{
    using UnityEngine;

    public class InputManager : Singleton<InputManager>
    {
        public static System.Action OnTouchStart;
        public static System.Action OnTouchEnd;
        public static System.Action OnClick;

        [SerializeField] float clickDuration = 0.2f;
        private float clickTimeRem = -1f;

        #region Public Properties

        public bool IsTouchScreen { get; private set; } = false;
        public bool IsTouchDown { get; private set; } = false;
        public Vector3 TouchPos { get; private set; } = Vector3.zero;

        #endregion

        #region Mono Behaviour

        protected override void Awake()
        {
            base.Awake();

            IsTouchScreen = Application.platform == RuntimePlatform.Android
                    || Application.platform == RuntimePlatform.IPhonePlayer;
        }

        private void Update()
        {
            if(clickTimeRem > -0.00001f)
                clickTimeRem -= Time.deltaTime;

            if(Input.GetKeyDown(KeyCode.P))
                Debug.Break();

            if(!GameManager.Instance.IsInputActive)
                return;

            if (IsTouchScreen)
                CheckTouches();
            else
                CheckMouse();
        }

        #endregion


        #region Private Functions

        private void CheckMouse()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnInputStart(Input.mousePosition);
                return;
            }

            if (Input.GetMouseButton(0))
            {
                OnInputMove(Input.mousePosition);
                return;
            }

            if (Input.GetMouseButtonUp(0))
            {
                OnInputEnd(Input.mousePosition);
                return;
            }
        }

        private void CheckTouches()
        {
            if (Input.touchCount <= 0)
                return;

            switch (Input.touches[0].phase)
            {
                case TouchPhase.Began:
                    TouchPos = Input.GetTouch(0).position;
                    OnInputStart(TouchPos);
                    break;
                case TouchPhase.Moved:
                    OnInputMove(Input.GetTouch(0).position);
                    break;
                case TouchPhase.Canceled:
                case TouchPhase.Ended:
                    OnInputEnd(Input.GetTouch(0).position);
                    break;
            }
        }

#endregion

#region Input

        private void OnInputStart(Vector3 pos)
        {
            IsTouchDown = true;
            TouchPos = pos;

            OnTouchStart?.Invoke();

            clickTimeRem = clickDuration;
        }

        private void OnInputMove(Vector3 pos)
        {
            IsTouchDown = true;
            TouchPos = pos;
        }

        private void OnInputEnd(Vector3 pos)
        {
            IsTouchDown = false;

            OnTouchEnd?.Invoke();

            if (clickTimeRem > 0f)
                OnClick?.Invoke();

            clickTimeRem = -1f;
        }

        #endregion
    }

}



/*
        #region Public Functions

        public bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = GetPointerPosition();
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        #endregion
        */



/*
private Vector2 GetPointerPosition()
{
    if (IsTouchScreen)
    {
        if (Input.touchCount == 0)
            return Vector2.zero;

        return new Vector2(Input.touches[0].position.x, Input.touches[0].position.y);
    }

    return new Vector2(Input.mousePosition.x, Input.mousePosition.y);
}
*/