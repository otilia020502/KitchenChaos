using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floor_Door
{
    public class RoomThree : MonoBehaviour
    {
        // List to store rows of the floor
        public List<Transform> floorRows;

        // Colors for alternating rows
        public Color whiteColor = Color.white;
        public Color blackColor = Color.black;

        // Speed of the movement
        public float moveSpeed = 5f;

        // Dictionary to store original positions of rows
        private Dictionary<Transform, Vector3> originalPositions;

        void Start()
        {
            InitializeRowColors();
            CacheOriginalPositions();
        }

        void Update()
        {
            MoveRows();
        }

        private void InitializeRowColors()
        {
            for (int i = 0; i < floorRows.Count; i++)
            {
                Color rowColor = (i % 2 == 0) ? blackColor : whiteColor;
                Transform row = floorRows[i];
                for (int j = 0; j < row.childCount; j++)
                {
                    Renderer renderer = row.GetChild(j).GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material.color = rowColor;
                    }
                }
            }
        }

        private void CacheOriginalPositions()
        {
            originalPositions = new Dictionary<Transform, Vector3>();
            foreach (Transform row in floorRows)
            {
                originalPositions[row] = row.position;
            }
        }

        private void MoveRows()
        {
            foreach (Transform row in floorRows)
            {
                // Move row to the left
                row.position += Vector3.left * moveSpeed * Time.deltaTime;

                // Check if the row needs to wrap around to the right
                Vector3 firstRowOriginalPosition = originalPositions[floorRows[0]];
                

                if (row.position.x <= firstRowOriginalPosition.x - row.GetChild(0).localScale.x)
                {
                    // Find the last row's position
                    Vector3 lastRow = originalPositions[floorRows[floorRows.Count - 1]];

                    // Move the current row to the position of the last row
                    row.position = new Vector3(lastRow.x , lastRow.y, lastRow.z);
                }
            }
        }
    }
}
