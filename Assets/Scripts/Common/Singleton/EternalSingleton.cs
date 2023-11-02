
namespace Evu.Common
{

    using UnityEngine;

    public class EternalSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; } = null;

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            transform.parent = null;

            Instance = this as T;

            DontDestroyOnLoad(gameObject);
        }

#if UNITY_EDITOR

        public static void SetEditorInstance()
        {
            Instance = FindObjectOfType<T>();
        }

#endif

    }
}
