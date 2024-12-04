using UnityEngine;

public class MouseTarget : MonoBehaviour
{
    [SerializeField] private LayerMask layers;

    private Camera cam;
    private Ray ray;

    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layers))
            transform.position = raycastHit.point;
    }
}
