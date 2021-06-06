using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameFramework.GameStructure;
using GameFramework.GameStructure.GameItems.Components.AbstractClasses;
using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.Service;
using UnityEngine;

namespace GameFramework.GameStructure.GameItems.Components
{
    /// <summary>
    /// This component can support playing an (addressable) audio clip for a GameItem
    /// </summary>
    [RequireComponent(typeof(GameItemInstanceHolder<GameItem>))]
    [AddComponentMenu("Game Framework/GameStructure/GameItem/PlayAudioClip")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/game-structure/")]
    public class PlayAudioClip : MonoBehaviour
    {
        private AudioSource audioSource;
        private GameItemContextBase giHolder;
        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            giHolder = GetComponent<GameItemContextBase>();
        }

        /// <summary>
        /// Play a given audio clip on this GameObject
        /// </summary>
        /// <param name="clip"></param>
        public void Play(AudioClip clip)
        {
            if (audioSource != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
            else if (GameManager.Instance != null)
            {
                //If no audio source on the GameObjec, then use the global audio source
                GameManager.Instance.PlayEffect(clip);
            }
            return;
        }

        /// <summary>
        /// Play an audio clip in the GameItem asset
        /// </summary>
        /// <param name="clipName"></param>
        public void PlayBoundAddressableAudioClip(string clipName)
        {
            AudioClip audioClip = giHolder.GameItem.GetLocalisableAsset<AudioClip>(GameItem.LocalisableAssetType.Custom,
                "Touched");
            Play(audioClip);
        }

        /// <summary>
        /// Play an audio clip in the global addressable assets
        /// </summary>
        /// 
        /// <param name="clipName">The resource name of the clip</param>
        /// <param name="label">The addressable label of the clip</param>
        public async Task PlayGlobalAddressableAudioClip(string clipName, string label = "Audio")
        {
            AudioClip clip = await AddressableResService.GetInstance().LoadResourceAsync<AudioClip>(clipName, label);
            Play(clip);
        }
    }
}