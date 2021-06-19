using System;
using System.Threading.Tasks;
using GameFramework.Operation;

namespace GameFramework.Repository
{
    public interface IOperationRepository : IRepository
    {
        Task<CharacterOperation> SendUserOperation(UserOperation operation);
        Task<CharacterOperation> SendCharacterOperationResult(OperationResult result);
    }
}
