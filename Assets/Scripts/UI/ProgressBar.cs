using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image graphics;

    public float maxValue;

    private float _value;

    public float Value
    {
        get { return _value; } set { _value = value; ValueChange(); }
    }

    private void ValueChange()
    {
        var k = maxValue / _value;
        graphics.fillAmount = (100 / k) / 100;
    }
}
