using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Display.Placement.Components
{
    public abstract class MoveStrategy
    {
        public const string LINEAR_MOVE_STRATEGY = "Linear";
        public const string BEZIER_SIMPLE_STRATEGY = "BezierSimple";
        /// <summary>
        /// Move target
        /// </summary>
        protected Vector3 Target;

        /// <summary>
        /// The component to move
        /// </summary>
        protected MoveToPosition Component;

        public MoveStrategy(MoveToPosition component)
        {
            Component = component;
        }

        /// <summary>
        /// Handle the movement of the owner object
        /// </summary>
        public virtual void UpdateMovement()
        {
            UpdateSpeed();
        }

        /// <summary>
        /// Update the speed according to time and acceleration
        /// </summary>
        protected void UpdateSpeed()
        {
            //Enable acceleration?
            if (Component.EnableAcceleration)
            {
                //Yes, then calculate the ImSpeed.
                if (Component.ImSpeed < Component.Speed)
                {
                    Component.ImSpeed += Component.Acceleration * Time.deltaTime;
                    if (Component.ImSpeed > Component.Speed)
                    {
                        Component.ImSpeed = Component.Speed;
                    }
                } else
                {
                    //Current speed is the max speed
                    Component.ImSpeed = Component.Speed;
                }
            }
        }

        /// <summary>
        /// The the target of the movement
        /// </summary>
        /// <param name="target"></param>
        public virtual void SetTarget(Vector3 target)
        {
            Target = target;
        }

        /// <summary>
        /// Turn the gameobject to face to the target
        /// </summary>
        /// <param name="targetVector">The vector from this object to target</param>
        /// <param name="rotateAngel">The rotate angel around the gameobject's forward direction</param>
        protected void LookAt(Vector3 targetVector, float rotateAngel)
        {
            //TODO: Now we suppose Y axis is up-direction, may need to support change this in the future
            //Apply the rotation
            Quaternion newRotation = Component.CalculateRotation(targetVector.normalized, rotateAngel);
            float gap = Quaternion.Angle(Component.transform.rotation, newRotation);
            //Debug.Log(string.Format("gap: {8} rotation: {0}, {1}, {2}, {3} new: {4}, {5}, {6}, {7} \n v: {9}, {10}, {11}",
            //    Component.transform.rotation.w, Component.transform.rotation.x, Component.transform.rotation.y, Component.transform.rotation.z,
            //    newRotation.w, newRotation.x, newRotation.y, newRotation.z,
            //    gap, targetVector.x, targetVector.y, targetVector.z));
            //Debug.Log("gap: " + gap);
            if (gap > 0.1)
            {
                //Constant turn speed
                //float t = rotateSpeed / gap * Time.deltaTime;
                //Rotate faster in the beginning
                float t = Component.TurnSpeed * Time.deltaTime;
                newRotation = Quaternion.Lerp(Component.transform.rotation, newRotation, t);
            }
            if (Component.RigidbodyComp == null)
            {
                Component.transform.rotation = newRotation;
            }
            else
            {
                Component.RigidbodyComp.MoveRotation(newRotation);
            }
        }
    }

    /// <summary>
    /// Move to target linearly
    /// </summary>
    public class LinearMoveStrategy : MoveStrategy
    {
        public LinearMoveStrategy(MoveToPosition component) : base(component)
        {
        }

        public override void UpdateMovement()
        {
            base.UpdateMovement();
            Vector3 targetVector = Target - Component.transform.position;
            Move(targetVector);
            if (Component.LookAtForward)
            {
                LookAt(targetVector, Component.Angel);
            }
        }

        private void Move(Vector3 targetVector)
        {
            //Get the target vector
            Vector3 movement = targetVector;
            //Normalize the speed, then calculate the movement by delta time
            movement = movement.normalized * Component.ImSpeed * Time.deltaTime;

            if (movement.sqrMagnitude >= targetVector.sqrMagnitude)
            {
                //If the distance to go is bigger than the distance to target, means we arrived
                if (Component.RigidbodyComp == null)
                {
                    Component.transform.position = Component.Target;
                }
                else
                {
                    Component.RigidbodyComp.MovePosition(Component.transform.position + targetVector);
                }
                //Notify the component that we're arrived
                Component.OnArrived();
            }
            else
            {
                if (Component.RigidbodyComp == null)
                {
                    Component.transform.position = Component.transform.position + movement;
                }
                else
                {
                    Component.RigidbodyComp.MovePosition(Component.transform.position + movement);
                }
            }
        }
    }

    /// <summary>
    /// Move to target along with a serial of bezier control point
    /// </summary>
    public class MoveWithBezierStrategy : MoveStrategy
    {
        protected Vector3[] Points;
        protected Vector3[] calculationBuf;
        //Roughly estimation for the path length
        private float pathLength;
        //The length of the road we already moved
        private float movedLength;
        //The position where the object is in last update call
        private Vector3 lastPosition;

        private float t;
        private float lengthFactor;

        public MoveWithBezierStrategy(MoveToPosition component) : base(component)
        {
        }

        public override void UpdateMovement()
        {
            base.UpdateMovement();
            Vector3 targetDirection = Move();
            if (Component.LookAtForward)
            {
                LookAt(targetDirection, Component.Angel);
            }
        }


        public Vector3 Move()
        {
            //Calculate the distance this object moved after last update
            float lastStep = (Component.transform.position - lastPosition).magnitude;
            //The total length the object moved
            movedLength += lastStep;
            //Calculate t
            t = (float)(t + lastStep / (pathLength * lengthFactor));
            if (t > 0)
            {
                //factor correction
                lengthFactor = movedLength / t / pathLength;
            }

            //Calculate the move distance based on speed
            float nextMovementDist = Component.ImSpeed * Time.deltaTime;
            //TODO: In pause status, the deltaTime will be zero

            //Based on the speed, what next t we will use to calculate the Bezier curve
            float plan_t = (float)(t + nextMovementDist / (pathLength * lengthFactor));
            if (plan_t > 1)
            {
                plan_t = 1;
            }
            //Bezier calculation, get the move target for next update
            Vector3 nextTarget = GetBezierPoint(plan_t);
            Vector3 nextTargetDirection = nextTarget - Component.transform.position;
            //The distance from current position to next target
            float gap = nextTargetDirection.magnitude;
            if (gap > 0)
            {
                //The length factor correction
                lengthFactor /= (nextMovementDist / gap);
            }
            nextTargetDirection = nextTargetDirection.normalized * nextMovementDist;

            Vector3 distToEndPoint = Points[Points.Length - 1] - Component.transform.position;
            //Record last position before we apply the change
            lastPosition = Component.transform.position;

            if (nextTargetDirection.sqrMagnitude >= distToEndPoint.sqrMagnitude)
            {
                //If the distance to go is bigger than the distance to target, means we arrived
                if (Component.RigidbodyComp == null)
                {
                    Component.transform.position = Component.Target;
                }
                else
                {
                    Component.RigidbodyComp.MovePosition(Component.transform.position + nextTargetDirection);
                }
                //Notify the component that we're arrived
                Component.OnArrived();
            }
            else
            {
                if (Component.RigidbodyComp == null)
                {
                    Component.transform.position = Component.transform.position + nextTargetDirection;
                }
                else
                {
                    Component.RigidbodyComp.MovePosition(Component.transform.position + nextTargetDirection);
                }
            }
            //Debug.Log(string.Format("step: {0} t: {1} plan_t: {2}, gap: {13}, factor:{3}, position: {4}, {5}, {6} \n target: {7}, {8}, {9}, delta: {10}, {11}, {12} ",
            //    nextMovementDist, t, plan_t, lengthFactor, Component.transform.position.x,
            //    Component.transform.position.y, Component.transform.position.z, nextTarget.x,
            //    nextTarget.y, nextTarget.z, nextTargetDirection.x, nextTargetDirection.y, nextTargetDirection.z,
            //    gap));
            return nextTargetDirection;
        }

        public void SetBezierTargets(List<Vector3> points)
        {
            Points = new Vector3[points.Count+1];
            calculationBuf = new Vector3[points.Count + 1];
            Array.Copy(points.ToArray(), 0, Points, 1, points.Count);
            Points[0] = Component.transform.position;
            //The distance of the first point to current position
            pathLength = 0;
            for (int i = 1; i < Points.Length; i++)
            {
                pathLength += (Points[i] - Points[i - 1]).magnitude;
            }
            movedLength = 0;
            lastPosition = Component.transform.position;
            t = 0;
            lengthFactor = 1;

        }

        private Vector3 GetBezierPoint(float t)
        {
            Array.Copy(Points, 0, calculationBuf, 0, Points.Length);
            for (int i = 0; i < Points.Length - 1; i++)
            {
                for(int j = 0; j < Points.Length - 1 - i; j++)
                {
                    calculationBuf[j] = Vector3.Lerp(calculationBuf[j], calculationBuf[j+1], t);
                }
            }
            return calculationBuf[0];
        }
    }

    /// <summary>
    /// The simple version Bezier strategy will use a point on the forward direction
    /// as the control point to make the path smooth
    /// </summary>
    public class MoveBezierSimpleStrategy : MoveWithBezierStrategy
    {
        public MoveBezierSimpleStrategy(MoveToPosition component) : base(component)
        {
        }

        public override void SetTarget(Vector3 target)
        {
            base.SetTarget(target);
            Vector3 position = Component.transform.position;
            Vector3 forward = Component.GetForward();
            //Use a point on the forward direction as a control point
            position += forward.normalized * (target-position).magnitude/2;
            SetBezierTargets(new List<Vector3>() { position, target });

        }
    }
}