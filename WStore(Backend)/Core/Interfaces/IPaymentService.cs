using Core.Models.Product.Payment;

namespace Core.Interfaces;

public interface IPaymentService
{
    Task AddCard(PaymentAddUpdateModel model);
    Task UpdateCard(Guid id, PaymentAddUpdateModel model);
    Task<ICollection<PaymentItemModel>> GetCardByUser();
    
    Task DeleteCard(Guid id);
}