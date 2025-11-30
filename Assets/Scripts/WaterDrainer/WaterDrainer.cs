using UnityEngine;
using Droppy.StatSystem;


namespace Droppy.WaterDrainer
{
    public class WaterDrainer : MonoBehaviour 
    {
        [SerializedField] private Stat waterStat;
        [SerializedField] private float drainRatePerSecond = 0.5f;

        void Update()
        {
            if (waterStat==null)
            {
                Debug.LogError("O Stat da Agua não foi configurado")
                return;
            }

            DrainWater();
        }

        private void DrainWater()
        {
            float amountToDrain = drainRatePerSecond*Time.deltaTime;
            StatModifier drainModifier = new StatModifier(StatModifierType.Add, -amountToDrain);

            StatManager.Modify(waterStat, drainModifier);
        }
    }
}