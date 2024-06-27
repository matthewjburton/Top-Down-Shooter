using System;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Weapon[] weapons;
    public Weapon currentWeapon;
    public static event Action OnWeaponSwitched;

    void Update()
    {
        if (InputManager.Instance.AttackJustPressed)
            currentWeapon.Attack(gameObject);

        if (InputManager.Instance.SwitchWeaponInput)
            SwitchWeapon(GetNextWeapon());
    }

    void SwitchWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
        OnWeaponSwitched?.Invoke();
    }

    Weapon GetNextWeapon()
    {
        int index = Array.IndexOf(weapons, currentWeapon);
        index = (index + 1) % weapons.Length;
        return weapons[index];
    }
}