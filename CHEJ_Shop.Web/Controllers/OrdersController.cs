namespace CHEJ_Shop.Web.Controllers
{
    using Data;
    using Data.Repository;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.ViewModels;
    using System;
    using System.Threading.Tasks;

    [Authorize]
    public class OrdersController : Controller
    {
        #region Attributes

        private readonly IOrderRepository iOrderRepository;
        private readonly IProductRepository iProductRepository;

        #endregion Attributes

        #region Constructor

        public OrdersController(
            IOrderRepository _iOrderRepository,
            IProductRepository _iProductRepository)
        {
            this.iOrderRepository = _iOrderRepository;
            this.iProductRepository = _iProductRepository;
        }

        #endregion Constructor

        #region Methods Controller

        public async Task<IActionResult> Index()
        {
            var model = await iOrderRepository.GetOrdersAsync(
                this.User.Identity.Name);
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = await this.iOrderRepository.GetDetailTempsAsync(
                this.User.Identity.Name);
            return this.View(model);
        }

        #endregion Methods Controller

        #region Methods

        public IActionResult AddProduct()
        {
            var model = new AddItemViewModel
            {
                Quantity = 1,
                Products = this.iProductRepository.GetComboProducts()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(
            AddItemViewModel _model)
        {
            if (this.ModelState.IsValid)
            {
                await this.iOrderRepository.AddItemToOrderAsync(_model, this.User.Identity.Name);
                return this.RedirectToAction("Create");
            }

            return this.View(_model);
        }

        public async Task<IActionResult> DeleteItem(
            int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await this.iOrderRepository.DeleteDetailTempAsync(id.Value);
            return this.RedirectToAction("Create");
        }

        public async Task<IActionResult> Increase(
            int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await this.iOrderRepository.ModifyOrderDetailTempQuantityAsync(id.Value, 1);
            return this.RedirectToAction("Create");
        }

        public async Task<IActionResult> Decrease(
            int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await this.iOrderRepository.ModifyOrderDetailTempQuantityAsync(id.Value, -1);
            return this.RedirectToAction("Create");
        }

        public async Task<IActionResult> ConfirmOrder()
        {
            var response = await this.iOrderRepository.ConfirmOrderAsync(
                this.User.Identity.Name);
            if (response)
            {
                return this.RedirectToAction("Index");
            }
            return this.RedirectToAction("Create");
        }

        public async Task<IActionResult> Deliver(
            int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await this.iOrderRepository.GetOrdersAsync(id.Value);
            if (order == null)
            {
                return NotFound();
            }

            var model = new DeliverViewModel
            {
                Id = order.Id,
                DeliveryDate = DateTime.Today
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Deliver(
            DeliverViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                await this.iOrderRepository.DeliverOrder(model);
                return this.RedirectToAction("Index");
            }

            return this.View();
        }


        #endregion Methods
    }
}