using Cinemachine;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private bool miniMapMode = true;

    private static MapCamera _instance;

    private void Awake()
    {
        _instance = this;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        virtualCamera.m_Lens.OrthographicSize = 25f;
    }

    //private void LateUpdate()
    //{
    //    GameObject player = GameObject.FindGameObjectWithTag("Player");
    //    if (player == null) return;

    //    if (miniMapMode)
    //    {
    //        virtualCamera.m_Lens.OrthographicSize = 25f;

    //        Vector3 playerRotation = player.transform.rotation.eulerAngles;
    //        playerRotation.x = 90f;
    //        transform.rotation = Quaternion.Euler(playerRotation);
    //    }
    //    else 
    //    {
    //        virtualCamera.m_Lens.OrthographicSize = 50f;

    //        transform.rotation = Quaternion.Euler(new Vector3(90f, 0, 0));
    //    }
    //}

    public static void ToggleCamera()
    {
        _instance.miniMapMode = !_instance.miniMapMode;

        if (_instance.miniMapMode)
        {
            _instance.virtualCamera.m_Lens.OrthographicSize = 25f;
        }
        else
        {
            _instance.virtualCamera.m_Lens.OrthographicSize = 50f;
        }
    }

    public static bool IsInMiniMapMode()
    {
        return _instance.miniMapMode;
    }
}
