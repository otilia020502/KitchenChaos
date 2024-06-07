using UnityEngine;
[CreateAssetMenu]
public class CuttingRecipeSO : ScriptableObject
{
    public KitchenObjectSo input;
    public KitchenObjectSo output;
    public int cuttingProgressMax;
}
