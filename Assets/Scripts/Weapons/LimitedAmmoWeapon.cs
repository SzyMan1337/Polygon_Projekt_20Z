using UnityEngine;


public class LimitedAmmoWeapon : Weapon
{
    [SerializeField, Range(1, 1000)] private int maxAmmo = 10;
    private int currentAmmo = 0;


    public int CurrentAmmo => currentAmmo;
    public int MaxAmmo => maxAmmo;

    protected override void Awake()
    {
        currentAmmo = maxAmmo;
        base.Awake();
    }

    public override void Shoot()
    {
        if (currentAmmo > 0 && shotCooldown <= 0.0f)
        {
            --currentAmmo;
            base.Shoot();
        }     
    }

    public void AddAmmo(int amount)
    {
        currentAmmo += amount;
    }
}
