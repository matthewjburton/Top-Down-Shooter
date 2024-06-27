using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] Weapon currentWeapon;

    void Update()
    {
        if (InputManager.Instance.AttackJustPressed)
            currentWeapon.Attack(gameObject);
    }
}