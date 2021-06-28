using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

namespace GameFramework.GameStructure
{
    /// <summary>
    /// An MonoBehaviour which will call LateStart() after the GameManager is ready
    /// </summary>
    /// The MonoBehaviour will call LateStart() after the GameManager is ready
    public abstract class GmStartDependBehaviour : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(GmDependStart());
        }

        IEnumerator GmDependStart()
        {
            //Wait for GameManager ready
            while (GameManager.Instance == null || !GameManager.Instance.IsInitialised)
            {
                yield return null;
            }
            //Call LateStart
            GmReadyStart();
        }

        protected abstract void GmReadyStart();
    }
}
