namespace Counters
{
    public class DeliveryCounter : BaseCounter
    {
        public override void Interact(Player player)
        {
            if (player.TryGetPlateObject(out PlatekitchenObject platekitchenObject))
            {
               platekitchenObject.DestorySelf();
            }
        }
    }
}
