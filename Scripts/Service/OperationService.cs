using System;
using System.Threading.Tasks;
using GameFramework.Operation;
using GameFramework.Repository;

namespace GameFramework.Service
{
    public class OperationService : BaseService
    {
        private IOperationRepository oprRepository;
        protected static OperationService _instance;

        public static OperationService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new OperationService();
                }
                return _instance;
            }
        }

        private OperationService()
        {
            oprRepository = RepoFactory.GetRepository<IOperationRepository>();
        }

        public async Task<CharacterOperation> SendCharacterOperationResult(OperationResult result)
        {
            return await oprRepository.SendCharacterOperationResult(result);
        }

        public async Task<CharacterOperation> SendUserOperation(UserOperation operation)
        {
            return await oprRepository.SendUserOperation(operation);
        }
    }
}