using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private Weapon[] weapons; // weapon[0] is the main weapon - we start the game with it
    private int currentWeapon = 1;


    public Weapon FirstWeapon
    {
        get => weapons[0];
    }

    public Weapon SecondWeapon
    {
        get => weapons[1];
        set
        {
            weapons[1] = value;
        }
    }

    private void Awake()
    {
        weapons = new Weapon[2];
        weapons = GetComponentsInChildren<Weapon>();
        ChangeActive(0, 1);
    }

    private void ChangeActive(int i, int j) 
    {
        weapons[i].gameObject.SetActive(true);
        weapons[j].gameObject.SetActive(false);
    }

    public Weapon ChangeWeapon()
    {
        int curr = 0, sec = 1;
        if (weapons[1] != null && currentWeapon == 0)
        {
            curr = 1;
            sec = 0;
        }

        ChangeActive(curr, sec);
        currentWeapon = curr;
        return weapons[curr];
    }

}
