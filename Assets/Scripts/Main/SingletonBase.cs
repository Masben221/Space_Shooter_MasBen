using UnityEngine;

namespace SpaceShooter
{
    [DisallowMultipleComponent]
    public abstract class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
    {
        [Header("Singleton")]
        [SerializeField] private bool m_IsDoNotDestroyOnLoad;
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("MonoSingleton: object of type already exists, instance will be destroyed = " + typeof(T).Name);

                Destroy(this);

                m_IsDoNotDestroyOnLoad = false;

                return;
            }
            Instance = this as T;
            if (m_IsDoNotDestroyOnLoad == true)
                DontDestroyOnLoad(gameObject);
        }
    }
}
