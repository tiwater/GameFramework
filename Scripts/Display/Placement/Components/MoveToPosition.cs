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
    /// Move the GameObject to a given position
    /// </summary>
    [AddComponentMenu("Game Framework/Display/Placement/MoveToPosition")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/display/")]
    public class MoveToPosition : MonoBehaviour
    {
        /// <summary>
        /// A position that this gameobject will move to. Assign a Vector3 to this
        /// property will replace current target.
        /// </summary>
        public Vector3 Target
        {
            get
            {
                return _target;
            }
            set
            {
                CancelMovement();
                AddTarget(value);
            }
        }
        [SerializeField]
        private Vector3 _target;

        private List<Vector3> targets = new List<Vector3>();

        protected MoveStrategy Strategy;
        protected Dictionary<string, MoveStrategy> strategies = new Dictionary<string, MoveStrategy>();

        protected bool isMoving = false;

        /// <summary>
        /// The Speed to move.
        /// </summary>
        [Tooltip("The Speed of the gameobject.")]
        public float Speed = 1.2f;

        [Tooltip("Shall we apply Acceleration to speed when start move.")]
        public bool EnableAcceleration = true;

        [Tooltip("The Acceleration of the gameobject.")]
        public float Acceleration = 0.5f;

        [Tooltip("Whether the speed will change randomly.")]
        public bool RandomSpeed = false;

        public float SpeedChangeInterval = 5;

        [Tooltip("The rotation angel around the forwardDirection.")]
        public float Angel = 0;

        [Tooltip("The turn speed for gameobject.")]
        public float TurnSpeed = 2;

        public Rigidbody RigidbodyComp;

        public UnityEvent OnArrivedTarget = new UnityEvent();


        [Tooltip("Whether the object should look at the forward direction when moving.")]
        public bool LookAtForward = true;

        private float speedResetTime = 0;

        /// <summary>
        /// Current Speed of the object
        /// </summary>
        public float ImSpeed {
            get
            {
                return _imSpeed;
            }
            set
            {
                _imSpeed = value;
            }
        }

        private float _imSpeed;

        //Default direction
        private AxisDirection forwardDirection = AxisDirection.Z;

        private bool isPrefabLoaded = false;

        private void Awake()
        {
            transform.position = Target;
            //Defautl is linear
            SetStrategy(MoveStrategy.BEZIER_SIMPLE_STRATEGY);
            //SetStrategy(MoveStrategy.LINEAR_MOVE_STRATEGY);
        }

        protected void Start()
        {
            if (RigidbodyComp == null)
            {
                RigidbodyComp = GetComponent<Rigidbody>();
            }
        }

        protected void Update()
        {
            if (RigidbodyComp == null)
            {
                UpdatePosition();
            }
        }

        private void FixedUpdate()
        {
            if (RigidbodyComp != null)
            {
                UpdatePosition();
            }
        }

        /// <summary>
        /// Update the object's position according to speed and strategy
        /// </summary>
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
                if (isMoving)
                {
                    //Should the speed change randomly
                    if (RandomSpeed)
                    {
                        speedResetTime += Time.deltaTime;
                        if (speedResetTime > SpeedChangeInterval)
                        {
                            speedResetTime = 0;
                            //Pick a random speed
                            ImSpeed = UnityEngine.Random.Range(Speed * 0.2f, Speed);
                        }
                    }
                    Strategy.UpdateMovement();
                }
            }
        }

        /// <summary>
        /// Add a target position to the target list of this object
        /// The object will move to the targets one by one
        /// </summary>
        /// <param name="target"></param>
        public void AddTarget(Vector3 target)
        {
            targets.Add(target);
            if (!isMoving)
            {
                NextTarget();
            }
        }

        /// <summary>
        /// Start to move to next target if any
        /// </summary>
        /// <returns></returns>
        public Vector3 NextTarget()
        {
            if (targets.Count > 0)
            {
                isMoving = true;
                //Move the target from the list to the Target property
                _target = targets[0];
                targets.RemoveAt(0);
                if (Strategy != null)
                {
                    Strategy.SetTarget(Target);
                }
            }
            return Target;
        }

        /// <summary>
        /// Cancel current movement
        /// </summary>
        public void CancelMovement()
        {
            if (isMoving)
            {
                Target = transform.position;
                targets.Clear();
                isMoving = false;
                ImSpeed = 0;
            }
        }

        /// <summary>
        /// Called by the MoveStrategy to notify the object already arrived the target
        /// </summary>
        public void OnArrived()
        {
            //Do we have targets remained?
            if (targets.Count > 0)
            {
                //Yes, then move to next target
                NextTarget();
            }
            else
            {
                isMoving = false;
                ImSpeed = 0;
                //Notify the listeners
                OnArrivedTarget.Invoke();
            }
        }

        /// <summary>
        /// Get the forward direction for the prefab
        /// </summary>
        /// <returns></returns>
        public Vector3 GetForward()
        {
            if (forwardDirection == AxisDirection.NegativeX)
            {
                return Vector3.Cross(transform.forward, transform.up);
            }
            else if (forwardDirection == AxisDirection.X)
            {
                return Vector3.Cross(transform.up, transform.forward);
            }
            else if (forwardDirection == AxisDirection.NegativeZ)
            {
                return -transform.forward;
            }
            else if (forwardDirection == AxisDirection.Z)
            {
                return transform.forward;
            }
            return transform.forward;
        }

        /// <summary>
        /// Calculate the rotation so the object can look at the target and stand up
        /// </summary>
        /// <param name="targetVector"></param>
        /// <param name="rotateAngel"></param>
        /// <returns></returns>
        public Quaternion CalculateRotation(Vector3 targetVector, float rotateAngel = 0)
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
                //Rotate
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
                //Rotate
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
                //Rotate
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
                //Rotate
                if (rotateAngel != 0)
                {
                    Quaternion rq = Quaternion.AngleAxis(rotateAngel, target);
                    up = rq * up;
                }
            }
            //Apply the rotation
            return Quaternion.LookRotation(forward, up);
        }

        public virtual void SetStrategy(string strategyType)
        {

            if (strategies.ContainsKey(strategyType))
            {
                Strategy = strategies[strategyType];
                return;
            }
            else
            {
                if (strategyType.Equals(MoveStrategy.LINEAR_MOVE_STRATEGY))
                {
                    Strategy = new LinearMoveStrategy(this);
                } else if (strategyType.Equals(MoveStrategy.BEZIER_SIMPLE_STRATEGY))
                {
                    Strategy = new MoveBezierSimpleStrategy(this);
                }
                strategies.Add(strategyType, Strategy);
            }
        }
    }
}