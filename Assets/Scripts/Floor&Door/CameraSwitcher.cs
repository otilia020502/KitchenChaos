using UnityEngine;

namespace Floor_Door
{
    public class CameraSwitcher : MonoBehaviour
    {
        public GameObject camera1;
        public GameObject camera2;
        public GameObject camera3;
        public GameObject player; // The GameObject that will trigger the switch

        private int currentCameraIndex;

        private void Start()
        {
            // Ensure only the first camera is active at the start
            camera1.SetActive(true);
            camera2.SetActive(false);
            camera3.SetActive(false);
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
                camera1.SetActive(false);
                camera2.SetActive(true);
                camera3.SetActive(false);
                currentCameraIndex = 2;
            }
            else if (currentCameraIndex == 2)
            {
                camera1.SetActive(false);
                camera2.SetActive(false);
                camera3.SetActive(true);
                currentCameraIndex = 3;
            }
            else if (currentCameraIndex == 3)
            {
                camera1.SetActive(true);
                camera2.SetActive(false);
                camera3.SetActive(false);
                currentCameraIndex = 1;
            }
        }
    }
}