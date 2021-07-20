using UnityEngine;
using GameFramework.GameStructure.Characters;

namespace GameFramework.Display.Placement.Components
{
    public abstract class MemsDriver : MonoBehaviour
    {
        public bool ApplyToPlayerOnly = true;
        public CharacterHolder GameItemHolder;

        protected float accelerometerUpdateInterval = 1.0f / 60.0f;
        protected float lowPassKernelWidthInSeconds = 1.0f;

        protected float lowPassFilterFactor;
        protected Vector3 lowPassValue = Vector3.zero;

        protected Rigidbody RigidbodyComp;

        private Animator animator;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            //Only apply to player?
            if (ApplyToPlayerOnly)
            {
                //Check whether is item represent player
                bool isPlayer = GameItemHolder.PlayerGameItem.IsPlayerCharacter();
                if (!isPlayer)
                {
                    //No, then disable this component
                    this.enabled = false;
                    return;
                }
            }
            if (RigidbodyComp == null)
            {
                RigidbodyComp = GetComponent<Rigidbody>();
            }
            lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
            lowPassValue = UnityEngine.Input.acceleration;
        }

        public Animator GetAnimator()
        {
            if(animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }
            return animator;
        }
    }
}