using System.Collections;
using System.Collections.Generic;
using GameFramework.GameStructure.Game.ObjectModel;
using static GameFramework.GameStructure.Model.GameItemEquipment;

namespace GameFramework.GameStructure.Model
{
    public class AddressableGameItemMeta
    {
        public enum ContentType { All, Entity, Wearables, Weapon, Accessories, Scene, Level, World }

        /// <summary>
        /// Id of the GameItem
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The Id array of the apps the GameItem supports
        /// </summary>
        public string[] AppIds { get; set; }

        //TODO: I18N
        /// <summary>
        /// The name of the GameItem
        /// </summary>
        public string Name { get; set; }

        //TODO: I18N
        /// <summary>
        /// The name of the GameItem
        /// </summary>
        public string Description { get; set; }

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
        /// For the wearable items, the equipment slot which support
        /// </summary>
        public List<Slot> Slots { get; set; }

        /// <summary>
        /// Whether the GameItem is consumable
        /// </summary>
        public bool Consumable { get; set; }

        /// <summary>
        /// The addressable resources the character constains. Store in <name , labels> format.
        /// The combination of name and labels should be able to locate the unique resource.
        /// </summary>
        public Dictionary<string, string> Resources { get; set; }

        /// <summary>
        /// The GameItem this item supports. Store in <GameItemType, GameItemId> format, e.g. <Character, "Fish1">
        /// <GameItemType, null> means support all GameItems under the given type.
        /// </summary>
        public Dictionary<GameConfiguration.GameItemType, string> SupportItems { get; set; }

        /// <summary>
        /// The equipment slot this item supports if it is equipable.
        /// </summary>
        public List<Slot> SupportSlots { get; set; }
    }
}