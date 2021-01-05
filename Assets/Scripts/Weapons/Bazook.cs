using UnityEngine;

public class Bazook : Weapon
{
    private static readonly int maxAmmunitionCount = 3;
    private int ammunitionCount = maxAmmunitionCount;
    private readonly float pushBackForce = 3.0f;


    public override void Shoot()
    {
        if (ammunitionCount > 0) 
        {
            base.Shoot();
            --ammunitionCount;

            // push player back
            SceneManager.Instance.Player.Body.AddForce(SceneManager.Instance.Player.transform.forward * pushBackForce, ForceMode.Impulse);
        }     
    }

    public override void DetachWeapon()
    {
        base.DetachWeapon();

        // when dropped the ammunition count resets
        ammunitionCount = maxAmmunitionCount;
    }

}
