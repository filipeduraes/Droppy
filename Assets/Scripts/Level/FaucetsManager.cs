using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaucetsManager : MonoBehaviour
{
    [Header("Configuração")]
    public GameObject faucetPrefab;   
    public Collider2D spawnArea;      
    public int maxSimultaneous = 10;  

    private List<FaucetController> activeFaucets = new List<FaucetController>();

    private void Start()
    {
        for (int i = 0; i < maxSimultaneous; i++)
        {
            SpawnNewFaucet();
        }
    }

    public void FaucetClosed(FaucetController faucet)
    {
        activeFaucets.Remove(faucet);
        SpawnNewFaucet();
    }

    void SpawnNewFaucet()
    {
        Vector2 pos = GetRandomPointInArea();
        GameObject f = Instantiate(faucetPrefab, pos, Quaternion.identity);

        FaucetController controller = f.GetComponent<FaucetController>();
        controller.manager = this;

        controller.Open();

        activeFaucets.Add(controller);
    }

    Vector2 GetRandomPointInArea()
    {
        var b = spawnArea.bounds;
        return new Vector2(
            Random.Range(b.min.x, b.max.x),
            Random.Range(b.min.y, b.max.y)
        );
    }
}
