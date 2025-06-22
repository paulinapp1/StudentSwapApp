using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Interfaces;

namespace PaymentService.Domain.Models
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

        public async Task<List<PaymentTransaction>> GetAllTransactions()
        {
            return await _dataContext.Payments.ToListAsync();
        }

    }
}

