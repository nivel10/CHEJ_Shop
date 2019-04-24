namespace CHEJ_Shop.Web.Data
{
    using Entities;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Repository : IRepository
    {
        private readonly DataContext dataContext;

        public Repository(DataContext _dataContext)
        {
            this.dataContext = _dataContext;
        }

        public IEnumerable<Product> GetProducts()
        {
            return this.dataContext.Products.OrderBy(p => p.Name);
        }

        public Product GetProduct(int id)
        {
            return this.dataContext.Products.Find(id);
        }

        public void AddProduct(Product product)
        {
            this.dataContext.Products.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            this.dataContext.Update(product);
        }

        public void RemoveProduct(Product product)
        {
            this.dataContext.Products.Remove(product);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await this.dataContext.SaveChangesAsync() > 0;
        }

        public bool ProductExists(int id)
        {
            return this.dataContext.Products.Any(p => p.Id == id);
        }
    }
}