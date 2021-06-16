using UnityEngine;
using System.Collections;
using static GameFramework.GameStructure.GameItems.ObjectModel.GameItem;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using GameFramework.GameStructure.GameItems.Components;

namespace GameFramework.Display.Placement.Components
{

    /// <summary>
    /// Maintain a fixed distance from the specified transform
    /// </summary>
    [AddComponentMenu("Game Framework/Display/Placement/MoveToTransform")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/display/")]
    public class MoveToTransform : MonoBehaviour
    {
        /// <summary>
        /// A transform that this gameobject will follow.
        /// </summary>
        public Vector3 Target
        {
            get
            {
                return _target;
            }
            set
            {
                _target = value;
                isMoving = true;
            }
        }
        [SerializeField]
        [Tooltip("A transform that this gameobject will go to.")]
        private Vector3 _target;

        protected bool isMoving = false;

        /// <summary>
        /// The Speed with which to follow the Target.
        /// </summary>
        [Tooltip("The Speed of the gameobject.")]
        public float Speed = 1.2f;

        [Tooltip("The distance to target that gameobject stop move.")]
        public float StopDistance = 0.15f;

        [Tooltip("The rotation angel around the forwardDirection.")]
        public float Angel = 0;

        [Tooltip("The turn speed for gameobject.")]
        public float TurnSpeed = 5;

        public UnityEvent OnArrivedTarget = new UnityEvent();

        private Rigidbody rigidbodyComp;

        //Default direction
        private AxisDirection forwardDirection = AxisDirection.Z;

        private bool isPrefabLoaded = false;

        private void Awake()
        {
            transform.position = Target;
        }

        protected void Start()
        {
            rigidbodyComp = GetComponent<Rigidbody>();
        }

        protected void Update()
        {
            //TODO: Support Rigidbody
            if (rigidbodyComp == null)
            {
                UpdatePosition();
            }
        }

        private void FixedUpdate()
        {
            if (rigidbodyComp != null)
            {
                UpdatePosition();
            }
        }

        private void UpdatePosition()
        {
            if (transform.childCount > 0)
            {
                if (!isPrefabLoaded)
                {
                    isPrefabLoaded = true;
                    PositionModifier pm = GetComponentInChildren<PositionModifier>();
                    if (pm != null)
                    {
                        forwardDirection = pm.ForwardDirection;
                    }
                }
                Vector3 targetVector = Target - transform.position;
                if (targetVector.sqrMagnitude > StopDistance * 1.2 && !isMoving)
                {
                    isMoving = true;
                }
                else if (isMoving && targetVector.sqrMagnitude < StopDistance * 0.8)
                {
                    isMoving = false;
                    //Notify the listeners
                    OnArrivedTarget.Invoke();
                }
                if (isMoving)
                {
                    Move(targetVector);
                    LookAt(targetVector, Angel);
                }
            }
        }

        private void Move(Vector3 targetVector)
        {
            //Get the target vector
            Vector3 movement = targetVector;
            //Normalize the speed, then calculate the movement by delta time
            movement = movement.normalized * Speed * Time.deltaTime;
            if (rigidbodyComp == null)
            {
                transform.position = transform.position + movement;
            }
            else
            {
                rigidbodyComp.MovePosition(transform.position + movement);
            }
        }


        /// <summary>
        /// Turn the gameobject to face to the target
        /// </summary>
        /// <param name="targetVector">The target vector</param>
        /// <param name="rotateAngel">The rotate angel around the gameobject's forward direction</param>
        private void LookAt(Vector3 targetVector, float rotateAngel)
        {
            //TODO: Now we suppose Y axis is up-direction, may need to support change this in the future
            //Apply the rotation
            Quaternion newRotation = CalculateRotation(targetVector, rotateAngel);
            float gap = Quaternion.Angle(transform.rotation, newRotation);
            if (gap > 0.1)
            {
                //Constant turn speed
                //float t = rotateSpeed / gap * Time.deltaTime;
                //Rotate faster in the beginning
                float t = TurnSpeed * Time.deltaTime;
                newRotation = Quaternion.Lerp(transform.rotation, newRotation, t);
            }
            if (rigidbodyComp == null)
            {
                transform.rotation = newRotation;
            }
            else
            {
                rigidbodyComp.MoveRotation(newRotation);
            }
        }

        public Quaternion CalculateRotation(Vector3 targetVector, float rotateAngel)
        {
            Vector3 target = targetVector;
            //The actual Z
            Vector3 forward = targetVector;
            Vector3 up = Vector3.up;
            Vector3 right = Vector3.right;

            if (forwardDirection == AxisDirection.NegativeX)
            {
                //Forward (Z)
                if (target.x == 0 && target.z == 0)
                {
                    forward = Vector3.right;
                }
                else
                {
                    forward = Vector3.Cross(up, target);
                }
                //Up
                up = Vector3.Cross(target, forward);
                //Roate
                if (rotateAngel != 0)
                {
                    Quaternion rq = Quaternion.AngleAxis(rotateAngel, target);
                    forward = rq * forward;
                    up = rq * up;
                }

            }
            else if (forwardDirection == AxisDirection.X)
            {
                //Forward (Z)
                if (target.x == 0 && target.z == 0)
                {
                    forward = Vector3.right;
                }
                else
                {
                    forward = Vector3.Cross(target, up);
                }
                //Up
                up = Vector3.Cross(forward, target);
                //Roate
                if (rotateAngel != 0)
                {
                    Quaternion rq = Quaternion.AngleAxis(rotateAngel, target);
                    forward = rq * forward;
                    up = rq * up;
                }
            }
            else if (forwardDirection == AxisDirection.NegativeZ)
            {
                //Forward (Z)
                forward = -target;
                //Up
                if (target.x == 0 && target.z == 0)
                {
                    up = Vector3.back;
                }
                else
                {
                    right = Vector3.Cross(up, forward);
                    up = Vector3.Cross(forward, right);
                }
                //Roate
                if (rotateAngel != 0)
                {
                    Quaternion rq = Quaternion.AngleAxis(rotateAngel, target);
                    up = rq * up;
                }
            }
            else if (forwardDirection == AxisDirection.Z)
            {
                //Forward (Z)
                forward = target;
                //Up
                if (target.x == 0 && target.z == 0)
                {
                    up = Vector3.back;
                }
                else
                {
                    right = Vector3.Cross(up, forward);
                    up = Vector3.Cross(forward, right);
                }
                //Roate
                if (rotateAngel != 0)
                {
                    Quaternion rq = Quaternion.AngleAxis(rotateAngel, target);
                    up = rq * up;
                }
            }
            //Apply the rotation
            return Quaternion.LookRotation(forward, up);
        }

    }
}