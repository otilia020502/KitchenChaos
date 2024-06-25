using System;

namespace Counters
{
    public class DeliveryCounter : BaseCounter
    {
        public static DeliveryCounter Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public override void Interact(Player player)
        {
            if (player.TryGetPlateObject(out PlatekitchenObject platekitchenObject))
            {
               DeliveryManager.Instance.DeliverRecipe(platekitchenObject);
               
               KitchenObject.DestroyKitchenObject(player.GetKitchenObject());
            }
        }
    }
}
