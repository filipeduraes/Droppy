using UnityEngine;
using Droppy.StatSystem;

public class PurityDamage : MonoBehaviour
{
    [SerializeField] private Stat purityStat;

    public void ReducePurity(float amount)
    {
        StatModifier modifier = new StatModifier(
            StatModifierType.Add,   
            -amount               
        );

        // Aplicar na Stat
        StatManager.Modify(purityStat, modifier);
    }

    private void Update()
    {
        // Aperte a tecla P para testar
        if (Input.GetKeyDown(KeyCode.P))
        {
            ReducePurity(10);
            Debug.Log("PUREZA -10 (teste)");
        }
    }

}


