using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentService.Domain.Interfaces;

namespace PaymentService.Domain
{
    public class InMemoryTransactionRepository : ITransactionRepository
    {
        private static readonly ConcurrentDictionary<Guid, PaymentTransaction> _transactions
            = new ConcurrentDictionary<Guid, PaymentTransaction>();

        public Task AddAsync(PaymentTransaction transaction)
        {
            _transactions[transaction.Id] = transaction;
            return Task.CompletedTask;
        }

        public Task<PaymentTransaction> GetByIdAsync(Guid id)
        {
            _transactions.TryGetValue(id, out var transaction);
            return Task.FromResult(transaction);
        }

        public Task<IEnumerable<PaymentTransaction>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<PaymentTransaction>>(_transactions.Values);
        }
    }
}

