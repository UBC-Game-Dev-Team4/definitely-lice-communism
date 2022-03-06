using UnityEngine;

namespace Singleton
{
    /// <summary>
    /// Inherit from this base class to create a singleton that can be destroyed.
    /// e.g. public class MyClassName : DestroySingleton<MyClassName> {}
    /// </summary>
    /// <remarks>
    /// If this singleton cannot be found, Instance will return null.
    /// </remarks>
    public class DestroySingleton<T> : MonoBehaviour where T : DestroySingleton<T>
    {
        protected static T instance;

        public static T Instance
        {
            get
            {
                if (instance != null) return instance;
                instance = FindObjectOfType(typeof(T)) as T;
                if (instance == null)
                {
                    Debug.LogWarning("oh well. Prepare for crash.");
                }

                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance != null && instance != this)
            {
                Debug.LogWarning("There's two instances! You are not a clown, you are the entire circus.");
                if (instance.GetType() == this.GetType())
                {
                    Debug.LogWarning("Overwriting instance and praying");
                    instance = this as T;
                }
            }
        }

        private void OnDestroy()
        {
            if (instance != this) instance = null;
        }
    }
}