using PurchaseService.Application.DTO;
using PurchaseService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseService.Application.Interfaces
{
    public interface IPurchaseService
    {
        Task<PurchaseModel> CreatePurchase(int listingId, int userId, CreatePurchaseRequest paymentRequest);
        Task<PurchaseModel> CancelPurchase(int purchaseId);
    }
}
