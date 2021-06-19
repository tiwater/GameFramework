using System;
using UnityEngine;
using static GameFramework.UI.Dialogs.Components.DialogInstance;

/// <summary>
/// The subclass of CharacterOperation must be put into namespace GameFramework.Operation,
/// then it can be desearilzed by the JsonConvert
/// </summary>
namespace GameFramework.Operation
{
    /// <summary>
    /// To speak with the user
    /// </summary>
    public class Speak : CharacterOperation
    {

        public string Content;
        public DialogButtonsType DialogType;
        public int SpeakResult;

        public Speak()
        {
        }

        public Speak(string Id, string PlayerGameItemId, string content, DialogButtonsType dialogType) : base(Id, PlayerGameItemId)
        {
            Content = content;
            DialogType = dialogType;
        }
    }


    /// <summary>
    /// To move the operator
    /// </summary>
    public class Move : CharacterOperation
    {

        public Vector3 Target;
        public int SpeedIdx;

        public Move()
        {
        }

        public Move(string Id, string PlayerGameItemId, Vector3 target, int speedIdx) : base(Id, PlayerGameItemId)
        {
            Target = target;
            SpeedIdx = speedIdx;
        }

    }

    /// <summary>
    /// To bubble
    /// </summary>
    public class Bubble : CharacterOperation
    {

        public Bubble()
        {
        }

        public Bubble(string Id, string PlayerGameItemId) : base(Id, PlayerGameItemId)
        {
        }

    }

    /// <summary>
    /// Idling...
    /// </summary>
    public class Idle : CharacterOperation
    {

        public Idle()
        {
        }


        public Idle(string Id, string PlayerGameItemId) : base(Id, PlayerGameItemId)
        {
        }

    }

    /// <summary>
    /// Normal actions
    /// </summary>
    public class NormalShowAction : CharacterOperation
    {
        protected string ActionType;

        public NormalShowAction()
        {
        }

        public NormalShowAction(string Id, string PlayerGameItemId, string actionType) : base(Id, PlayerGameItemId)
        {
            ActionType = actionType;
        }

    }

    /// <summary>
    /// Sleep for a specified duration in seconds
    /// </summary>
    public class Sleep : CharacterOperation
    {
        protected long SleepTime;

        public Sleep()
        {
        }

        public Sleep(string Id, string PlayerGameItemId, long sleepTime) : base(Id, PlayerGameItemId)
        {
            SleepTime = sleepTime;
        }

    }
}