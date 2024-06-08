using UnityEngine;

public class RoomFloorManager : MonoBehaviour
{
    [SerializeField] private Transform floorsParent; // Parent object containing all floors
    [SerializeField] private Material whiteMaterial; // Material for white tiles
    [SerializeField] private Material blackMaterial; // Material for black tiles

    private void Start()
    {
        ApplyChessboardPattern();
    }

    private void ApplyChessboardPattern()
    {
        if (floorsParent == null)
        {
            Debug.LogError("Floors parent is not assigned.");
            return;
        }

        int rowCount = floorsParent.childCount;
        for (int i = 0; i < 10; i++)
        {
            Transform row = floorsParent.GetChild(i);
            int columnCount = row.childCount;
            for (int j = 0; j < columnCount; j++)
            {
                Renderer renderer = row.GetChild(j).GetComponent<Renderer>();
                if (renderer != null)
                {
                    if ((i + j) % 2 == 0)
                    {
                        renderer.sharedMaterial = whiteMaterial;
                    }
                    else
                    {
                        renderer.sharedMaterial = blackMaterial;
                    }
                    Debug.Log($"Setting material for tile at ({i}, {j}) to {(renderer.sharedMaterial == whiteMaterial ? "white" : "black")}");
                }
                else
                {
                    Debug.LogWarning($"Renderer not found for tile at ({i}, {j})");
                }
            }
        }
    }
}