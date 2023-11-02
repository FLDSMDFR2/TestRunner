using Unity.VisualScripting;
using UnityEngine;

public class Gate : Obstacle
{
    public bool Triggered;

    #region ISpawnable
    public override void RestData()
    {
        Triggered = false;
    }
    #endregion

    protected virtual void PerformTriggeredFunction(Player player)
    {
        player.TriggeredGate();

        this.gameObject.SetActive(false);
    }

    protected override void OnTriggerEnter(Collider collision)
    {
        base.OnTriggerEnter(collision);

        if (Triggered) return;

        var player = collision.gameObject.GetComponent<Player>();
        if (player != null && player.CanTriggerGate())
        {
            Triggered = true;
            PerformTriggeredFunction(player);
        }
    }
}
