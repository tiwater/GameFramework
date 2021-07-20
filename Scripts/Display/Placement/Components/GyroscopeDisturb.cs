using System.Collections;
using System.Collections.Generic;
using GameFramework.Display.Placement.Components;
using UnityEngine;

namespace GameFramework.Display.Placement.Components
{
    public class MemsAction : MonoBehaviour
    {
        public float Speed = 20;

        private int index = 0;

        public float PushForce = 1;
        public float VibrativeFreq = 8;
        public float ShakeVolve = 0.00002f;

        public CameraFollow CameraFollowComp;
        public MoveToPosition MoveToPositionComp;

        private float t = 0;
        private Vector3 vibrativeForce = Vector3.zero;
        private Vector3 lastVibrativeForce = Vector3.zero;

        float accelerometerUpdateInterval = 1.0f / 60.0f;
        float lowPassKernelWidthInSeconds = 1.0f;

        private float lowPassFilterFactor;
        private Vector3 lowPassValue = Vector3.zero;

        private bool supportGyroscope;
        Gyroscope gyroscope;

        Rigidbody RigidbodyComp;

        // Start is called before the first frame update
        void Start()
        {
            if (RigidbodyComp == null)
            {
                RigidbodyComp = GetComponent<Rigidbody>();
            }
            if (CameraFollowComp == null)
            {
                //Try to get the canvas for background by ConfigBackgroundCamera
                var cameraComps = Resources.FindObjectsOfTypeAll(typeof(CameraFollow));
                foreach (var comp in cameraComps)
                {
                    //Suppose we only have one
                    CameraFollowComp = (CameraFollow)comp;
                    break;
                }
            }
            if (MoveToPositionComp == null)
            {
                MoveToPositionComp = GetComponent<MoveToPosition>();
            }
            lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
            lowPassValue = UnityEngine.Input.acceleration;
            InitGyroscope();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            AccelerationDriveMove();
            CheckGyroscope();

        }

        Vector3 LowPassFilterAccelerometer(Vector3 prevValue)
        {
            Vector3 newValue = Vector3.Lerp(prevValue, UnityEngine.Input.acceleration, lowPassFilterFactor);
            return newValue;
        }

        private void AccelerationDriveMove()
        {
            var accForce = lowPassValue;
            lowPassValue = LowPassFilterAccelerometer(lowPassValue);

            var delta = lowPassValue - accForce;
            AccelerationDeltaDrive(delta);
            accForce = lowPassValue;

            // remap the device acceleration axis to game coordinates:
            // Device's XY plane to game's XZ plane
            accForce.x = UnityEngine.Input.acceleration.x;
            accForce.z = UnityEngine.Input.acceleration.y;
            accForce.y = 0;

            // clamp acceleration vector to the unit sphere 
            if (accForce.sqrMagnitude > 1)
            {
                accForce.Normalize();
            }

            accForce *= Speed * Time.deltaTime;

            if ((index++) % 15 == 0)
            {
                Debug.Log(string.Format("mems: {0}, {1}, {2}", UnityEngine.Input.acceleration.x, UnityEngine.Input.acceleration.y, UnityEngine.Input.acceleration.z));
            }

            //Move object
            transform.Translate(accForce);
        }

        private void AccelerationDeltaDrive(Vector3 accDelta)
        {
            accDelta.z = accDelta.y;
            accDelta.y = 0;
            if ((index) % 15 == 0)
            {
                Debug.Log("delta:" + accDelta.sqrMagnitude);
            }
            t = t + Time.deltaTime;
            if (accDelta.sqrMagnitude > ShakeVolve)
            {
                vibrativeForce = lastVibrativeForce + accDelta * 100;
                if (vibrativeForce.sqrMagnitude > 1)
                {
                    vibrativeForce.Normalize();
                }
                t = 0;
            }
            lastVibrativeForce = vibrativeForce * Mathf.Exp(-t / 2) * Mathf.Sin(t * VibrativeFreq);
            //RigidbodyComp.MovePosition(lastVibrativeForce * PushForce);
            if (t < 4 && lastVibrativeForce.sqrMagnitude > 0.0025f)
            {
                //RigidbodyComp.AddForce(lastVibrativeForce);
                CameraFollowComp.enabled = false;
                RigidbodyComp.MovePosition(lastVibrativeForce * PushForce);
            }
            else
            {
                CameraFollowComp.enabled = true;
                RigidbodyComp.MovePosition(Vector3.zero);
            }
        }

        private void InitGyroscope()
        {
            supportGyroscope = SystemInfo.supportsGyroscope;
            Debug.Log("Support Gyroscope: " + supportGyroscope);
            gyroscope = UnityEngine.Input.gyro;
            gyroscope.enabled = true;
        }

        private void CheckGyroscope()
        {
            if (supportGyroscope)
            {

                //获取设备重力加速度向量
                Vector3 deviceGravity = UnityEngine.Input.gyro.gravity;
                //设备的旋转速度，返回结果为x，y，z轴的旋转速度，单位为（弧度/秒）
                Vector3 rotationVelocity = UnityEngine.Input.gyro.rotationRate;
                //获取更加精确的旋转
                Vector3 rotationVelocity2 = UnityEngine.Input.gyro.rotationRateUnbiased;
                //设置陀螺仪的更新检索时间，即隔 0.1秒更新一次
                UnityEngine.Input.gyro.updateInterval = 0.1f;
                //获取移除重力加速度后设备的加速度
                Vector3 acceleration = UnityEngine.Input.gyro.userAcceleration;
                //UnityEngine.Input.gyro.attitude 返回值为 Quaternion类型，即设备旋转欧拉角
                transform.rotation = Quaternion.Slerp(transform.rotation, ConvertRotation(UnityEngine.Input.gyro.attitude), lowPassFilterFactor);
            }
        }



        private Quaternion ConvertRotation(Quaternion q)
        {
            return Quaternion.Euler(90, 0, 0) * (new Quaternion(-q.x, -q.y, q.z, q.w)); //横屏旋转90度，然后左右手坐标系反转。
        }
    }
}