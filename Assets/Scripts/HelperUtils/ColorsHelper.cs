using UnityEngine;

public class ColorsHelper : MonoBehaviour
{
    public Color BaseDamageColor;
    protected static Color _baseDamageColor;
    public static Color GetBaseDamageColor()
    {
        return _baseDamageColor; 
    }

    protected virtual void Awake()
    {
        _baseDamageColor = BaseDamageColor;
    }
}
