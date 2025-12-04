using Droppy.StatSystem;
using UnityEngine;
using UnityEngine.UI;

public class WaterTankUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Stat waterStat;

    [SerializeField] private Color waterColor = new Color(0.1f, 0.55f, 1f);


    private void OnEnable()
    {
        StatManager.OnWaterLevelChanged += UpdateLevel;

        UpdateLevel(StatManager.Read(waterStat));
    }

    private void OnDisable()
    {
        StatManager.OnWaterLevelChanged -= UpdateLevel;
    }

    private void UpdateLevel(float value)
    {
        float percent = value / waterStat.InitialAmount;

        fillImage.fillAmount = percent;

        //fillImage.color = Color.Lerp(emptyColor, fullColor, percent);
    }
}


