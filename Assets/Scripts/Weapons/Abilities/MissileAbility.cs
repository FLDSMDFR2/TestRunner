using UnityEngine;

public class MissileAbility : Ability 
{ 
    public GameObject Missile;

    public override void TriggerAbility(Player player)
    {
        if (Missile == null) return;

        var missileScript = Missile.GetComponent<Missile>();
        if (missileScript)
        {
            var missileGo = ObjectPooler.GetObject(missileScript.Name, Missile, player.FireLocation.position, player.FireLocation.transform.rotation);
            if (missileGo != null)
            {
                var spawnScript = missileGo.GetComponent<Missile>();
                spawnScript.Fire(player.FireLocation.position);
            }
        }
    }
}
