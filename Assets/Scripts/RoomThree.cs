using UnityEngine;
using System.Collections;

public class RoomThree : MonoBehaviour
{
    [SerializeField] private Transform floorsParent; // Parent object containing all floors
    [SerializeField] private Color whiteColor = Color.white; // Color for white tiles
    [SerializeField] private Color blackColor = Color.black; // Color for black tiles
    [SerializeField] private float changeInterval = 1f; // Time interval between each row's start change
    [SerializeField] private float transitionDuration = 0.5f; // Duration for the color transition

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
            for (int i = 0; i < floorsParent.childCount; i++)
            {
                Transform row = floorsParent.GetChild(i);
                Color currentColor = row.GetChild(0).GetComponent<Renderer>().material.color;
                Color newColor = (currentColor == whiteColor) ? blackColor : whiteColor;

                StartCoroutine(SmoothColorTransition(row, newColor));
                yield return new WaitForSeconds(changeInterval); // Delay between each row's color change to create a sweeping effect
            }
        }
    }

    private IEnumerator SmoothColorTransition(Transform row, Color newColor)
    {
        float elapsedTime = 0f;
        int columnCount = row.childCount;
        Color[] startColors = new Color[columnCount];

        // Get the starting colors of the row
        for (int j = 0; j < columnCount; j++)
        {
            Renderer renderer = row.GetChild(j).GetComponent<Renderer>();
            if (renderer != null)
            {
                startColors[j] = renderer.material.color;
            }
        }

        // Smoothly transition to the new color
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);

            for (int j = 0; j < columnCount; j++)
            {
                Renderer renderer = row.GetChild(j).GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = Color.Lerp(startColors[j], newColor, t);
                }
            }

            yield return null;
        }
    }
}
