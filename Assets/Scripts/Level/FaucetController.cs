using UnityEngine;
using Droppy.StatSystem;

public class FaucetController : MonoBehaviour
{
    [Header("Manager")]
    public FaucetsManager manager;

    [Header("Referências")]
    [SerializeField] private GameObject water;
    [SerializeField] private StatModifierTime modifier;

    public bool IsOpen { get; private set; } = false;

    private void Awake()
    {
        Open();
    }

    public void Open()
    {
        if (IsOpen) return;

        IsOpen = true;

        if (water != null)
            water.SetActive(true);

        if (modifier != null)
            modifier.enabled = true;
    }

    public void Close()
    {
        if (!IsOpen) return;

        IsOpen = false;

        if (water != null)
            water.SetActive(false);

        if (modifier != null)
            modifier.enabled = false;

        if (manager != null)
            manager.FaucetClosed(this);
    }

    private void OnMouseDown()
    {
        if (IsOpen)
            Close();
    }
}
