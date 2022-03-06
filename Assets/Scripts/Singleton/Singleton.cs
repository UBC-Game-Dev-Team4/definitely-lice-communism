using UnityEngine;

namespace Singleton
{
    /// <summary>
    /// Inherit from this base class to create a singleton.
    /// e.g. public class MyClassName : Singleton<MyClassName> {}
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        // Check to see if we're about to be destroyed.
        protected static int m_ShuttingDown = 0;
        protected static object m_Lock = new object();
        protected static T m_Instance;
        protected static bool m_IsInitialized = false;

        public static T Instance
        {
            get
            {
                if (m_ShuttingDown > 0)
                {
                    Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                                     "' already destroyed. Returning null.");
                    return null;
                }

                // Don't think we need the lock, but I pulled this from wiki and will leave it.
                lock (m_Lock)
                {
                    if (m_Instance == null)
                    {
                        // Search for existing instance.
                        m_Instance = (T)FindObjectOfType(typeof(T));

                        // Create new instance if one doesn't already exist.
                        if (m_Instance == null)
                        {
                            // Create a new GameObject to attach the singleton to.
                            var singletonObject = new GameObject();
                            m_Instance = singletonObject.AddComponent<T>();
                            singletonObject.name = typeof(T).ToString() + " (Singleton)";

                            // Make instance persistent.
                            DontDestroyOnLoad(singletonObject);
                        }
                        else if (!m_IsInitialized)
                        {
                            m_Instance.InitializeFields();
                        }
                    }

                    return m_Instance;
                }
            }
        }

        protected virtual void Awake()
        {
            // Destroy if there is already an existing instance
            if (m_Instance != null && m_Instance != this)
            {
                Debug.Log(typeof(T).ToString() + " singleton already exists as: " + m_Instance.gameObject + ".\n    Destroying this game object: " + gameObject);

                // We're not actually shutting down, so decrement before incrementing in OnDestroy().
                m_ShuttingDown--;
                Destroy(gameObject);
            }
            else if (m_Instance == null)
            {
                m_Instance = (T)this;
                //m_ShuttingDown = 0; //Reset.
                DontDestroyOnLoad(gameObject);
            }
        }

        private protected virtual void InitializeFields()
        {
            m_IsInitialized = true;
        }
        private void OnApplicationQuit()
        {
            m_ShuttingDown++;
        }

        private void OnDestroy()
        {
            m_ShuttingDown++;
        }
    }
}