using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentService.Domain.Models;

namespace PaymentService.Domain.Interfaces
{
    public interface ITransactionRepository
    {
        Task<PaymentTransaction> AddAsync(PaymentTransaction transaction);
        Task<List<PaymentTransaction>> GetAllTransactions();
    }
}

