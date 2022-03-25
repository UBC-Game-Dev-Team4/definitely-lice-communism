using ItemInventory;

namespace LevelOne
{
    /// <summary>
    ///     Plate is a collectable item that can break.
    /// </summary>
    public class PlateInteractable : CollectableItem
    {
        public override void Interact(object src, params object[] args)
        {
            if (_pickedUp) return;
            base.Interact(src, args);
            LevelOneInfoStorer.CastedSingleton.CastedInfo.wasPlatePickedUp = true;
        }
    }
}