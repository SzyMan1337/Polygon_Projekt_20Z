using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class WeaponManager : MonoBehaviour
{
    private List<LimitedAmmoWeapon> weapons = new List<LimitedAmmoWeapon>();
    private int currentWeaponIndex = 0;


    public bool HasWeapon => CurrentWeapon != null;
    private LimitedAmmoWeapon CurrentWeapon => weapons.Count > 0 ? weapons[currentWeaponIndex] : null;


    private void Awake()
    {
        foreach (var weapon in GetComponentsInChildren<LimitedAmmoWeapon>())
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
        var weapon = other.GetComponent<LimitedAmmoWeapon>();
        if (weapon != null && !weapons.Contains(weapon))
        {
            PickupWeapon(weapon);
        }
    }

    private void PickupWeapon(LimitedAmmoWeapon weapon)
    {
        Assert.IsNotNull(weapon);
        foreach(LimitedAmmoWeapon wpn in weapons)
        {
            Debug.Log(wpn.Name);
            Debug.Log(weapon.Name);
            if (string.Equals(wpn.Name, weapon.Name))
            {
                Debug.Log("juz bylo");
                wpn.AddAmmo(weapon.MaxAmmo);
                weapon.gameObject.SetActive(false);
                return;
            }
        }
        Debug.Log("nowa");
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
