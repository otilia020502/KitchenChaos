namespace Counters
{
    public class DeliveryCounter : BaseCounter
    {
        public override void Interact(Player player)
        {
            if (player.TryGetPlateObject(out PlatekitchenObject platekitchenObject))
            {
               DeliveryManager.Instance.DeliverRecipe(platekitchenObject);
               
               platekitchenObject.DestorySelf();
            }
        }
    }
}
