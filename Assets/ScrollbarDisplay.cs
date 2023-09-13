using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScrollbarDisplay : MonoBehaviour
{
    
    [SerializeField] TMP_Text valueText;
    [SerializeField] string preprendText;
    [SerializeField] string playerPrefsName;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Slider>().value = (float) PlayerPrefs.GetInt(playerPrefsName, (int) GetComponent<Slider>().value);
    }

    public void UpdateText(float value)
    {
        valueText.text = preprendText + value;
        PlayerPrefs.SetInt(playerPrefsName, (int) value);
    }
}
