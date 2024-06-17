using Cinemachine;
using UnityEngine;

namespace Floor_Door
{
    public class CameraSwitcher : MonoBehaviour
    {
        public CinemachineVirtualCamera camera1;
        public CinemachineVirtualCamera camera2;
        public CinemachineVirtualCamera camera3;
        public GameObject player; // The GameObject that will trigger the switch

        private int currentCameraIndex;

        private void Start()
        {
            camera1.Priority=12;
            
            currentCameraIndex = 1;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == player)
            {
                SwitchCamera();
            }
        }

        private void SwitchCamera()
        {
            if (currentCameraIndex == 1)
            {
                camera1.Priority=10;
                camera2.Priority=12;
                camera3.Priority=10;
                currentCameraIndex = 2;
            }
            else if (currentCameraIndex == 2)
            {
                camera1.Priority=10;
                camera2.Priority=10;
                camera3.Priority=12;
                currentCameraIndex = 3;
            }
            else if (currentCameraIndex == 3)
            {
                camera1.Priority=12;
                camera2.Priority=10;
                camera3.Priority=10;
                currentCameraIndex = 1;
            }
        }
    }
}