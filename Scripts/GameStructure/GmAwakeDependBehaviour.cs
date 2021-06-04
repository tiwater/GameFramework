using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

namespace GameFramework.GameStructure
{
    public abstract class GmAwakeDependBehaviour : MonoBehaviour
    {
        void Awake()
        {
            StartCoroutine(GmDependAwake());
        }

        IEnumerator GmDependAwake()
        {

            //Wait for the GameManager init process
            while (!GameManager.Instance.IsInitialised)
            {
                yield return Task.Yield();
            }
            GmReadyAwake();
        }

        protected abstract void GmReadyAwake();
    }
}