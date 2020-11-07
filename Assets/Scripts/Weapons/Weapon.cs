using UnityEngine;
using UnityEngine.Assertions;


public class Weapon : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab = null;
    [SerializeField] private Transform barrelEnd = null;


    private void Awake()
    {
        Assert.IsNotNull(projectilePrefab);
        Assert.IsNotNull(barrelEnd);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(projectilePrefab, barrelEnd.position, barrelEnd.rotation);
        }
    }
}