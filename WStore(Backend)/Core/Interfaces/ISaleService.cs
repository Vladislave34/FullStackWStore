using Core.Models.Product.Sale;

namespace Core.Interfaces;

public interface ISaleService
{
    Task<List<SaleItemModel>> GetSales();
}