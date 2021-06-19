using System;
using GameFramework.Operation.Components;
using GameFramework.Service;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace GameFramework.Operation
{
    /// <summary>
    /// Base class for the character's operation
    /// </summary>
    public abstract class CharacterOperation
    {
        /// <summary>
        /// The operation add mode:
        /// Replace will stop and replace current operation;
        /// Insert will stop current operation and start the new operation,
        /// after the new operation ends, the original operation will be resumed;
        /// Add will add the new operation to the end of stack and start it after all previous operation ends
        /// </summary>
        public enum OperationAddMode { Replace, Insert, Add};
        /// <summary>
        /// The operaton id
        /// </summary>
        public string Id;
        /// <summary>
        /// The operator id
        /// </summary>
        public string PlayerGameItemId;
        /// <summary>
        /// Operation type
        /// </summary>
        public string OperationType;
        /// <summary>
        /// Shall we send result of the operation to server side or not
        /// </summary>
        public bool ExpectFeedback;
        /// <summary>
        /// Shall we replace current operation. If not, the original operation will be resumed after the new one ends
        /// </summary>
        public OperationAddMode AddMode;
        /// <summary>
        /// The object to execute this operation
        /// </summary>
        private IOperationPerformer Performer;

        public CharacterOperation()
        {
            OperationType = this.GetType().Name;
        }

        public CharacterOperation(string id, string playerGameItemId, bool feedback = false, OperationAddMode addMode = OperationAddMode.Replace)
        {
            Id = id;
            PlayerGameItemId = playerGameItemId;
            ExpectFeedback = feedback;
            OperationType = this.GetType().Name;
            addMode = addMode;
        }

        public void Execute(IOperationPerformer performer)
        {
            Performer = performer;
            performer.PerformOperation(this);
        }

        /// <summary>
        /// Callback when the operation is done
        /// </summary>
        /// <param name="resultCode"></param>
        public async void OnResult(int resultCode)
        {
            OperationResult result = new OperationResult();
            result.OperationId = Id;
            result.Result = resultCode;
            if (ExpectFeedback)
            {
                //Call service to send the result code
                CharacterOperation operation = await OperationService.Instance.SendCharacterOperationResult(result);

                //If got a new operation for the feedback, execute it
                if (operation != null)
                {
                    //TODO: Check whether the performer for the new operation changed or not
                    operation.Execute(Performer);
                }
            }
        }

        /// <summary>
        /// Deserialize the json string to the CharacterOperation 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static CharacterOperation Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<CharacterOperation>(json, new CharacterOperationConverter<CharacterOperation>());
        }
    }

    public class OperationResult
    {
        public string OperationId;
        public int Result;
    }

    /// <summary>
    /// The operation user started
    /// </summary>
    public class UserOperation
    {
        public enum UserOperationType { Click };

        public string OperatorId;
        public string TargetId;
        public UserOperationType OperationType;

    }

    /// <summary>
    /// The json converter to handle the deserialization of the subclass of CharacterOperation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CharacterOperationConverter<T> : JsonConverter<T> where T : CharacterOperation
    {
        public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var typeName = jsonObject["OperationType"].ToString();

            // Create the instance of the subclass by type name. The type must be under namespace GameFramework.Operation
            var type = Type.GetType("GameFramework.Operation." + typeName);
            var target = (T)Activator.CreateInstance(type);

            // Populate the object's properties
            serializer.Populate(jsonObject.CreateReader(), target);
            return target;
        }

        public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}