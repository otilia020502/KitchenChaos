using UnityEngine;
[CreateAssetMenu]
public class FryingRecipeSO : ScriptableObject
{
    public KitchenObjectSo input;
    public KitchenObjectSo output;
    public float fryingTimerMax;
}
