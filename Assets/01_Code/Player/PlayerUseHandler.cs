using UnityEngine;
using KWJ.Code.Entities;
using KWJ.Code.UI.Inventory;

namespace KWJ.Code.Players
{
    public class PlayerUseHandler : MonoBehaviour, IEntityComponent
    {
        public bool IsUsing { get; private set; }
        
        [SerializeField] private Transform _handPoint;
        
        [SerializeField] private Transform _dropPos;

        private Player _agent;
        private PlayerInventoryBar _inventoryBar;
        
        private Transform _item;
        private bool _isFirstClicking;
        
        public void Initialize(Entity entity)
        {
            _agent = entity as Player;
            
            _inventoryBar = entity.GetCompo<PlayerInventoryBar>();
            
        }

        /*private void OnEnable()
        {
            _agent.PlayerInputSo.OnDropItemAction += OnDropItem;
            _agent.PlayerInputSo.OnUseAction += OnUse;
            _agent.PlayerInputSo.OnSecondaryUseAction += OnSecondaryUse;
            _agent.PlayerInputSo.OnGunLoadAction += OnGunLoad;
        }

        private void OnDisable()
        {
            _agent.PlayerInputSo.OnDropItemAction -= OnDropItem;
            _agent.PlayerInputSo.OnUseAction -= OnUse;
            _agent.PlayerInputSo.OnSecondaryUseAction -= OnSecondaryUse;
            _agent.PlayerInputSo.OnGunLoadAction -= OnGunLoad;
        }*/
        
        
        /*
        public void PickUpItem(PickUpable pickRandom)
        {
            pickRandom.transform.SetParent(_handPoint);
            pickRandom.transform.position = _handPoint.position;
            pickRandom.transform.rotation = _handPoint.rotation;
            _item = pickRandom.transform;
        }

        public void OnDropItem()
        {
            if(_item == null) return;
            
            if (_item.transform.parent != _handPoint && _item.transform.parent != _agent.AimingPoint)
            {
                _item.transform.parent.SetParent(null);
            }
            else
            {
                _item.transform.SetParent(null);
            }
            
            _item = null;
            _inventory.TakeOutInventory(_dropPos.position);
        }
        private void OnSecondaryUse(bool obj)
        {
            if(_inventory.GetInventorySlotObject() is not Pistol) return;
            
            IsUsing = obj;
            
            if (obj && _inventory.IsSlotSelected() && !_inventory.IsSelectedSlotEmpty())
            {
                if (_inventory.GetInventorySlotObject().gameObject
                    .TryGetComponent<IItem>(out var item))
                {
                    item.SecondaryUseItem();
                }
            }
            else if (!obj && _inventory.IsSlotSelected() && !_inventory.IsSelectedSlotEmpty())
            {
                if (_inventory.GetInventorySlotObject().gameObject
                    .TryGetComponent<IItem>(out var item))
                {
                    item.SecondaryUseItem();
                }
            }
        }

        private void OnUse(bool obj)
        {
            if (obj && _inventory.IsSlotSelected() && !_inventory.IsSelectedSlotEmpty())
            {
                if (_inventory.GetInventorySlotObject().gameObject
                    .TryGetComponent<IItem>(out var item))
                {
                    if (item.IsDisposable == false)
                        item.UseItem();
                    else
                    {
                        if (_inventory.GetInventorySlotObject() is HealthKit healthKit)
                        {
                            if (_agent.EntityHealth.IsCurrentHealthMax == false)
                            {
                                SoundManager.Instance.PlaySFX("OpenBox", transform);
                                
                                item.UseItem();
                                _inventory.ResetSlotItem();
                            }
                        }
                    }
                }
            }
        }
        
        private void OnGunLoad()
        {
            if(_inventory.GetInventorySlotObject() is not Pistol) return;
            
            if (_inventory.IsSlotSelected() && !_inventory.IsSelectedSlotEmpty())
            {
                if (_inventory.GetInventorySlotObject() is Pistol pistol)
                {
                    if (!pistol.IsAmmoFull() && _ammo.AmmoCount > 0)
                    {
                        SoundManager.Instance.PlaySFX("GunRifle", transform);
                        _ammo.AmmoCount = pistol.AddAmmo(_ammo.AmmoCount);
                    }
                }
            }
        }*/
    }
}