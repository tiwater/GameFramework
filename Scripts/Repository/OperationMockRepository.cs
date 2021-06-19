using System;
using System.Threading.Tasks;
using GameFramework.Operation;
using GameFramework.UI.Dialogs.Components;

namespace GameFramework.Repository
{
    public class OperationMockRepository : BaseRepository, IOperationRepository
    {
        public async Task<CharacterOperation> SendCharacterOperationResult(OperationResult result)
        {
            CharacterOperation operation = new Speak("s3", "3", "Got result: " + result.Result, DialogInstance.DialogButtonsType.Ok);
            operation.ExpectFeedback = false;
            return operation;
        }

        public async Task<CharacterOperation> SendUserOperation(UserOperation operation)
        {
            CharacterOperation newOperation = new Speak("s4", operation.OperatorId, "Got UserOperation: " + operation.OperationType, DialogInstance.DialogButtonsType.Ok);
            newOperation.ExpectFeedback = false;
            return newOperation;
        }
    }
}