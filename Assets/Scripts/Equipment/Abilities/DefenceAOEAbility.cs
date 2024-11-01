using UnityEngine;

public class DefenceAOEAbility : AOEAbility
{
    public override void TriggerAbility(Player player) 
    {
        if (!CanPerformAbility()) return;

        if (AOEImpact == null) return;

        base.TriggerAbility(player);

        var impact = AOEImpact.GetComponent<AOEImpact>();
        if (impact == null) return;

        var aoeObj = ObjectPooler.GetObject(impact.GetName() + "_DefenceAbility", AOEImpact, player.gameObject.transform.position,Quaternion.identity);
        if (aoeObj != null)
        {
            var aoeScript = aoeObj.GetComponent<AOEImpact>();
            if (aoeScript != null)
            {
                aoeScript.TriggerImpact(player.gameObject.transform.position, this, CurrentDamage, CurrentRadius, CurrentDuration);
            }
        }    
    }
}
