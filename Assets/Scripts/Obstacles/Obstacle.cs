using UnityEngine;

public class Obstacle : MonoBehaviour, ISpawnable
{
    public string Name;

    public float Speed;

    protected virtual void Update()
    {
        if (GameStateData.IsPaused) return;

        // move gate to player
        transform.Translate(Vector3.forward * MoveSpeed() * Time.deltaTime);
    }

    #region ISpawnable
    public string GetName()
    {
        return Name;
    }

    public virtual void ResetData(){ }
    #endregion

    protected virtual float MoveSpeed()
    {
        return GameTimeManager.GetSpeedByTime(Speed);
    }

    protected virtual void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            // once object is off screen delete it
            this.gameObject.SetActive(false);
        }
    }
}
