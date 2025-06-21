using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentService.Domain.Interfaces;

namespace PaymentService.Domain
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly DataContext _dataContext;
        public TransactionRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<PaymentTransaction> AddAsync(PaymentTransaction transaction)
        {
            await _dataContext.AddAsync(transaction);
            await _dataContext.SaveChangesAsync();
            return transaction;
        }
    

      
    }
}

