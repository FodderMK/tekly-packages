using Tekly.DataModels.Models;

namespace TeklySample.Game.Items
{
    public class ItemInventoryModel : ObjectModel
    {
        private readonly ItemInventory m_inventory;

        public ItemInventoryModel(ItemInventory inventory)
        {
            m_inventory = inventory;
        }

        protected override bool TryGetModel(string modelKey, out IModel model)
        {
            if (!base.TryGetModel(modelKey, out model)) {
                var inventoryItemModel = new InventoryItemModel(m_inventory.Get(modelKey));
                Add(modelKey, inventoryItemModel);
                model = inventoryItemModel;
            }

            return true;
        }
    }
    
    public class InventoryItemModel : ObjectModel, ITickable
    {
        private readonly InventoryItem m_item;

        [ObjectModelField("name")] private readonly StringValueModel m_name = new StringValueModel("");
        [ObjectModelField("icon")] private readonly SpriteValueModel m_icon =  new SpriteValueModel();
        [ObjectModelField("count")] private readonly NumberValueModel m_count = new NumberValueModel(0);
        
        public InventoryItemModel(InventoryItem item)
        {
            m_item = item;
            m_name.Value = m_item.NameId;
            m_icon.Value = item.Icon;
         
            AddSelf();
        }

        protected override void OnTick()
        {
            m_count.Value = m_item.Count;
        }
    }
}