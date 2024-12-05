using Cinemachine;
using System;
using System.Linq;
using UnityEngine;

namespace PlayerMovement
{
    [Serializable]
    public enum MovementDirection
    {
        PositiveX,
        PositiveZ,
        NegativeX,
        NegativeZ,
    }

    [Serializable]
    public class MovementSetting
    {
        public string settingName;
        public Vector3 cameraOffset;
        public MovementDirection wDirection;
        public MovementDirection aDirection;
        public MovementDirection sDirection;
        public MovementDirection dDirection;
    }

    public class CameraManager : MonoBehaviour
    {
        public MovementSetting[] movementSettings;
        public AnimationCurve cameraTransition;

        private MovementSetting currentMovementSettings;
        private CinemachineVirtualCamera virtualCamera;
        private CinemachineTransposer virtualCamTransposer;
        private Vector3 oldOffset;
        private Vector3 newOffset;

        private float currentTransitionTime = 0.5f;

        private static CameraManager _instance;

        private void Awake()
        {
            _instance = this;
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
            virtualCamTransposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        }

        private void Start()
        {
            currentMovementSettings = movementSettings[0];
            virtualCamTransposer.m_FollowOffset = currentMovementSettings.cameraOffset;
        }

        private void Update()
        {
            if (currentTransitionTime < 0.5f)
            {
                virtualCamTransposer.m_FollowOffset.x = Mathf.Lerp(oldOffset.x, newOffset.x, cameraTransition.Evaluate(currentTransitionTime));
                virtualCamTransposer.m_FollowOffset.y = Mathf.Lerp(oldOffset.y, newOffset.y, cameraTransition.Evaluate(currentTransitionTime));
                virtualCamTransposer.m_FollowOffset.z = Mathf.Lerp(oldOffset.z, newOffset.z, cameraTransition.Evaluate(currentTransitionTime));

                currentTransitionTime += Time.deltaTime;
            }
        }

        public static MovementSetting GetMovementSettings()
        {
            return _instance.currentMovementSettings;
        }

        public static void ChangeSetting(string setName)
        {
            var selectedSet = _instance.movementSettings.FirstOrDefault(e => e.settingName == setName);
            if (selectedSet == null)
            {
                Debug.LogWarning("Camera setting is not valid.");
                return;
            }

            _instance.currentMovementSettings = selectedSet;

            _instance.ApplySettings();
        }

        private void ApplySettings()
        {
            oldOffset = virtualCamTransposer.m_FollowOffset;
            newOffset = currentMovementSettings.cameraOffset;

            currentTransitionTime = 0f;
        }
    }
}