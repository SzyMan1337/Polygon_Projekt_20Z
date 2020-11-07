using UnityEngine;
using UnityEngine.Assertions;


public class Weapon : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab = null;
    [SerializeField] private Transform barrelEnd = null;
    [SerializeField] private float reloadTime = 0.2f;
    private float timeToShoot = 0.0f;


    private void Awake()
    {
        Assert.IsNotNull(projectilePrefab);
        Assert.IsNotNull(barrelEnd);
    }

    private void Update()
    {
        if ((Input.GetMouseButton(0)) && (timeToShoot <= 0.0f))
        {
            timeToShoot = reloadTime;
            Instantiate(projectilePrefab, barrelEnd.position, barrelEnd.rotation);
        }
        else if(timeToShoot > 0)
        {
            timeToShoot -= Time.deltaTime;
        }
    }
}