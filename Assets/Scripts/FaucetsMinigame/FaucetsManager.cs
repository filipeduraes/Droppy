using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Droppy.FaucetsMinigame
{
    public class FaucetsManager : MonoBehaviour
    {
        [Header("Torneiras existentes na cena")]
        [SerializeField] private List<FaucetController> faucets;

        [Header("Intervalo entre aberturas aleatórias")]
        [SerializeField] private float minTime = 1.5f;
        [SerializeField] private float maxTime = 3.5f;

        private readonly List<FaucetController> activeFaucets = new();
        private float timer;

        private void OnEnable()
        {
            FaucetController.OnFaucetClosed += HandleFaucetClosed;
        }

        private void OnDisable()
        {
            FaucetController.OnFaucetClosed -= HandleFaucetClosed;
        }

        private void Start()
        {
            ResetTimer();
        }

        private void Update()
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                OpenRandomFaucet();
                ResetTimer();
            }
        }

        private void ResetTimer()
        {
            timer = Random.Range(minTime, maxTime);
        }

        private void OpenRandomFaucet()
        {
            List<FaucetController> closed = faucets.Where(f => !f.IsOpen).ToList();

            if (closed.Count == 0)
                return;

            FaucetController chosen = closed[Random.Range(0, closed.Count)];

            chosen.Open();
            activeFaucets.Add(chosen);
        }

        private void HandleFaucetClosed(FaucetController faucet)
        {
            activeFaucets.Remove(faucet);
        }
    }
}
