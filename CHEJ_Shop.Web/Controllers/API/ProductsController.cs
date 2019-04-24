namespace CHEJ_Shop.Web.Controllers.API
{
    using Data.Entities;
    using Data;
    using Helpers;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using System.IO;
    using System;

    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductsController : Controller
    {
        #region Attributes

        private readonly IProductRepository iProductRepository;
        private readonly IUserHelper iUserHelper;

        #endregion Attributes

        #region Constructor

        public ProductsController(
           IProductRepository _productRepository,
           IUserHelper _iUserHelper)
        {
            this.iProductRepository = _productRepository;
            this.iUserHelper = _iUserHelper;
        }

        #endregion Constructor

        #region Methods

        [HttpGet]
        public IActionResult GetProducts()
        {
            #region Old Code

            //var listProducts = this.productRepository.GetAllWithUsers();

            //return Ok(listProducts); 

            #endregion Old Code

            return Ok(this.iProductRepository.GetAllWithUsers());
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct(
            [FromBody] Common.Models.Product product)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var user = await this.iUserHelper.GetUserByEmailAsync(product.User.Email);
            if (user == null)
            {
                return this.BadRequest("Invalid user");
            }

            var imageUrl = string.Empty;
            if (product.ImageArray != null && product.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(product.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                //  var folder = "wwwroot\\images\\Products";
                var fullPath = $"{MethodsHelper.GetUrlImagesProducts}{file}";

                var response = Common.Helpers.FilesHelper.UplodaImage(
                    stream,
                    MethodsHelper.GetPathImagesProducts,
                    file);

                if (response.IsSuccess)
                {
                    imageUrl = fullPath;
                }
            }

            var entityProduct = new Product
            {
                IsAvailabe = product.IsAvailabe,
                LastPurchase = product.LastPurchase,
                LastSale = product.LastSale,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                User = user,
                ImageUrl = imageUrl,
            };

            var newProduct = await this.iProductRepository.CreateAsync(entityProduct);
            return Ok(newProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(
            [FromRoute] int id, [FromBody] Product newProduct)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            if (id != newProduct.Id)
            {
                return BadRequest();
            }

            var oldProduct = await this.iProductRepository.GetByIdAsync(id);
            if (oldProduct == null)
            {
                return this.BadRequest($"Product Id: {id} don't exists.");
            }

            //TODO: Upload images
            oldProduct.IsAvailabe = newProduct.IsAvailabe;
            oldProduct.LastPurchase = newProduct.LastPurchase;
            oldProduct.LastSale = newProduct.LastSale;
            oldProduct.Name = newProduct.Name;
            oldProduct.Price = newProduct.Price;
            oldProduct.Stock = newProduct.Stock;

            var updatedProduct = await this.iProductRepository.UpdateAsync(oldProduct);
            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var product = await this.iProductRepository.GetByIdAsync(id);
            if (product == null)
            {
                return this.NotFound();
            }

            await this.iProductRepository.DeleteAsync(product);
            return Ok(product);
        }

        #endregion Methods
    }
}