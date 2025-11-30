using UnityEngine;
using Droppy.StatSystem;

public class StatColorChanger : MonoBehaviour
{
    [SerializeField] private Stat stat;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color fullColor = Color.white;                
    [SerializeField] private Color lowColor = Color.gray; 

    private void OnEnable()
    {
        StatManager.OnStatModified += OnPurityChanged;
        
        OnPurityChanged(stat.ID);
    }

    private void OnDisable()
    {
        StatManager.OnStatModified -= OnPurityChanged;
    }

    private void OnPurityChanged(string id)
    {
        if (id != stat.ID)
        {
            return;
        }

        float purity = StatManager.Read(stat);
        float t = purity / stat.InitialAmount;

        spriteRenderer.color = Color.Lerp(lowColor, fullColor, t);
    }
}
