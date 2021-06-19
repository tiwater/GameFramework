using System;

namespace GameFramework.Operation.Components
{
    /// <summary>
    /// Interface to handle the operation execution
    /// </summary>
    public interface IOperationPerformer
    {
        /// <summary>
        /// Execute the operation
        /// </summary>
        /// <param name="operation"></param>
        void PerformOperation(CharacterOperation operation);
    }
}