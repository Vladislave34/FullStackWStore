using AutoMapper;
using Core.Models.Product.Adrress;
using Core.Models.Product.Payment;
using Domain.Entities;

namespace Core.Mappers;

public class PaymentMapper :  Profile
{
    public PaymentMapper()
    {
        CreateMap<PaymentEntity, PaymentItemModel>();
        CreateMap<PaymentAddUpdateModel, PaymentEntity>();

    }
}