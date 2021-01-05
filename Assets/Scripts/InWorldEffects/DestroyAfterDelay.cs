using UnityEngine;


public class DestroyAfterDelay : MonoBehaviour
{
    [SerializeField, Range(0.0f, 100.0f)] private float lifetime = 1.0f;


    private void Update()
    {
        if (lifetime > 0.0f)
        {
            lifetime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
