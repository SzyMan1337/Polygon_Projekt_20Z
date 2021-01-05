using UnityEngine;

public class Rifle : Weapon
{

    public override void Shoot()
    {
        if (Input.GetMouseButton(0))
        {
            base.Shoot();
        }
    }

}
