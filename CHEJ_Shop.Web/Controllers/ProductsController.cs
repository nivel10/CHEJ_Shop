namespace CHEJ_Shop.Web.Controllers
{
    using Data;
    using Data.Entities;
    using Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductsController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IUserHelper userHelper;

        public ProductsController(
            IProductRepository _productRepository,
            IUserHelper _userHelper)
        {
            this.productRepository = _productRepository;
            this.userHelper = _userHelper;
        }

        #region Methods View Controller

        // GET: Products
        public IActionResult Index()
        {
            return View(this.productRepository.GetAll().OrderBy(p => p.Name));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                //  return NotFound();
                return new NotFoundViewResult("ProductNotFound");
            }

            var product = await this.productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                //  return NotFound();
                return new NotFoundViewResult("ProductNotFound");
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(Product product)
        public async Task<IActionResult> Create(
            ProductViewModel _productViewModel)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (_productViewModel.ImageFile != null &&
                    _productViewModel.ImageFile.Length > 0)
                {
                    //  CHEJ - GUID
                    var guidProduct = Guid.NewGuid().ToString();
                    var fileName = $"{guidProduct}.jpg";

                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\Products",
                        fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await _productViewModel.ImageFile.CopyToAsync(stream);
                    }

                    path = $"~/images/Products/{fileName}";
                }

                _productViewModel.User =
                    await this.userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                var product = this.ToProduct(_productViewModel, path);

                await this.productRepository.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }

            return View(_productViewModel);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await this.productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            //  return View(product);
            var productViewModel = ToProducViewModel(product);
            return View(productViewModel);

        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //  public async Task<IActionResult> Edit(Product product)
        public async Task<IActionResult> Edit(ProductViewModel _productViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = _productViewModel.ImageUrl;

                    if (_productViewModel.ImageFile != null &&
                        _productViewModel.ImageFile.Length > 0)
                    {
                        //  CHEJ - GUID
                        var guidProduct = Guid.NewGuid().ToString();
                        var fileName = $"{guidProduct}.jpg";

                        path = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot\\images\\Products",
                            fileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await _productViewModel.ImageFile.CopyToAsync(stream);
                        }

                        path = $"~/images/Products/{fileName}";
                    }

                    _productViewModel.User = await this.userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    var product = this.ToProduct(_productViewModel, path);
                    await this.productRepository.UpdateAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.productRepository.ExistAsync(_productViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(_productViewModel);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await this.productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await this.productRepository.GetByIdAsync(id);
            await this.productRepository.DeleteAsync(product);
            return RedirectToAction(nameof(Index));
        }

        #endregion Methods View Controller

        #region Methods others

        private Product ToProduct(
            ProductViewModel _productViewModel,
            string path)
        {
            return new Product
            {
                Id = _productViewModel.Id,
                ImageUrl = path,
                IsAvailabe = _productViewModel.IsAvailabe,
                LastPurchase = _productViewModel.LastPurchase,
                LastSale = _productViewModel.LastSale,
                Name = _productViewModel.Name,
                Price = _productViewModel.Price,
                Stock = _productViewModel.Stock,
                User = _productViewModel.User
            };
        }

        private ProductViewModel ToProducViewModel(
            Product _product)
        {
            return new ProductViewModel
            {
                Id = _product.Id,
                ImageUrl = _product.ImageUrl,
                IsAvailabe = _product.IsAvailabe,
                LastPurchase = _product.LastPurchase,
                LastSale = _product.LastSale,
                Name = _product.Name,
                Price = _product.Price,
                Stock = _product.Stock,
                User = _product.User
            };
        }

        public ActionResult ProductNotFound()
        {
            return this.View();
        }

        #endregion  Methods others
    }
}