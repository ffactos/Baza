using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField]
    float TimeBeforeDestroy = 10f;

    void Start()
    {
        Destroy(gameObject, TimeBeforeDestroy);
    }

}
