using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class WeaponManager : MonoBehaviour
{
    private List<Weapon> weapons = new List<Weapon>(); // weapon[0] is the main weapon - we start the game with it
    private int currentWeapon = 0;
    private int numberOfWeapons = 1;

    public Weapon CurrentWeapon => weapons[currentWeapon];


    private void Awake()
    {
        weapons.AddRange(GetComponentsInChildren<Weapon>());
        Assert.IsNotNull(weapons);
        weapons[currentWeapon].OnGround = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            Debug.Log("curr weapon: " + currentWeapon);
    }

    public void DetachCurrentWeapon()
    {
        if (currentWeapon > 0)
        {
            --numberOfWeapons;
            weapons[currentWeapon].DetachWeapon();
            weapons.RemoveAt(currentWeapon--);
            weapons[currentWeapon].gameObject.SetActive(true);
        }
    }

    public void AddWeapon(Weapon w)
    {
        // Set current weapon to inactive
        weapons[currentWeapon].gameObject.SetActive(false);

        // Increase number of weapons in hand
        ++numberOfWeapons;

        // Add weapon
        weapons.Add(w);

        // Set WeaponManager as parent
        w.transform.SetParent(this.transform);
        w.transform.localPosition = Vector3.zero;
        w.transform.localRotation = Quaternion.Euler(Vector3.zero);
        w.transform.localScale = Vector3.one;

        // Set the current weapon to the added weapon
        currentWeapon = numberOfWeapons - 1;
    }

    public void ChangeWeaponUp()
    {
        weapons[currentWeapon].gameObject.SetActive(false);
        currentWeapon = (currentWeapon + 1) % numberOfWeapons;
        weapons[currentWeapon].gameObject.SetActive(true);
    }

    public void ChangeWeaponDown()
    {
        weapons[currentWeapon].gameObject.SetActive(false);
        if (currentWeapon == 0)
            currentWeapon = numberOfWeapons - 1;
        else
            currentWeapon = (currentWeapon - 1) % numberOfWeapons;
        weapons[currentWeapon].gameObject.SetActive(true);
    }

}
