using UnityEngine;
using Puerts;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine.EventSystems;
using GameFramework.UI.Dialogs.Components;
using GameFramework.Operation;
using GameFramework.Operation.Components;
using GameFramework.Messaging;
using GameFramework.Service;

namespace GameFramework.GameStructure.JsSupport
{
    public delegate void ModuleInit(JsExtBehaviour monoBehaviour);

    public class JsExtBehaviour : GmAwakeDependBehaviour, IPointerClickHandler, IOperationPerformer
    {
        public delegate void OperationCallback(int result);
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
        public Action<CharacterOperation> JsOnOperation;
        public Action<BaseMessage> JsOnMessage;

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

        /// <summary>
        /// Receives the message for the js code
        /// </summary>
        /// <param name="message"></param>
        public void OnMessage(BaseMessage message)
        {
            if (JsOnMessage != null)
            {
                JsOnMessage(message);
            }
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

        /// <summary>
        /// Send user's operation to server side. If got a new operation, execute it
        /// </summary>
        /// <param name="userOperation"></param>
        /// <returns></returns>
        public async Task SendUserOperation(UserOperation userOperation)
        {
            var operation = await OperationService.Instance.SendUserOperation(userOperation);
            if (operation != null)
            {
                operation.Execute(this);
            }
        }

        /// <summary>
        /// Send the result of an operation to the server side. If got a new operation, execute it
        /// </summary>
        /// <param name="result">The result of an operation</param>
        /// <returns></returns>
        public async Task SendOperationResult(OperationResult result)
        {
            var operation = await OperationService.Instance.SendCharacterOperationResult(result);
            if (operation != null)
            {
                operation.Execute(this);
            }
        }

        /// <summary>
        /// Execute the given operation in the js env
        /// </summary>
        /// <param name="operation"></param>
        public void PerformOperation(CharacterOperation operation)
        {
            if (JsOnOperation != null)
            {
                JsOnOperation(operation);
            }
        }
    }
}