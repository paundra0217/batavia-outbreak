using UnityEngine;

public class UIMap : MonoBehaviour
{
    public GameObject miniMap;
    [SerializeField] private GameObject bigMap;

    private void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            miniMap.SetActive(false);
            bigMap.SetActive(false);
            return;
        }

        if (MapCamera.IsInMiniMapMode())
        {
            miniMap.SetActive(true);
            bigMap.SetActive(false);
        }
        else
        {
            miniMap.SetActive(false);
            bigMap.SetActive(true);
        }
    }
}
