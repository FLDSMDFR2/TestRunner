using TMPro;
using UnityEngine;

public class FrameRate : MonoBehaviour
{
    protected int lastIndex;
    protected float[] frameDeltaTimeArray;

    public TextMeshProUGUI FrameRateLabel;

    protected virtual void Awake()
    {
        frameDeltaTimeArray = new float[50];
        lastIndex = 0;
    }

    protected virtual void Update()
    {
        frameDeltaTimeArray[lastIndex] = Time.deltaTime;
        lastIndex = (lastIndex +1) % frameDeltaTimeArray.Length;

        FrameRateLabel.text = "FPS : " + Mathf.RoundToInt(CalculateFrameRate()).ToString();
    }

    protected virtual float CalculateFrameRate()
    {
        float total = 0;
        foreach (var frame in frameDeltaTimeArray)
        {
            total += frame;
        }
        return frameDeltaTimeArray.Length / total;
    }
}
