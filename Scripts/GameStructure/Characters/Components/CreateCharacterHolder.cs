using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameFramework.GameStructure;
using GameFramework.GameStructure.Characters.ObjectModel;
using GameFramework.GameStructure.GameItems.Components.AbstractClasses;
using GameFramework.GameStructure.Util;
using UnityEngine;
using static GameFramework.GameStructure.GameItems.ObjectModel.GameItemContext;

namespace GameFramework.GameStructure.Characters
{
    /// <summary>
    /// Create the CharacterHolder
    /// </summary>
    /// Create the CharacterHolder from Player data, especially when a player has multiple characters,
    /// need this component to create the CharacterHolder in loop
    [AddComponentMenu("Game Framework/GameStructure/Characters/Create Character Holder")]
    public class CreateCharacterHolder : MonoBehaviour
    {
        public GameObject CharacterHolder;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(CreateCoroutine());
        }

        // Update is called once per frame
        void Update()
        {

        }

        private IEnumerator CreateCoroutine()
        {
            while (!GameManager.Instance.IsInitialised)
            {
                yield return Task.Yield();
            }
            CreateHolder();
        }

        public virtual async Task CreateHolder()
        {
            //Get the game items under current player
            var items = GameManager.Instance.Players.Selected.PlayerDto.OwnedItems;
            foreach (var item in items)
            {
                //If it's character
                if (item.GiType == typeof(Character).Name)
                {
                    Vector3 position = item.Props.GetVector3(Constants.PROP_KEY_POSITION).Value;
                    Vector3 rotation = item.Props.GetVector3(Constants.PROP_KEY_ROTATION).Value;
                    GameObject characterHolder = GameObject.Instantiate(CharacterHolder, position,
                       Quaternion.Euler(rotation.x, rotation.y, rotation.z));
                    CharacterHolder holderComponent = characterHolder.GetComponent<CharacterHolder>();
                    BindItem(holderComponent, item);
                }
            }
        }

        public virtual async Task BindItem(CharacterHolder holderComponent, PlayerGameItem item)
        {
            await holderComponent.BindCharacterPGI(item, item.PrefabType);
        }
    }
}