using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    private bool shot = false;


    protected override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonUp(0))
            shot = false;
    }

    public override void Shoot()
    {
        if (shot == false)
        {
            shot = true;
            base.Shoot();
        }
    }
}
