using Cinemachine;
using UnityEngine;

namespace Floor_Door
{
    public class CameraSwitcher : MonoBehaviour
    {
        public CinemachineVirtualCamera camera1; 
        public CinemachineVirtualCamera camera2; 
        public CinemachineVirtualCamera camera3; 
        public GameObject player;

        private void Start()
        {
            
            camera1.gameObject.SetActive(true);
            camera2.gameObject.SetActive(true);
            camera3.gameObject.SetActive(true);

            camera1.Priority = 12;
            camera2.Priority = 10;
            camera3.Priority = 10;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == player)
            {
                if (gameObject.name == "TriggerRoom1")
                {
                    SwitchToRoom(1);
                }
                else if (gameObject.name == "TriggerRoom2")
                {
                    SwitchToRoom(2);
                }
                else if (gameObject.name == "TriggerRoom3")
                {
                    SwitchToRoom(3);
                }
            }
        }

        private void SwitchToRoom(int roomNumber)
        {
            switch (roomNumber)
            {
                case 1:
                    camera1.Priority = 12;
                    camera2.Priority = 10;
                    camera3.Priority = 10;
                    break;
                case 2:
                    camera1.Priority = 10;
                    camera2.Priority = 12;
                    camera3.Priority = 10;
                    break;
                case 3:
                    camera1.Priority = 10;
                    camera2.Priority = 10;
                    camera3.Priority = 12;
                    break;
            }
        }
    }
}
