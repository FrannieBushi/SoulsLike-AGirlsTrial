using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    public CombatController playerCombat;
    public string attackID;

    void Start()
    {
        playerCombat = GetComponentInParent<CombatController>();
    }

    public int GetDamage()
    {
        return playerCombat.GetCurrentAttackDamage();
    }

    public void SetAttackID()
    {
        attackID = "Combo_" + playerCombat.combo + "_ID_" + playerCombat.attackActivationID;
    }
}
