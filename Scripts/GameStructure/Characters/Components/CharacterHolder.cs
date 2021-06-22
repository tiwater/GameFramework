using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameFramework.GameStructure.Characters.ObjectModel;
using GameFramework.GameStructure.GameItems.ObjectModel;
using UnityEngine;
using static GameFramework.GameStructure.GameItems.ObjectModel.GameItem;
using static GameFramework.GameStructure.GameItems.ObjectModel.GameItemContext;
using GameFramework.GameStructure.AddressableGameItems.ObjectModel;
using GameFramework.GameStructure.PlayerGameItems.ObjectModel;
using System;

namespace GameFramework.GameStructure.Characters
{
    /// <summary>
    /// Hold the Character GameItem, do the equipment and then display it
    /// </summary>
    /// Hold the Character GameItem, mount the equiment, textures, etc. And display it when the assets are ready
    [AddComponentMenu("Game Framework/GameStructure/Characters/Character Holder")]
    public class CharacterHolder : GameItemInstanceHolder<Character>
    {
        /// <summary>
        /// The equipped GameItem information.
        /// </summary>
        [Tooltip("The equipped GameItem information.")]
        public List<EquipmentInfo> EquipmentInfos;

        private bool Updated = false;

        /// <summary>
        /// Returns the current Character GameItem
        /// </summary>
        /// <returns></returns>
        protected override GameItemManager<Character, GameItem> GetGameItemManager()
        {
            return GameManager.Instance.Characters;
        }

        public override async Task RunMethod(bool isStart)
        {
            if (Context.ContextMode != ContextModeType.FromLoop)
            {
                //Loop mode will be set by the creator
                await base.RunMethod(isStart);
                if (!Updated)
                {
                    EquipAssets();
                }
            }
        }

        /// <summary>
        /// Bind a Character PlayerGameItem to this component, the prefab of of the Character will be displayed,
        /// and the equipment will be equipped
        /// </summary>
        /// <param name="gameItem"></param>
        /// <param name="localisablePrefabType"></param>
        public virtual async Task BindCharacterPGI(PlayerGameItem gameItem, LocalisablePrefabType localisablePrefabType = LocalisablePrefabType.InGame)
        {
            if (gameItem == null || gameItem.GiType != typeof(Character).Name)
            {
                Debug.LogError("Only Character PlayerGameItem is allowed!");
                return;
            }
            PlayerGameItem = gameItem;

            await AttachCharacterPrefabById(gameItem.GiId, localisablePrefabType);


            var equipments = gameItem.Equipments;//new List<CharacterEquipment>(GameManager.Instance.Players.Selected.PlayerDto.CharacterEquipments[gameItem.Id]);
            if (equipments != null)
            {
                PopulateCharacterEquipments(equipments);
                Updated = false;
                if (!Updated)
                {
                    //If the equipment is already displayed, we need to manually trigger the equipment process
                    //Otherwise it will be called automatically in RunMethod
                    await EquipAssets();
                }
            }
        }

        /// <summary>
        /// Instantiate the specified prefab for this GameObject, destroy the origin prefabs if any,
        /// so the new equipment can be equipped
        /// </summary>
        /// <param name="GiId"></param>
        /// <param name="contextModeType"></param>
        /// <param name="localisablePrefabType"></param>
        public async Task AttachCharacterPrefabById(string GiId, LocalisablePrefabType localisablePrefabType)
        {

            Context.ContextMode = ContextModeType.ByNumber;
            Context.Number = GiId;
            PrefabType = localisablePrefabType;

            //Clear the origin assets for new equipment
            _selectedPrefabInstance = null;
            //Detach the children
            transform.DetachChildren();
            foreach (var instance in _cachedPrefabInstances.Values)
            {
                GameObject.Destroy(instance);
            }
            _cachedPrefabInstances.Clear();
            await RunMethod(true);
        }

        /// <summary>
        /// Convert the equipments in the PlayerGameItem to the format this component can understand
        /// </summary>
        /// <param name="equipments"></param>
        private void PopulateCharacterEquipments(List<PlayerGameItem> equipments)
        {
            List<EquipmentInfo> equipmentInfos = new List<EquipmentInfo>();
            foreach (var equipment in equipments)
            {
                EquipmentInfo equipmentInfo = new EquipmentInfo();
                Enum.TryParse<Slot>(equipment.GetAttr(PlayerGameItem.ATTRS_SLOTTED), true, out equipmentInfo.slot);
                equipmentInfo.equipment = GameManager.Instance.GetIBaseGameItemManager(equipment.GiType).BaseGetItem(equipment.GiId);
                equipmentInfo.PrefabType = equipment.PrefabType;
                equipmentInfos.Add(equipmentInfo);
            }
            EquipmentInfos = equipmentInfos;
        }

        /// <summary>
        /// Convert the equipments in the PlayerDto to the format this component can understand
        /// </summary>
        /// <param name="equipments"></param>
        //private void PopulateCharacterEquipments(List<GameItemEquipment> equipments)
        //{
        //    List<EquipmentInfo> equipmentInfos = new List<EquipmentInfo>();
        //    foreach (var equipment in equipments)
        //    {
        //        EquipmentInfo equipmentInfo = new EquipmentInfo();
        //        equipmentInfo.slot = equipment.EquipSlot;
        //        var gameItem = GameManager.Instance.PlayerGameItems.GetPlayerGameItemById(equipment.GameItemId);
        //        equipmentInfo.equipment = GameManager.Instance.GetIBaseGameItemManager(gameItem.GiType).BaseGetItem(gameItem.GiId);
        //        equipmentInfos.Add(equipmentInfo);
        //    }
        //    EquipmentInfos = equipmentInfos;
        //}

        /// <summary>
        /// Wear the equipable assets, put the resources to proper places in the GameObject tree
        /// </summary>
        /// <returns></returns>
        private async Task EquipAssets()
        {
            if (EquipmentInfos != null)
            {
                Updated = true;
                if (_selectedPrefabInstance != null)
                {
                    _selectedPrefabInstance.hideFlags = HideFlags.HideAndDontSave;
                }
                List<Task> tasks = new List<Task>();
                foreach (var equipmentInfo in EquipmentInfos)
                {
                    if (equipmentInfo.equipment.GetType() == typeof(AddressableGameItem))
                    {
                        AddressableGameItem agi = (AddressableGameItem)equipmentInfo.equipment;
                        agi.Apply(gameObject, equipmentInfo.slot, equipmentInfo.PrefabType);
                        //if (string.IsNullOrEmpty(agi.AddressableName) && string.IsNullOrEmpty(agi.AddressableLabel))
                        //{
                        //    //TODO: Should provide full info for the assets
                        //    //If not supplied the Addressable info, fallback to Package
                        //    tasks.Add(agi.Apply(gameObject, equipmentInfo.slot, null, agi.Package, true));
                        //}
                        //else
                        //{
                        //    tasks.Add(agi.Apply(gameObject, equipmentInfo.slot, new List<string> { agi.AddressableName }, agi.AddressableLabel, true));
                        //}
                    }
                }
                //await Task.WhenAll(tasks.ToArray());

                if (_selectedPrefabInstance != null)
                {
                    _selectedPrefabInstance.hideFlags = HideFlags.None;
                }
            }
        }
    }
}