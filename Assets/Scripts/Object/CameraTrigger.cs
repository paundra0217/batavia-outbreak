using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerMovement;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] private string setName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CameraManager.ChangeSetting(setName);
        }   
    }
}
