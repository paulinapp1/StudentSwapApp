using PurchaseService.Application.DTO;
using PurchaseService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseService.Application
{
    public interface IPurchaseService
    {
        Task<PurchaseModel> createPurchase( int listingId, int userId);
        Task<PurchaseModel> CancelPurchase(int purchaseId);
    }
}
