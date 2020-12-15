using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bazook : Weapon
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetMouseButton(0))
        {
            SceneManager.Instance.Player.PushPlayerBack();
            Debug.Log("bazzok");
        }
    }
}
