using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGate : Gate
{
    public int HealthToGain;

    protected override void PerformTriggeredFunction(Player player)
    {
        base.PerformTriggeredFunction(player);

        player.UpdateHealth(HealthToGain);
    }
}
