using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

namespace GameFramework.GameStructure
{
    /// <summary>
    /// An MonoBehaviour which will call LateStart() after the GameManager is ready
    /// </summary>
    /// The MonoBehaviour will call LateStart() after the GameManager is ready
    public abstract class GMDependBehaviour : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(GmDependStart());
        }

        IEnumerator GmDependStart()
        {
            //Wait for GameManager ready
            while (!GameManager.Instance.IsInitialised)
            {
                yield return Task.Yield();
            }
            //Call LateStart
            LateStart();
        }

        protected abstract void LateStart();
    }
}
