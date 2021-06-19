using System;
using System.Threading.Tasks;
using GameFramework.Operation;

namespace GameFramework.Repository
{
    public class OperationRpcRepository : BaseRepository, IOperationRepository
    {
        public async Task<CharacterOperation> SendCharacterOperationResult(OperationResult result)
        {
            throw new NotImplementedException();
        }

        public async Task<CharacterOperation> SendUserOperation(UserOperation operation)
        {
            throw new NotImplementedException();
        }
    }
}