using UnityEngine;
using Droppy.StatSystem;

public class PurityColorChanger : MonoBehaviour
{
    [SerializeField] private Stat purityStat;
    [SerializeField] private SpriteRenderer spriteRenderer;


    private Color fullPurityColor = Color.white;                
    private Color lowPurityColor = new Color(0.6f, 0.6f, 0.6f); 

    private void OnEnable()
    {
        StatManager.OnStatModified += OnPurityChanged;
    }

    private void OnDisable()
    {
        StatManager.OnStatModified -= OnPurityChanged;
    }

    private void OnPurityChanged(string id)
    {
        if (id != purityStat.ID)
            return;

        float purity = StatManager.Read(purityStat);

        float t = purity / 100f;

        spriteRenderer.color = Color.Lerp(lowPurityColor, fullPurityColor, t);
    }
}
