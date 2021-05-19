using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework.UI.Other.Components
{
    /// <summary>
    /// Show FPS for debug purpose.
    /// </summary>
    [AddComponentMenu("Game Framework/UI/Other/ShowFps")]
    [HelpURL("http://www.flipwebapps.com/unity-assets/game-framework/ui/")]
    public class ShowFps : MonoBehaviour
    {
        /// <summary>
        /// Last time to update the frame rate
        /// </summary>
        private float m_lastUpdateShowTime = 0f;
        /// <summary>
        /// The interval to update the frame rate
        /// </summary>
        private readonly float m_updateTime = 0.05f;
        /// <summary>
        /// The frames displayed
        /// </summary>
        private int m_frames = 0;
        private float m_FPS = 0;
        private Rect m_fps;
        private GUIStyle m_style = new GUIStyle();

        void Awake()
        {
            Application.targetFrameRate = 100;
        }

        void Start()
        {
            m_lastUpdateShowTime = Time.realtimeSinceStartup;
            m_fps = new Rect(150, 0, 250, 200);
            m_style.fontSize = 30;
            m_style.normal.textColor = Color.red;
        }

        void Update()
        {
            m_frames++;
            if (Time.realtimeSinceStartup - m_lastUpdateShowTime >= m_updateTime)
            {
                m_FPS = m_frames / (Time.realtimeSinceStartup - m_lastUpdateShowTime);
                m_frames = 0;
                m_lastUpdateShowTime = Time.realtimeSinceStartup;
                //Debug.Log("FPS: " + m_FPS + "，间隔: " + m_FrameDeltaTime);
            }
        }

        void OnGUI()
        {
            GUI.Label(m_fps, "FPS: " + m_FPS, m_style);
        }

    }
}