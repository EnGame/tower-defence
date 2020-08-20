using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = GetComponent<T>();
        }
        else
        {
            Debug.Log("Is not null");
            Destroy(gameObject);
        }
    }

    public static T GetInstance()
    {
        return _instance;
    }
}
