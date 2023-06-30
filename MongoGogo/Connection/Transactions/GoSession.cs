using MongoDB.Driver;
using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace MongoGogo.Connection.Transactions
{
    internal class GoSession<TContext>: IDisposable
    {
        private readonly GoTransactionOption _option;

        private IGoContext<TContext> _goContext { get; set; }

        private IClientSessionHandle _session;

        internal IClientSessionHandle Session
        {
            get 
            {
                if(_session == null)
                {
                    var sessionOption = new ClientSessionOptions();
                    if (_option != null) sessionOption.CausalConsistency = _option.CausalConsistency;
                    _session = _goContext.StartSession(sessionOption);
                    _session.StartTransaction();
                }
                return _session;
            }
        }

        private GoTransactionStatus _Status { get; set; }

        internal bool HasAnyOperation 
        {
            get 
            {
                return _Status.HasAnyOperation;
            }
            set
            {
                _Status.HasAnyOperation = value;
            } 
        }

        public GoSession(IGoContext<TContext> goContext, GoTransactionOption option = default)
        {
            _goContext = goContext;
            _option = option;
            _session = default;
            _Status = new GoTransactionStatus();
        }

        internal void CommitTransaction()
        {
            _session?.CommitTransaction();
        }

        internal async Task CommitTransactionAsync()
        {
            await _session?.CommitTransactionAsync();
        }

        public void Dispose()
        {
            _session?.Dispose();
        }
    }
}
