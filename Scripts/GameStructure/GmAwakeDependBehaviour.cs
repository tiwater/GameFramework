using UnityEngine;
using System.Collections;
using GameFramework.GameStructure;
using System.Threading.Tasks;

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
