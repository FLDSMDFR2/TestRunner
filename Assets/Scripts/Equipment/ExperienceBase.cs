using UnityEngine;

public class ExperienceBase : MonoBehaviour
{
    [Header("Experience")]
    public int Experience;

    public int Level => GetLevel();

    protected float levelConstant = .2f;

    public virtual int GetLevel()
    {
        return Mathf.FloorToInt(levelConstant * Mathf.Sqrt(Experience) + 1);
    }

    public virtual void GainExperience(int experience)
    {
        Experience += experience;
    }
}
