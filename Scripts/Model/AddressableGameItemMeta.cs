using System.Collections;
using System.Collections.Generic;
using GameFramework.GameStructure.AddressableGameItems.ObjectModel;
using static GameFramework.GameStructure.PlayerGameItems.ObjectModel.GameItemEquipment;

namespace GameFramework.GameStructure.Model
{
    public class AddressableGameItemMeta : GameItemMeta
    {
        public enum ContentType { All, Entity, Wearables, Weapon, Accessories, Skin, Scene, Level, World }

        /// <summary>
        /// Id of the GameItem
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The Id array of the apps the GameItem supports
        /// </summary>
        public string[] AppIds { get; set; }

        /// <summary>
        /// The thumbnail of the GameItem to display in eshop
        /// </summary>
        public string ThumbnailUrl { get; set; }

        /// <summary>
        /// Package of the GameItem, it should be the same as the addressable group name
        /// </summary>
        public string Package { get; set; }

        /// <summary>
        /// Type of the GameItem
        /// </summary>
        public ContentType Type { get; set; }

        /// <summary>
        /// For the wearable items, the equipment slot it supports
        /// </summary>
        public List<Slot> Slots { get; set; }

        /// <summary>
        /// Whether the GameItem is consumable
        /// </summary>
        public bool Consumable { get; set; }

        /// <summary>
        /// The resources this addressable GameItem constains.
        /// </summary>
        public List<ResourceInfo> Resources { get; set; }

        /// <summary>
        /// The GameItem this item supports.
        /// </summary>
        public List<SupportItemInfo> SupportItems { get; set; }
    }
}