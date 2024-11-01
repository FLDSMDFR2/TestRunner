using System.Reflection;
using TMPro;
using UnityEngine;

[RequireComponent(typeof (TextMeshPro))]
public class DamagePopup : MonoBehaviour
{
    public GameObject prefab;

    public float DisappearTimer = 1f;
    protected float disappearTimer;

    public float DisappearSpeed = 3f;

    public Vector3 MoveVector = new Vector3(0,5,0);
    protected Vector3 moveVector;

    public float MoveVectorOffeset = 30f;
    public float MoveSlowDown = 8f;

    protected TextMeshPro textMeshPro;
    protected Color textColor;

    public void Create(Vector3 postion, int damageAmount, float fontSize = 5, Color textColor = new Color())
    {
        var popup = ObjectPooler.GetObject("DamagePopup", prefab, postion, Quaternion.identity);
        if (popup != null)
        {
            var script = popup.GetComponent<DamagePopup>();
            script.Setup(damageAmount, fontSize, textColor);
        }
    }

    protected virtual void Awake()
    {
        textMeshPro = GetComponent<TextMeshPro>();
    }

    protected virtual void Setup(int damageAmount, float fontSize, Color textColor)
    {
        disappearTimer = DisappearTimer;
        moveVector = MoveVector * MoveVectorOffeset;

        textMeshPro.fontSize = fontSize;
        textMeshPro.color = this.textColor = textColor;
        textMeshPro.SetText(damageAmount.ToString());  
    }

    protected virtual void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * MoveSlowDown * Time.deltaTime;

        disappearTimer -= Time.deltaTime;
        if (disappearTimer <= 0) 
        {
            textColor.a -= DisappearSpeed * Time.deltaTime;
            textMeshPro.color = textColor;
            if (textColor.a <= 0) this.gameObject.SetActive(false);
        }
    }

}
