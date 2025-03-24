using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tenant.API.Base.Controller;
using Tenant.API.Base.Model;
using Tenant.Query.Model.Product;
using Tenant.Query.Service.Product;

namespace Tenant.Query.Controllers.Product
{
    [Route("api/1.0/products")]
    public class ProductsController : TnBaseController<Service.Product.ProductService>
    {
        Service.Product.ProductService productService;

        public ProductsController(ProductService service, IConfiguration configuration, ILoggerFactory loggerFactory) : base(service, configuration, loggerFactory)
        {
            this.productService = service;
        }

        /// <summary>
        /// Get list of product for the tenant and vendor
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="vendorId"></param>
        /// <returns></returns>
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(List<Model.Product.ProductMaster>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        [Route("tenants/{tenantId}")]
        [HttpGet]
        public IActionResult GetProducts([FromRoute] string tenantId, [FromBody] Search search)
        {
            try
            {
                //get list of products
                List<Model.Product.ProductMaster> products = this.Service.GetProducts(tenantId, search);

                //return data
                return StatusCode(StatusCodes.Status200OK, new ApiResult() { Data = products });
            }
            catch (KeyNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ApiResult() { Exception = ex.Message });
            }
            catch (System.Exception ex)
            {
                API.Base.Model.Exception modelException = new API.Base.Model.Exception(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResult() { Exception = modelException });
            }
        }

        [HttpGet]
        [Route("tenants/{tenantId}/products")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(Model.Product.ProductMaster))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request", typeof(ApiResult))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Resource not found", typeof(ApiResult))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ApiResult))]
        public IActionResult GetProducts([FromRoute] string tenantId,
                                         [FromQuery(Name = "include-inactive")] bool includeInactive,
                                         [FromQuery(Name = "include-category")] bool includeCategory,
                                         [FromQuery(Name = "include-images")] bool includeImages,
                                         [FromQuery(Name = "include-reviews")] bool includeReviews)
        {
            try
            {
                //TODO set Query parameter to the filter
                Model.Product.PorductFilter filter = new Model.Product.PorductFilter()
                {
                    IncludeInactive = includeInactive,
                    IncludeCategory = includeCategory,
                    IncludeImages = includeImages,
                    IncludeReviews = includeReviews
                };

                //Local variable
                List<Model.Product.ProductMaster> productMaster = new List<Model.Product.ProductMaster>();

                //calling service
                productMaster = this.Service.GetProductMaster(tenantId, filter);

                // Return productMaster
                return StatusCode(StatusCodes.Status200OK, new ApiResult() { Data = productMaster });
            }
            // key not found exeception
            catch (KeyNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ApiResult() { Exception = ex.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResult() { Exception = ex.Message });
            }
        }

        [HttpGet]
        [Route("tenants/{tenantId}/category")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(Model.Product.ProductCategory))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request", typeof(ApiResult))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Resource not found", typeof(ApiResult))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ApiResult))]
        public IActionResult GetCategory([FromRoute] string tenantId)
        {
            try
            {
                //Local variable
                List<Model.Response.Category> productCategories = new List<Model.Response.Category>();

                //calling service
                productCategories = this.Service.GetCategory(tenantId);

                // Return productMaster
                return StatusCode(StatusCodes.Status200OK, new ApiResult() { Data = productCategories });
            }
            // key not found exeception
            catch (KeyNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ApiResult() { Exception = ex.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResult() { Exception = ex.Message });
            }
        }

        [HttpGet]
        [Route("tenants/{tenantId}/menu-master")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(Model.Product.ProductCategory))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request", typeof(ApiResult))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Resource not found", typeof(ApiResult))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ApiResult))]
        public IActionResult GetMenuMaster([FromRoute] string tenantId)
        {
            try
            {
                //Local variable
                List<Model.Response.MenuMaster.MenuMaster> menuMasters = new List<Model.Response.MenuMaster.MenuMaster>();

                //calling service
                menuMasters = this.Service.GetMenuMaster();

                // Return productMaster
                return StatusCode(StatusCodes.Status200OK, new ApiResult() { Data = menuMasters });
            }
            // key not found exeception
            catch (KeyNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ApiResult() { Exception = ex.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResult() { Exception = ex.Message });
            }
        }

        [HttpGet]
        [Route("tenants/{tenantId}/product-master")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(Model.Product.ProductCategory))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request", typeof(ApiResult))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Resource not found", typeof(ApiResult))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ApiResult))]
        public IActionResult GetProductMaster([FromRoute] string tenantId)
        {
            try
            {
                //Local variable
                List<Model.Response.ProductMaster.ProductMaster> productMasters = new List<Model.Response.ProductMaster.ProductMaster>();

                //calling service
                productMasters = this.Service.GetProductMaster();

                // Return productMaster
                return StatusCode(StatusCodes.Status200OK, new ApiResult() { Data = productMasters });
            }
            // key not found exeception
            catch (KeyNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ApiResult() { Exception = ex.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResult() { Exception = ex.Message });
            }
        }

        [HttpGet]
        [Route("{productId}/product-details")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(Model.Product.ProductCategory))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request", typeof(ApiResult))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Resource not found", typeof(ApiResult))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ApiResult))]
        public IActionResult GetProductDetails([FromRoute] string productId)
        {
            try
            {
                //Local variable
                Model.Response.ProductMaster.ProductMaster productDetails = new Model.Response.ProductMaster.ProductMaster();

                //calling service
                productDetails = this.Service.GetProductDetails(productId);

                // Return productMaster
                return StatusCode(StatusCodes.Status200OK, new ApiResult() { Data = productDetails });
            }
            // key not found exeception
            catch (KeyNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ApiResult() { Exception = ex.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResult() { Exception = ex.Message });
            }
        }

        [HttpGet]
        [Route("tenants/{tenantId}/cart-list")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(Model.Product.ProductCategory))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request", typeof(ApiResult))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Resource not found", typeof(ApiResult))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ApiResult))]
        public IActionResult GetCartProduct([FromRoute] string tenantId)
        {
            try
            {
                //Local variable
                List<Model.Response.CartList> productMasters = new List<Model.Response.CartList>();

                //calling service
                productMasters = this.Service.GetCartProduct();

                // Return productMaster
                return StatusCode(StatusCodes.Status200OK, new ApiResult() { Data = productMasters });
            }
            // key not found exeception
            catch (KeyNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ApiResult() { Exception = ex.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResult() { Exception = ex.Message });
            }
        }

        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(ApiResult))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Success", typeof(ApiResult))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("tenants/{tenantId}/get-invoice-items")]
        public IActionResult GetInvoiceItemsForMapping([FromRoute] string tenantId, [FromBody] Model.Product.InvoiceItemMappingPayload payload)
        {
            try
            {
                //this.Logger.LogInformation($"Get invoice item(s) of tenantId : {tenantId} for mapping ");

                Model.Product.Response responses = this.Service.GetInvoiceItemsForMapping(tenantId, payload);

                return StatusCode(StatusCodes.Status200OK, new ApiResult() { Data = responses });
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResult() { Exception = ex.Message });
            }
        }
    }
}
