using UnityEngine;
using Puerts;
using System;
using System.IO;
using System.Collections;
using GameFramework.GameStructure;
using System.Threading.Tasks;
using UnityEngine.EventSystems;
using GameFramework.UI.Dialogs.Components;

namespace GameFramework.GameStructure.JsSupport
{
    public delegate void ModuleInit(JsExtBehaviour monoBehaviour);

    public class JsExtBehaviour : GmAwakeDependBehaviour, IPointerClickHandler
    {

        /// <summary>
        /// The js module name.
        /// </summary>
        [Tooltip("The js module to load.")]
        public string ModuleName;//The js module to load

        public Action JsStart;
        public Action JsUpdate;
        public Action JsFixedUpdate;
        public Action<PointerEventData> JsOnClick;
        public Action JsOnDestroy;

        private JsEnv JsEnv;
        public bool IsDebug = false;                    // Is it debug
        public int DebugPort = 43990;                   // Debug port
        private JsLoader Loader;
        private string ScriptsDir = Path.Combine(Application.streamingAssetsPath, "Scripts");
        //For TS debug
        //private string ScriptsDir = "/Users/<user>/Projects/carnie/TsProj/output";

        private bool IsJsStartCalled = false;

        public GameManager GameManager { get; set; }

        protected override void GmReadyAwake()
        {

            GameManager = GameManager.Instance;

            Loader = new JsLoader(ScriptsDir);
            if (JsEnv == null) JsEnv = new JsEnv(Loader, DebugPort);

            if (IsDebug)
            {                                //Start debug
                JsEnv.WaitDebugger();
            }

            ModuleInit init;
            init = JsEnv.Eval<ModuleInit>("const m = require('" + ModuleName + "'); m.default;");

            if (init != null)
            {
                init(this);
            }
            else
            {
                Debug.Log("init is null!!!");
            }
        }

        void Start()
        {
            if (JsStart != null)
            {
                JsStart();
                IsJsStartCalled = true;
            }
        }

        void Update()
        {
            //For late awake, it's possible when Start() is called but the JsStart is not available yet
            //So we have to check whether the JsStart is called or not in the Update call
            if (JsUpdate != null)
            {
                if (!IsJsStartCalled)
                {
                    Start();
                    IsJsStartCalled = true;
                }
                JsUpdate();
            }
        }

        void FixedUpdate()
        {
            //For late awake, it's possible when Start() is called but the JsStart is not available yet
            //So we have to check whether the JsStart is called or not in the Update call
            if (JsFixedUpdate != null)
            {
                if (!IsJsStartCalled)
                {
                    Start();
                    IsJsStartCalled = true;
                }
                JsFixedUpdate();
            }
        }

        void OnDestroy()
        {
            if (JsOnDestroy != null)
            {
                JsOnDestroy();
            }
            JsStart = null;
            JsUpdate = null;
            JsOnDestroy = null;

            JsEnv.Dispose();
        }

        public SceneItemInstanceManager GetSceneItemInstanceManager()
        {
            return transform.root.GetComponentInChildren<SceneItemInstanceManager>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (JsOnClick != null)
            {
                JsOnClick(eventData);
            }
        }

        /// <summary>
        /// Process dealy for JS code
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        public async Task Delay(int delay)
        {
            await Task.Delay(delay);
            return;
        }

        public DialogManager GetDialogManager()
        {
            return DialogManager.Instance;
        }

        public async Task PerformOperation()
        {
            //TODO: Call the service to send operation
        }
    }
}