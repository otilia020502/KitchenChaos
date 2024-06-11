using UnityEngine;

namespace Floor_Door
{
    public class RoomTwo : MonoBehaviour
    {
        [SerializeField] private Transform floorsParent; 
        [SerializeField] private Color blackColor = Color.black; 
        [SerializeField] private Color redColor = Color.red;
        [SerializeField] private LayerMask playerLayer; // Layer mask to identify the player

        private void Start()
        {
            if (floorsParent == null)
            {
                Debug.LogError("Floors parent is not assigned.");
                return;
            }

            SetInitialColors();
        }

        private void Update()
        {
            SetInitialColors();
            CheckPlayerPosition();
        }

        private void SetInitialColors()
        {
            for (int i = 0; i < floorsParent.childCount; i++)
            {
                Transform row = floorsParent.GetChild(i);

                for (int j = 0; j < row.childCount; j++)
                {
                    Renderer renderer = row.GetChild(j).GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material.color = blackColor;
                    }
                }
            }
        }
        

        private void CheckPlayerPosition()
        {
            for (int i = 0; i < floorsParent.childCount; i++)
            {
                Transform row = floorsParent.GetChild(i);

                for (int j = 0; j < row.childCount; j++)
                {
                    Transform cube = row.GetChild(j);

                    if (IsPlayerOnCube(cube))
                    {
                        SetRowAndColumnColor(i, j, redColor);
                        return; // Exit after finding the player to ensure only one row and column is red
                    }
                }
            }
        }

        private bool IsPlayerOnCube(Transform cube)
        {
            Collider[] colliders = Physics.OverlapBox(cube.position, cube.localScale / 2, Quaternion.identity, playerLayer);
           // Quaternion.identity: The orientation of the box. Quaternion.identity means no rotation, so the box is aligned with the world axes.
            return colliders.Length > 0;
        }

        private void SetRowAndColumnColor(int rowIndex, int columnIndex, Color color)
        {
            // Set the color of the entire row
            Transform row = floorsParent.GetChild(rowIndex);
            for (int j = 0; j < row.childCount; j++)
            {
                Renderer renderer = row.GetChild(j).GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = color;
                }
            }

            // Set the color of the entire column
            for (int i = 0; i < floorsParent.childCount; i++)
            {
                Renderer renderer = floorsParent.GetChild(i).GetChild(columnIndex).GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = color;
                }
            }
        }
    }
}
