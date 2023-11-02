using UnityEngine;

public class DefenceAOEAbility : Ability
{
    public int Damage;
    public int Radius;
    public GameObject AOEImpact;

    public override void TriggerAbility(Player player) 
    {
        if (AOEImpact == null) return;

        var impact = AOEImpact.GetComponent<AOEImpact>();
        if (impact == null) return;

        var aoeObj = ObjectPooler.GetObject(impact.GetName() + "DefenceAbility", AOEImpact, player.gameObject.transform.position,Quaternion.identity);
        if (aoeObj != null)
        {
            var aoeScript = aoeObj.GetComponent<AOEImpact>();
            if (aoeScript != null)
            {
                aoeScript.Damage = Damage;
                aoeScript.Radius = Radius;
                aoeScript.TriggerImpact(player.gameObject.transform.position);
            }
        }    
    }
}
