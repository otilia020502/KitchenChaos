using System.Collections;
using UnityEngine;

namespace Floor_Door
{
    public class RoomThree : MonoBehaviour
    {
        [SerializeField] private Transform floorsParent; // Parent object containing all floors
        [SerializeField] private Color whiteColor = Color.white; // Color for white tiles
        [SerializeField] private Color blackColor = Color.black; // Color for black tiles
        [SerializeField] private float changeInterval = 0.5f; // Time interval between each row's color change
        [SerializeField] private float moveDistance = 1f; // Distance to move each row
        [SerializeField] private float moveSpeed = 1f; // Speed of the row movement

        private Vector3[] originalPositions;
        private float rowWidth;

        private void Start()
        {
            if (floorsParent == null)
            {
                Debug.LogError("Floors parent is not assigned.");
                return;
            }

            InitializeRowColors();
            CacheOriginalPositions();
            CalculateRowWidth();
            StartCoroutine(MoveRows());
        }

        private void InitializeRowColors()
        {
            for (int i = 0; i < floorsParent.childCount; i++)
            {
                Transform row = floorsParent.GetChild(i);
                Color initialColor = (i % 2 == 0) ? blackColor : whiteColor;

                for (int j = 0; j < row.childCount; j++)
                {
                    Renderer renderer = row.GetChild(j).GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material.color = initialColor;
                    }
                }
            }
        }

        private void CacheOriginalPositions()
        {
            originalPositions = new Vector3[floorsParent.childCount];
            for (int i = 0; i < floorsParent.childCount; i++)
            {
                originalPositions[i] = floorsParent.GetChild(i).position;
            }
        }

        private void CalculateRowWidth()
        {
            if (floorsParent.childCount > 0 && floorsParent.GetChild(0).childCount > 0)
            {
                float tileWidth = floorsParent.GetChild(0).GetChild(0).localScale.x;
                rowWidth = tileWidth * floorsParent.GetChild(0).childCount;
            }
        }

        private IEnumerator MoveRows()
        {
            while (true)
            {
                // Move rows to the left
                yield return StartCoroutine(MoveAllRows(Vector3.left * moveDistance));

                yield return new WaitForSeconds(changeInterval);

                // Check if rows need to wrap around to the right
                for (int i = 0; i < floorsParent.childCount; i++)
                {
                    Transform row = floorsParent.GetChild(i);
                    if (row.position.x <= originalPositions[i].x - rowWidth)
                    {
                        row.position = new Vector3(originalPositions[i].x + rowWidth - moveDistance, row.position.y, row.position.z);
                    }
                }
            }
        }

        private IEnumerator MoveAllRows(Vector3 targetOffset)
        {
            float elapsedTime = 0f;
            Vector3[] startPositions = new Vector3[floorsParent.childCount];
            for (int i = 0; i < floorsParent.childCount; i++)
            {
                startPositions[i] = floorsParent.GetChild(i).position;
            }

            while (elapsedTime < moveDistance / moveSpeed)
            {
                for (int i = 0; i < floorsParent.childCount; i++)
                {
                    Transform row = floorsParent.GetChild(i);
                    row.position = Vector3.Lerp(startPositions[i], startPositions[i] + targetOffset, (elapsedTime * moveSpeed) / moveDistance);
                }
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            for (int i = 0; i < floorsParent.childCount; i++)
            {
                Transform row = floorsParent.GetChild(i);
                row.position = startPositions[i] + targetOffset;
            }
        }
    }
}
