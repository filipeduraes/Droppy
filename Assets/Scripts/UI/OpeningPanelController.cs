using UnityEngine;

public class OpeningPanelController : MonoBehaviour
{
    [SerializeField] private GameObject openingPanel; 

    private void Start()
    {
        openingPanel.SetActive(true);
    }

    public void ClosePanel()
    {
        openingPanel.SetActive(false);
    }
}
