using UnityEngine;
[CreateAssetMenu]
public class BurningRecipeSO : ScriptableObject
{
    public KitchenObjectSo input;
    public KitchenObjectSo output;
    public float burningTimerMax;
}
