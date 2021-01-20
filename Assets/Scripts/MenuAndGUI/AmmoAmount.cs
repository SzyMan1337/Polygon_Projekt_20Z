using UnityEngine;
using UnityEngine.Assertions;
using TMPro;


public class AmmoAmount : MonoBehaviour
{
    private TextMeshProUGUI text;
    private PlayerController player;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        Assert.IsNotNull(text);

        player = GetComponentInParent<PlayerController>();
        Assert.IsNotNull(player);
    }

    private void Update()
    {
        text.text = player.WeaponManager.CurrentWeapon.CurrentAmmo.ToString();
    }
}
