using System;
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

        private MovementSetting currentMovementSettings;

        private static CameraManager _instance;

        private void Awake()
        {
            _instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            currentMovementSettings = movementSettings[0];    
        }

        // Update is called once per frame
        void Update()
        {

        }

        public static MovementSetting GetMovementSettings()
        {
            return _instance.currentMovementSettings;
        }
    }
}