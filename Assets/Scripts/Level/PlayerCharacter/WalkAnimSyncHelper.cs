namespace Evu.Level
{
    using UnityEngine;

    public class WalkAnimSyncHelper : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private Vector3 lastPos;
        private float lastSpeed;
        private int animHashWalkSpeed;

        private void Awake()
        {
            lastPos = transform.position;
            lastSpeed = 0f;

            animHashWalkSpeed = Animator.StringToHash("walkSpeed");
        }

        private void Update()
        {
            float speed = (lastPos - transform.position).magnitude / Time.deltaTime;

            if (speed < 0.01f)
            {
                animator.SetFloat(animHashWalkSpeed, 0f);
            }
            else if(Mathf.Abs(speed - lastSpeed) > 0.001f)
            {
                speed = Mathf.Lerp(lastSpeed, speed, 10f * Time.deltaTime);
                animator.SetFloat(animHashWalkSpeed, speed);
            }

            lastSpeed = speed;
            lastPos = transform.position;
        }

    }

}