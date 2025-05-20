using System.Transactions;

namespace Backend.Core.Helpers
{
    public static class TransactionScopeHelper
    {
        public static TransactionScope StartTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
            => new(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = isolationLevel },
                TransactionScopeAsyncFlowOption.Enabled);
    }
}
