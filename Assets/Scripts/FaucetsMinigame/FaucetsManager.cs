using System.Collections.Generic;
using Droppy.Shared;
using UnityEngine;

namespace Droppy.FaucetsMinigame
{
    public class FaucetsManager : MonoBehaviour
    {
        [SerializeField] private List<Faucet> faucets;
        [SerializeField] private float minTime = 1.5f;
        [SerializeField] private float maxTime = 3.5f;

        private readonly List<Faucet> closedFaucets = new();
        private float timer = 0;

        private void OnEnable()
        {
            foreach (Faucet faucet in faucets)
            {
               closedFaucets.Add(faucet);
               faucet.SetOpen(false);
               faucet.OnClosed += HandleClosed;
            }
            
            StartTimer();
        }

        private void OnDisable()
        {
            foreach (Faucet faucet in faucets)
            {
                faucet.OnClosed -= HandleClosed;
            }
        }

        private void Update()
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                OpenRandomFaucet();
                StartTimer();
            }
        }

        private void StartTimer()
        {
            timer = Random.Range(minTime, maxTime);
        }

        private void OpenRandomFaucet()
        {
            if (closedFaucets.Count == 0)
            {
                return;
            }

            Faucet chosen = closedFaucets.GetRandomElement();
            chosen.SetOpen(true);
            closedFaucets.Remove(chosen);
        }

        private void HandleClosed(Faucet faucet)
        {
            if (!closedFaucets.Contains(faucet))
            {
                closedFaucets.Add(faucet);
            }
        }
    }
}
