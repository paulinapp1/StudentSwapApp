using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Domain.Interfaces
{
    public interface ITransactionRepository
    {
        Task AddAsync(PaymentTransaction transaction);
        Task<PaymentTransaction> GetByIdAsync(Guid id);
    }
}

