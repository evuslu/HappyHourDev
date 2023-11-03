namespace Evu.Level
{
    using UnityEngine;

    public class CameraManager : Singleton<CameraManager>
    {
        private Camera cam = null;
        private Vector3 lastTouchPos = Vector3.zero;

        [SerializeField]
        private float moveSensivity = 20f;

        private Vector3 moveForward;
        private Vector3 moveRight;
        public void InitManager()
        {
            cam = Camera.main;
            InputManager.OnTouchStart += OnTouchStart;

            Transform tr = cam.transform;
            moveForward = tr.forward;
            moveForward.y = 0f;
            moveForward = moveForward.normalized;

            moveRight = tr.right;
            moveRight.y = 0f;
            moveRight = moveRight.normalized;

            moveSensivity /= Mathf.Min(Screen.width, Screen.height);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            InputManager.OnTouchStart -= OnTouchStart;
        }
        
        private void OnTouchStart()
        {
            lastTouchPos = InputManager.Instance.TouchPos;
        }

        private void Update()
        {
            if (!GameManager.Instance.IsInputActive || !InputManager.Instance.IsTouchDown)
                return;

            Vector3 delta = InputManager.Instance.TouchPos - lastTouchPos;

            Debug.Log(delta);

            

            cam.transform.position += (moveForward * delta.y + moveRight * delta.x) * moveSensivity;

            lastTouchPos = InputManager.Instance.TouchPos;

        }


    }

}
