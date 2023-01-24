using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader OrderHeader);
        void UpdateStatus(int id, string orderStatus, string? paymentStatus = null);
        public void UpdateStripePaymentID(int id, string sessionId, string paymentItentId);
    }
}
