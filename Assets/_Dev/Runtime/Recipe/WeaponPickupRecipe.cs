using NeoFPS.ModularFirearms;
using NeoFPS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NeoFPS.SinglePlayer;
using System.Reflection;

namespace Playground
{
    [CreateAssetMenu(fileName = "Weapon Pickup Recipe", menuName = "Playground/Weapon Pickup Recipe")]
    public class WeaponPickupRecipe : Recipe<InteractivePickup>
    {
        [SerializeField, Tooltip("The Ammo recipe for this weapon. When the weapon is built the player should get this recipe too.")]
        private AmmoPickupRecipe ammoRecipe;

        private FpsInventorySwappable _inventory;
        private FpsInventorySwappable inventory
        {
            get
            {
                if (_inventory == null)
                {
                    _inventory = FpsSoloCharacter.localPlayerCharacter.inventory as FpsInventorySwappable;
                }
                return _inventory;
            }
        }

        public override bool ShouldBuild
        {
            get
            {
                IInventoryItem[] ownedItems = inventory.GetItems();
                for (int i = 0; i < ownedItems.Length; i++)
                {
                    if (ownedItems[i].itemIdentifier == pickup.GetItem().itemIdentifier)
                    {
                        return false;
                    }
                }

                Debug.LogError("Need to ensure that we are also providing the ammo recipe for earned weapon.");
                return true;
            }
        }
    }

    public static class InteractivePickupExtension
    {
        public static FpsInventoryItemBase GetItem(this InteractivePickup instance)
        {
            var fieldInfo = typeof(InteractivePickup).GetField("m_Item", BindingFlags.NonPublic | BindingFlags.Instance);
            return fieldInfo.GetValue(instance) as FpsInventoryItemBase;
        }
    }
}
