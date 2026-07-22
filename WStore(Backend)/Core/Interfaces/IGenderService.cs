using Bogus.DataSets;
using Core.Models.Product.Gender;

namespace Core.Interfaces;

public interface IGenderService
{
   Task<List<GenderItemModel>> GetAllGenders(string lng);
}