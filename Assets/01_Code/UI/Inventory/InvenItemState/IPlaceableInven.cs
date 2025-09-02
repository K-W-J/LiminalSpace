namespace KWJ.Code.UI.Inventory
{
    public interface IPlaceableInven
    {
        public void OnLeftClick(InventoryItem item);
        public void OnRightClick(InventoryItem item);
    }
}