namespace CHEJ_Shop.Web.Data.Repository
{
    using Helpers;
    using Models.ViewModels;
    using Entities;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;
    using System;

    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        #region Attributes

        private readonly DataContext context;
        private readonly IUserHelper iUserHelper;

        #endregion Attributes

        #region Constructor

        public OrderRepository(
            DataContext _context,
            IUserHelper _iUserHelper) : base(_context)
        {
            this.context = _context;
            this.iUserHelper = _iUserHelper;
        }

        #endregion Constructor

        #region Methods

        public async Task<IQueryable<Order>> GetOrdersAsync(
            string _userName)
        {
            var user = await this.iUserHelper.GetUserByEmailAsync(_userName);
            if (user == null)
            {
                return null;
            }

            if (await this.iUserHelper.IsUserInRoleAsync(
                user,
                MethodsHelper.RoleAdmin))
            {
                return this.context.Orders
                    .Include(o => o.User)
                    .Include(o => o.Items)
                    .ThenInclude(p => p.Product)
                    .OrderBy(o => o.Id);
            }

            return this.context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(p => p.Product)
                .Where(o => o.User == user)
                .OrderBy(o => o.Id);
        }

        public async Task<IQueryable<OrderDetailTemp>> GetDetailTempsAsync(
            string _userName)
        {
            var user = await this.iUserHelper.GetUserByEmailAsync(_userName);
            if (user == null)
            {
                return null;
            }

            return this.context.OrderDetailTemps
                .Include(o => o.Product)
                .Where(o => o.User == user)
                .OrderBy(o => o.Product.Name);
        }

        public async Task AddItemToOrderAsync(
            AddItemViewModel _model,
            string _userName)
        {
            var user = await this.iUserHelper.GetUserByEmailAsync(_userName);
            if (user == null)
            {
                return;
            }

            var product = await this.context.Products.FindAsync(_model.ProductId);
            if (product == null)
            {
                return;
            }

            var orderDetailTemp = await this.context.OrderDetailTemps
                .Where(odt => odt.User == user && odt.Product == product)
                .FirstOrDefaultAsync();
            if (orderDetailTemp == null)
            {
                orderDetailTemp = new OrderDetailTemp
                {
                    Price = product.Price,
                    Product = product,
                    Quantity = _model.Quantity,
                    User = user,
                };

                this.context.OrderDetailTemps.Add(orderDetailTemp);
            }
            else
            {
                orderDetailTemp.Quantity += _model.Quantity;
                this.context.OrderDetailTemps.Update(orderDetailTemp);
            }

            await this.context.SaveChangesAsync();
        }

        public async Task ModifyOrderDetailTempQuantityAsync(
            int _id,
            double _quantity)
        {
            var orderDetailTemp = await this.context.OrderDetailTemps.FindAsync(_id);
            if (orderDetailTemp == null)
            {
                return;
            }

            orderDetailTemp.Quantity += _quantity;
            if (orderDetailTemp.Quantity > 0)
            {
                this.context.OrderDetailTemps.Update(orderDetailTemp);
                await this.context.SaveChangesAsync();
            }
        }

        public async Task DeleteDetailTempAsync(
            int _id)
        {
            var orderDetailTemp = await this.context.OrderDetailTemps.FindAsync(_id);
            if (orderDetailTemp == null)
            {
                return;
            }

            this.context.OrderDetailTemps.Remove(orderDetailTemp);
            await this.context.SaveChangesAsync();
        }

        public async Task<bool> ConfirmOrderAsync(
            string _userName)
        {
            if (_userName == null)
            {
                throw new ArgumentNullException(nameof(_userName));
            }

            var user = await this.iUserHelper.GetUserByEmailAsync(_userName);
            if (user == null)
            {
                return false;
            }

            var orderTmps = await this.context.OrderDetailTemps
                .Include(o => o.Product)
                .Where(o => o.User == user)
                .ToListAsync();

            if (orderTmps == null || orderTmps.Count == 0)
            {
                return false;
            }

            var details = orderTmps.Select(o => new OrderDetail
            {
                Price = o.Price,
                Product = o.Product,
                Quantity = o.Quantity
            }).ToList();

            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                User = user,
                Items = details,
            };

            this.context.Orders.Add(order);
            this.context.OrderDetailTemps.RemoveRange(orderTmps);
            await this.context.SaveChangesAsync();
            return true;
        }

        public async Task DeliverOrder(
            DeliverViewModel _model)
        {
            var order = await this.context.Orders.FindAsync(_model.Id);
            if (order == null)
            {
                return;
            }

            order.DeliveryDate = _model.DeliveryDate;
            this.context.Orders.Update(order);
            await this.context.SaveChangesAsync();
        }

        public async Task<Order> GetOrdersAsync(
            int id)
        {
            return await this.context.Orders.FindAsync(id);
        }

        #endregion Methods
    }
}