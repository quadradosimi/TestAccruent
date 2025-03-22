using TestAccruent.Model;

namespace TestAccruent.Service
{
    public interface IProductService
    {
        Task<List<Product>> Get();
        Task<bool> ValidateIfProductExist(string idProduct);
    }
}
