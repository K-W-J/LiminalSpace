
namespace Code.Define
{
    public enum LevelType
    {
        None = -1,
        
        Factory,
        Tunnel,
        Mart,
        Shop,
        Mine,
        School,
        Mall,
        
        Max
    }
    
    public enum EnemeyType
    {
        None = -1,
    
        Miner,
        Clown,
        Snailman,
        Looker,
        Minder,
        MaskMan,
    
        Max
    }
    
    public enum ItemDragState
    {
        None,        // 기본 상태
        PickedUp,    // 들어올린 상태 (첫 클릭)
        Placing      // 놓는 중 (두 번째 클릭에서 포인터 뗄 때)
    }
    
    public enum ItemCategory
    {
        None = -1,
        
        Stone,
        Wood,
        Scrap,
        
        Max
    }
    
    public enum PanelType
    {
        None = -1,
        
        GameOver,
        Pause,
        Setting,
        Inventory,
        InventoryBar,
        
        Max
    }
}