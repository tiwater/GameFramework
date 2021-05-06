//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//
// Please direct any bugs/comments/suggestions to http://www.flipwebapps.com
// 
// The copyright owner grants to the end user a non-exclusive, worldwide, and perpetual license to this Asset
// to integrate only as incorporated and embedded components of electronic games and interactive media and 
// distribute such electronic game and interactive media. End user may modify Assets. End user may otherwise 
// not reproduce, distribute, sublicense, rent, lease or lend the Assets. It is emphasized that the end 
// user shall not be entitled to distribute or transfer in any way (including, without, limitation by way of 
// sublicense) the Assets in any other way than as integrated components of electronic games and interactive media. 

// The above copyright notice and this permission notice must not be removed from any files.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//----------------------------------------------

using System.Threading.Tasks;
using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.GameStructure.Players.Messages;
using GameFramework.Service;
using UnityEngine;

namespace GameFramework.GameStructure.Players.ObjectModel
{
    /// <summary>
    /// For managing an array of players inlcuding selection, unlocking
    /// </summary>
    public class PlayerGameItemManager : GameItemManager<Player, GameItem>
    {

        public static string DUMMY_PREFS_KEY = "Overall_data";
        /// <summary>
        /// Called when the current selection changes. Override this in any base class to provide further handling such as sending out messaging.
        /// </summary>
        /// <param name="newSelection"></param>
        /// <param name="oldSelection"></param>
        /// You may want to override this in your derived classes to send custom messages.
        public override void OnSelectedChanged(Player newSelection, Player oldSelection)
        {
            GameManager.SafeQueueMessage(new PlayerChangedMessage(newSelection, oldSelection));
        }

        public override async Task LoadFromStorage()
        {
            //if (Items == null)
            //{
            //    Items = new List<Player>();
            //}
            //Load default play schema from local
            await Load(0, 0);

            var player = await PlayerGameItemService.Instance.GetPlayerInstance();

            if (player == null)
            {
                //Simulate the server side created instance from meta of Player_0
                //Selected.PlayerDto = Selected.GeneratePlayerDto();
                //Selected.PlayerDto.Id = GameManager.Instance.UserId;
                //string playerString = JsonUtility.ToJson(Selected.PlayerDto);
                //PlayerPrefs.SetString(DUMMY_PREFS_KEY + Selected.PlayerDto.Id, playerString);
                _isLoaded = true;
            }
            else
            {
                // Create the player meta.
                //var player = ScriptableObject.CreateInstance<Player>();
                //Items.Add(player);

                //Player is a special case, because other config keys are stored under player variables,
                //so we have to set the selected player first
                Selected.PlayerGameItem = player;

                //Assert.AreNotEqual(Items.Count, 0, "You need to create 1 or more items in GameItemManager.Load()");
                //Selected = player;

                //player.Initialise(GameConfiguration.Instance, null, GameManager.Messenger,
                //            playerDto.Id, LocalisableText.CreateLocalised(), LocalisableText.CreateLocalised(), valueToUnlock: -1);
                _isLoaded = true;
            }
        }
    }
}