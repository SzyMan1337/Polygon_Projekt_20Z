using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class WeaponManager : MonoBehaviour
{
    private List<Weapon> weapons = new List<Weapon>();
    private int currentWeaponIndex = 0;


    public bool HasWeapon => CurrentWeapon != null;
    private Weapon CurrentWeapon => weapons.Count > 0 ? weapons[currentWeaponIndex] : null;


    private void Awake()
    {
        foreach (var weapon in GetComponentsInChildren<Weapon>())
        {
            PickupWeapon(weapon);
        }
        if (weapons.Count > 0)
        {
            weapons[currentWeaponIndex].gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var weapon = other.GetComponent<Weapon>();
        if (weapon != null && !weapons.Contains(weapon))
        {
            PickupWeapon(weapon);
        }
    }

    private void PickupWeapon(Weapon weapon)
    {
        Assert.IsNotNull(weapon);
        weapon.transform.SetParent(this.transform);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        weapons.Add(weapon);
        weapon.gameObject.SetActive(false);
    }

    public void ShootCurrentWeapon()
    {
        CurrentWeapon?.Shoot();
    }

    public void DropCurrentWeapon()
    {
        if (HasWeapon && !CurrentWeapon.IsPermanent)
        {
            var weapon = CurrentWeapon;
            weapons.Remove(weapon);
            Destroy(weapon.gameObject);
            if (currentWeaponIndex > 0)
            {
                --currentWeaponIndex;
            }
            if (HasWeapon)
            {
                CurrentWeapon.gameObject.SetActive(true);
            }
        }
    }

    public void CycleWeaponUp()
    {
        if (currentWeaponIndex < weapons.Count - 1)
        {
            CurrentWeapon.gameObject.SetActive(false);
            ++currentWeaponIndex;
            CurrentWeapon.gameObject.SetActive(true);
        }
    }

    public void CycleWeaponDown()
    {
        if (currentWeaponIndex > 0)
        {
            CurrentWeapon.gameObject.SetActive(false);
            --currentWeaponIndex;
            CurrentWeapon.gameObject.SetActive(true);
        }
    }
}
