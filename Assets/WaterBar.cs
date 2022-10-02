using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterBar : MonoBehaviour
{
    [SerializeField]
    private WaterManager _water;
    
    [SerializeField]
    private Image _fillImage;

    [SerializeField]
    private Text _timerText;

    // Update is called once per frame
    void Update()
    {
        _fillImage.fillAmount = 1 - _water.DisplayPercent;
        // _timerText.text = Mathf.CeilToInt(_water.TimeUntilRise).ToString();
    }
}
