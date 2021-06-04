using System;
using GameFramework.Platform.Abstract;
using GameFramework.Platform.Android;

namespace GameFramework.Platform
{
    /// <summary>
    /// The factory to get a UnityPlatformBridge instance
    /// </summary>
    public class UnityPlatformBridgeFactory
    {
        private static UnityPlatformBridge Instance;

        public static UnityPlatformBridge GetUnityPlatformBridge()
        {
            if (Instance == null)
            {
#if UNITY_EDITOR
                Instance = new UnityMockBridge();
#elif UNITY_ANDROID
                //And register the receiver in Android layer

                Instance = new UnityAndroidBridge();
#endif
            }
            return Instance;
        }
    }
}