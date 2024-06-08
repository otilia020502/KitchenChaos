using UnityEngine;
using System.Collections;

public class RoomThreeColorChanger : MonoBehaviour
{
    [SerializeField] private Transform floorsParent; // Parent object containing all floors
    [SerializeField] private Material whiteMaterial; // Material for white tiles
    [SerializeField] private Material blackMaterial; // Material for black tiles
    [SerializeField] private float changeInterval = 1f; // Time interval between each row's start change
    [SerializeField] private float transitionDuration = 0.5f; // Duration for the color transition

    private Material[] initialMaterials;

    private void Start()
    {
        if (whiteMaterial == null || blackMaterial == null)
        {
            Debug.LogError("Materials are not assigned.");
            return;
        }

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
        initialMaterials = new Material[floorsParent.childCount];

        for (int i = 0; i < floorsParent.childCount; i++)
        {
            Transform row = floorsParent.GetChild(i);
            Material initialMaterial = (i % 2 == 0) ? blackMaterial : whiteMaterial;

            initialMaterials[i] = initialMaterial;

            for (int j = 0; j < row.childCount; j++)
            {
                Renderer renderer = row.GetChild(j).GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.sharedMaterial = initialMaterial;
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
                Material currentMaterial = row.GetChild(0).GetComponent<Renderer>().sharedMaterial;
                Material newMaterial = (currentMaterial == whiteMaterial) ? blackMaterial : whiteMaterial;

                StartCoroutine(SmoothColorTransition(row, newMaterial, i));
                yield return new WaitForSeconds(changeInterval); // Delay between each row's color change to create a sweeping effect
            }
        }
    }

    private IEnumerator SmoothColorTransition(Transform row, Material newMaterial, int rowIndex)
    {
        float elapsedTime = 0f;
        int columnCount = row.childCount;
        Color[] startColors = new Color[columnCount];
        Color endColor = newMaterial.color;

        // Get the starting colors of the row
        for (int j = 0; j < columnCount; j++)
        {
            Renderer renderer = row.GetChild(j).GetComponent<Renderer>();
            if (renderer != null)
            {
                startColors[j] = renderer.sharedMaterial.color;
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
                    renderer.sharedMaterial.color = Color.Lerp(startColors[j], endColor, t);
                }
            }

            yield return null;
        }

        // Ensure the final color is set
        for (int j = 0; j < columnCount; j++)
        {
            Renderer renderer = row.GetChild(j).GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = newMaterial;
                renderer.sharedMaterial.color = endColor;
            }
        }

        // Update the initial material for this row
        initialMaterials[rowIndex] = newMaterial;
    }
}
