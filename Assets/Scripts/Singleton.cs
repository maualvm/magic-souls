using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool bIsShuttingDown;

    private static object _lockObject = new object();

    private static T _instance;

    public static T Instance
    {
        get
        {
            if (!bIsShuttingDown)
            {
                lock (_lockObject)
                {
                    if (_instance == null)
                    {
                        var singletonGameObject = new GameObject();
                        _instance = singletonGameObject.AddComponent<T>();
                        DontDestroyOnLoad(singletonGameObject);
                    }

                }
                return _instance;
            }
            return null;
        }
    }

    private void OnApplicationQuit()
    {
        bIsShuttingDown = true;
    }

    private void OnDestroy()
    {
        bIsShuttingDown = true;
    }
}
