using UnityEngine;
using System.Collections;

public class RoomThree : MonoBehaviour
{
    [SerializeField] private Transform floorsParent; // Parent object containing all floors
    [SerializeField] private Color whiteColor = Color.white; // Color for white tiles
    [SerializeField] private Color blackColor = Color.black; // Color for black tiles
    [SerializeField] private float changeInterval = 0.5f; // Time interval between each row's color change

    private void Start()
    {
        if (floorsParent == null)
        {
            Debug.LogError("Floors parent is not assigned.");
            return;
        }

        InitializeRowColors();
        StartCoroutine(ChangeRowColors());
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

    private IEnumerator ChangeRowColors()
    {
        while (true)
        {
            for (int i = floorsParent.childCount - 1; i >= 0; i--)
            {
                Transform row = floorsParent.GetChild(i);
                Color currentColor = row.GetChild(0).GetComponent<Renderer>().material.color;
                Color newColor = (currentColor == whiteColor) ? blackColor : whiteColor;

                SetRowColor(row, newColor);

                yield return new WaitForSeconds(changeInterval);
            }
        }
    }

    private void SetRowColor(Transform row, Color color)
    {
        for (int j = 0; j < row.childCount; j++)
        {
            Renderer renderer = row.GetChild(j).GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = color;
            }
        }
    }
}
