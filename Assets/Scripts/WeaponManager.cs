using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class WeaponManager : MonoBehaviour
{
    private List<LimitedAmmoWeapon> weapons = new List<LimitedAmmoWeapon>();
    private int currentWeaponIndex = 0;


    public bool HasWeapon => currentWeapon != null;
    private LimitedAmmoWeapon currentWeapon => weapons.Count > 0 ? weapons[currentWeaponIndex] : null;
    public LimitedAmmoWeapon CurrentWeapon => currentWeapon;


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
            if (string.Equals(wpn.Name, weapon.Name))
            {
                wpn.AddAmmo(weapon.MaxAmmo);
                weapon.gameObject.SetActive(false);
                return;
            }
        }
        weapon.transform.SetParent(this.transform);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        weapons.Add(weapon);
        weapon.gameObject.SetActive(false);
    }

    public void ShootCurrentWeapon()
    {
        currentWeapon?.Shoot();
    }

    public void DropCurrentWeapon()
    {
        if (HasWeapon && !currentWeapon.IsPermanent)
        {
            var weapon = currentWeapon;
            weapons.Remove(weapon);
            Destroy(weapon.gameObject);
            if (currentWeaponIndex > 0)
            {
                --currentWeaponIndex;
            }
            if (HasWeapon)
            {
                currentWeapon.gameObject.SetActive(true);
            }
        }
    }

    public void CycleWeaponUp()
    {
        if (currentWeaponIndex < weapons.Count - 1)
        {
            currentWeapon.gameObject.SetActive(false);
            ++currentWeaponIndex;
            currentWeapon.gameObject.SetActive(true);
        }
    }

    public void CycleWeaponDown()
    {
        if (currentWeaponIndex > 0)
        {
            currentWeapon.gameObject.SetActive(false);
            --currentWeaponIndex;
            currentWeapon.gameObject.SetActive(true);
        }
    }
}
