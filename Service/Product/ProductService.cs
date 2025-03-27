using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenant.API.Base.Repository;
using Tenant.API.Base.Service;
using Tenant.Query.Model.Product;
using static System.Net.Mime.MediaTypeNames;

namespace Tenant.Query.Service.Product
{
    public class ProductService : TnBaseService
    {
        #region Private property

        private Repository.Product.ProductRepository productRepository;
        private readonly ILoggerFactory _loggerFactory;

        #endregion

        public ProductService(Repository.Product.ProductRepository productRepository, 
                            IConfiguration configuration, 
                            ILoggerFactory loggerFactory, 
                            TnAudit xcAudit, 
                            TnValidation xcValidation) : base(xcAudit, xcValidation)
        {
            this.productRepository = productRepository;
            this._loggerFactory = loggerFactory;
            this.productRepository.Logger = loggerFactory.CreateLogger<Repository.Product.ProductRepository>();
        }

        private static void MapInvoiceTenantVendor(ProductMaster productMaster, DataRow row)
        {
            productMaster.Id = GetColumnValue<long>(row, "tenant_vendor_id", 0);
            productMaster.ProductName= GetColumnValue<string>(row, "tenant_vendor_name", string.Empty);
            productMaster.Active= GetColumnValue<bool>(row, "default_invoice_id_as_invoice_date", false);
        }

        private static T GetColumnValue<T>(DataRow row, string columnName, T defaultValue = default)
        {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
            {
                return (T)Convert.ChangeType(row[columnName], typeof(T));
            }
            return defaultValue;
        }

        internal List<Model.Product.ProductMaster> GetProductMaster(string tenantId, PorductFilter filter)
        {
            try
            {
                //Local variable
                List<Model.Product.ProductMaster> productMaster = new List<ProductMaster>();

                //Get Invoice aggregate
                productMaster = this.productRepository.GetProductMaster(tenantId, filter).Result;

                //Return
                return productMaster;
            }
            catch (KeyNotFoundException ex)
            {
                throw ex;   
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Model.Response.MenuMaster.MenuMaster> GetMenuMaster()
        {
            List<Model.Response.MenuMaster.MenuMaster> menu = (List<Model.Response.MenuMaster.MenuMaster>)Newtonsoft.Json.JsonConvert.DeserializeObject("[ { \"menuId\": 1, \"menuName\": \"Home\", \"orderBy\": 1, \"active\": true, \"image\": \"\", \"subMenu\": false, \"category\": [], \"link\": \"/\" }, { \"menuId\": 2, \"menuName\": \"Seed\", \"orderBy\": 2, \"active\": true, \"image\": \"\", \"subMenu\": true, \"category\": [ { \"categoryId\": 1, \"category\": \"All Seed\", \"active\": true }, { \"categoryId\": 2, \"category\": \"Vegetable\", \"active\": true }, { \"categoryId\": 3, \"category\": \"Herbal\", \"active\": true }, { \"categoryId\": 4, \"category\": \"Fruits\", \"active\": true }, { \"categoryId\": 5, \"category\": \"Greens\", \"active\": true } ] }, { \"menuId\": 3, \"menuName\": \"Plants\", \"orderBy\": 3, \"active\": true, \"image\": \"\", \"subMenu\": true, \"category\": [ { \"categoryId\": 1, \"category\": \"All Plants\", \"active\": true }, { \"categoryId\": 2, \"category\": \"Indoor\", \"active\": true }, { \"categoryId\": 3, \"category\": \"Outdoor\", \"active\": true }, { \"categoryId\": 4, \"category\": \"New Arrivals\", \"active\": true }, { \"categoryId\": 5, \"category\": \"Air Furify\", \"active\": true } ] }, { \"menuId\": 4, \"menuName\": \"Contact Us\", \"orderBy\": 4, \"active\": true, \"link\": \"/contactus\", \"image\": \"\", \"subMenu\": false } ]", typeof(List<Model.Response.MenuMaster.MenuMaster>));

            return menu.OrderBy(x => x.orderBy).ToList();
        }

        public List<Model.Response.ProductMaster.ProductMaster> GetProductMaster()
        {
            List<Model.Response.ProductMaster.ProductMaster> menu = (List<Model.Response.ProductMaster.ProductMaster>)Newtonsoft.Json.JsonConvert.DeserializeObject("[\r\n        {\r\n            \"productId\": 1001,\r\n            \"tenantId\": 10,\r\n            \"productName\": \"Apple\",\r\n            \"productDescription\": \"Apple\",\r\n            \"productCode\": \"SD101\",\r\n            \"fullDescription\": \"\",\r\n            \"specification\": \"\",\r\n            \"story\": \"\",\r\n            \"packQuantity\": 10,\r\n            \"quantity\": 10,\r\n            \"total\": 100,\r\n            \"price\": 200,\r\n            \"category\": 1,\r\n            \"rating\": 2,\r\n            \"active\": true,\r\n            \"trending\": 1,\r\n            \"userBuyCount\": 50,\r\n            \"return\": 2,\r\n            \"created\": \"date\",\r\n            \"modified\": \"date\",\r\n            \"in_stock\": true,\r\n            \"best_seller\": false,\r\n            \"deleveryDate\": 5,\r\n            \"offer\": \"50%\",\r\n            \"orderBy\": 15,\r\n            \"userId\": 1,\r\n            \"overview\": \"Lorem ipsum dolor sit amet consectetur adipisicing elit. Error unde quisquam magni vel eligendi nam.\",\r\n            \"long_description\": \"Lorem ipsum dolor sit amet consectetur, adipisicing elit. Soluta aut, vel ipsum maxime quam quia, quaerat tempore minus odio exercitationem illum et eos, quas ipsa aperiam magnam officiis libero expedita quo voluptas deleniti sit dolore? Praesentium tempora cumque facere consectetur quia, molestiae quam, accusamus eius corrupti laudantium aliquid! Tempore laudantium unde labore voluptates repellat, dignissimos aperiam ad ipsum laborum recusandae voluptatem non dolore. Reiciendis cum quo illum. Dolorem, molestiae corporis.\",\r\n            \"images\": [\r\n                {\r\n                    \"imageId\": 1,\r\n                    \"poster\": \"https://images.unsplash.com/photo-1633356122544-f134324a6cee?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=650&q=40\",\r\n                    \"main\" : true,\r\n                    \"active\":true,\r\n                    \"orderBy\": 1\r\n                }\r\n            ]\r\n        },\r\n        {\r\n            \"productId\": 1002,\r\n            \"tenantId\": 10,\r\n            \"productName\": \"Orange\",\r\n            \"productDescription\": \"Orange\",\r\n            \"productCode\": \"OR1O1\",\r\n            \"fullDescription\": \"\",\r\n            \"specification\": \"\",\r\n            \"story\": \"\",\r\n            \"packQuantity\": 10,\r\n            \"quantity\": 100,\r\n            \"total\": 100,\r\n            \"price\": 200,\r\n            \"category\": 2,\r\n            \"rating\": 3,\r\n            \"active\": true,\r\n            \"trending\": 1,\r\n            \"userBuyCount\": 50,\r\n            \"return\": 1,\r\n            \"created\": \"date\",\r\n            \"modified\": \"date\",\r\n            \"in_stock\": true,\r\n            \"best_seller\": false,\r\n            \"deleveryDate\": 5,\r\n            \"offer\": \"50%\",\r\n            \"orderBy\": 1,\r\n            \"userId\": 1,\r\n            \"overview\": \"Lorem ipsum dolor sit amet consectetur adipisicing elit. Error unde quisquam magni vel eligendi nam.\",\r\n            \"long_description\": \"Lorem ipsum dolor sit amet consectetur, adipisicing elit. Soluta aut, vel ipsum maxime quam quia, quaerat tempore minus odio exercitationem illum et eos, quas ipsa aperiam magnam officiis libero expedita quo voluptas deleniti sit dolore? Praesentium tempora cumque facere consectetur quia, molestiae quam, accusamus eius corrupti laudantium aliquid! Tempore laudantium unde labore voluptates repellat, dignissimos aperiam ad ipsum laborum recusandae voluptatem non dolore. Reiciendis cum quo illum. Dolorem, molestiae corporis.\",\r\n            \"images\": [\r\n                {\r\n                    \"imageId\": 1,\r\n                    \"poster\": \"https://images.unsplash.com/photo-1580894894513-541e068a3e2b?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=650&q=40\",\r\n                    \"orderBy\": 1,\r\n                    \"main\" : true,\r\n                    \"active\":true\r\n                }\r\n            ]\r\n        },\r\n        {\r\n            \"productId\": 1003,\r\n            \"tenantId\": 10,\r\n            \"productName\": \"Graps\",\r\n            \"productDescription\": \"Graps\",\r\n            \"productCode\": \"DR101\",\r\n            \"fullDescription\": \"\",\r\n            \"specification\": \"\",\r\n            \"story\": \"\",\r\n            \"packQuantity\": 10,\r\n            \"quantity\": 200,\r\n            \"total\": 200,\r\n            \"price\": 200,\r\n            \"category\": 2,\r\n            \"rating\": 1,\r\n            \"active\": true,\r\n            \"trending\": 1,\r\n            \"userBuyCount\": 50,\r\n            \"return\": 2,\r\n            \"created\": \"date\",\r\n            \"modified\": \"date\",\r\n            \"in_stock\": true,\r\n            \"best_seller\": false,\r\n            \"deleveryDate\": 5,\r\n            \"offer\": \"50%\",\r\n            \"orderBy\": 2,\r\n            \"userId\": 1,\r\n            \"overview\": \"Lorem ipsum dolor sit amet consectetur adipisicing elit. Error unde quisquam magni vel eligendi nam.\",\r\n            \"long_description\": \"Lorem ipsum dolor sit amet consectetur, adipisicing elit. Soluta aut, vel ipsum maxime quam quia, quaerat tempore minus odio exercitationem illum et eos, quas ipsa aperiam magnam officiis libero expedita quo voluptas deleniti sit dolore? Praesentium tempora cumque facere consectetur quia, molestiae quam, accusamus eius corrupti laudantium aliquid! Tempore laudantium unde labore voluptates repellat, dignissimos aperiam ad ipsum laborum recusandae voluptatem non dolore. Reiciendis cum quo illum. Dolorem, molestiae corporis.\",\r\n            \"images\": [\r\n                {\r\n                    \"imageId\": 1,\r\n                    \"poster\": \"https://images.unsplash.com/photo-1523726491678-bf852e717f6a?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=650&q=40\",\r\n                    \"main\" : true,\r\n                    \"active\":true,\r\n                    \"orderBy\": 1\r\n                }\r\n            ]\r\n        },\r\n        {\r\n            \"productId\": 1004,\r\n            \"tenantId\": 10,\r\n            \"productName\": \"Givi\",\r\n            \"productDescription\": \"Givi\",\r\n            \"productCode\": \"app001\",\r\n            \"fullDescription\": \"\",\r\n            \"specification\": \"\",\r\n            \"story\": \"\",\r\n            \"packQuantity\": 10,\r\n            \"quantity\": 400,\r\n            \"total\": 400,\r\n            \"price\": 400,\r\n            \"category\": 3,\r\n            \"rating\": 5,\r\n            \"active\": true,\r\n            \"trending\": 1,\r\n            \"userBuyCount\": 50,\r\n            \"return\": 1,\r\n            \"created\": \"date\",\r\n            \"modified\": \"date\",\r\n            \"in_stock\": true,\r\n            \"best_seller\": false,\r\n            \"deleveryDate\": 5,\r\n            \"offer\": \"10%\",\r\n            \"orderBy\": 3,\r\n            \"userId\": 1,\r\n            \"overview\": \"Lorem ipsum dolor sit amet consectetur adipisicing elit. Error unde quisquam magni vel eligendi nam.\",\r\n            \"long_description\": \"Lorem ipsum dolor sit amet consectetur, adipisicing elit. Soluta aut, vel ipsum maxime quam quia, quaerat tempore minus odio exercitationem illum et eos, quas ipsa aperiam magnam officiis libero expedita quo voluptas deleniti sit dolore? Praesentium tempora cumque facere consectetur quia, molestiae quam, accusamus eius corrupti laudantium aliquid! Tempore laudantium unde labore voluptates repellat, dignissimos aperiam ad ipsum laborum recusandae voluptatem non dolore. Reiciendis cum quo illum. Dolorem, molestiae corporis.\",\r\n            \"images\": [\r\n                {\r\n                    \"imageId\": 1,\r\n                    \"poster\": \"https://images.unsplash.com/photo-1523726491678-bf852e717f6a?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=650&q=40\",\r\n                    \"main\" : true,\r\n                    \"active\":true,\r\n                    \"orderBy\": 1\r\n                }\r\n            ]\r\n        },\r\n        {\r\n            \"productId\": 1005,\r\n            \"tenantId\": 10,\r\n            \"productName\": \"Gova\",\r\n            \"productDescription\": \"Gova\",\r\n            \"productCode\": \"app001\",\r\n            \"fullDescription\": \"\",\r\n            \"specification\": \"\",\r\n            \"story\": \"\",\r\n            \"packQuantity\": 10,\r\n            \"quantity\": 50,\r\n            \"total\": 50,\r\n            \"price\": 50,\r\n            \"category\": 1,\r\n            \"rating\": 3,\r\n            \"active\": true,\r\n            \"trending\": 1,\r\n            \"userBuyCount\": 50,\r\n            \"return\": 1,\r\n            \"created\": \"date\",\r\n            \"modified\": \"date\",\r\n            \"in_stock\": false,\r\n            \"best_seller\": true,\r\n            \"deleveryDate\": 5,\r\n            \"offer\": \"70%\",\r\n            \"orderBy\": 4,\r\n            \"userId\": 1,\r\n            \"overview\": \"Lorem ipsum dolor sit amet consectetur adipisicing elit. Error unde quisquam magni vel eligendi nam.\",\r\n            \"long_description\": \"Lorem ipsum dolor sit amet consectetur, adipisicing elit. Soluta aut, vel ipsum maxime quam quia, quaerat tempore minus odio exercitationem illum et eos, quas ipsa aperiam magnam officiis libero expedita quo voluptas deleniti sit dolore? Praesentium tempora cumque facere consectetur quia, molestiae quam, accusamus eius corrupti laudantium aliquid! Tempore laudantium unde labore voluptates repellat, dignissimos aperiam ad ipsum laborum recusandae voluptatem non dolore. Reiciendis cum quo illum. Dolorem, molestiae corporis.\",\r\n            \"images\": [\r\n                {\r\n                    \"imageId\": 1,\r\n                    \"poster\": \"https://images.unsplash.com/photo-1595617795501-9661aafda72a?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=650&q=40\",\r\n                    \"main\" : true,\r\n                    \"active\":true,\r\n                    \"orderBy\": 1\r\n                }\r\n            ]\r\n        },\r\n        {\r\n            \"productId\": 1006,\r\n            \"tenantId\": 10,\r\n            \"productName\": \"Mango\",\r\n            \"productDescription\": \"Mango\",\r\n            \"productCode\": \"PRT100\",\r\n            \"fullDescription\": \"\",\r\n            \"specification\": \"\",\r\n            \"story\": \"\",\r\n            \"packQuantity\": 10,\r\n            \"quantity\": 60,\r\n            \"total\": 60,\r\n            \"price\": 60,\r\n            \"category\": 1,\r\n            \"rating\": 4,\r\n            \"active\": true,\r\n            \"trending\": 1,\r\n            \"userBuyCount\": 50,\r\n            \"return\": 2,\r\n            \"created\": \"date\",\r\n            \"modified\": \"date\",\r\n            \"in_stock\": false,\r\n            \"best_seller\": true,\r\n            \"deleveryDate\": 5,\r\n            \"offer\": \"80%\",\r\n            \"orderBy\": 5,\r\n            \"userId\": 1,\r\n            \"overview\": \"Lorem ipsum dolor sit amet consectetur adipisicing elit. Error unde quisquam magni vel eligendi nam.\",\r\n            \"long_description\": \"Lorem ipsum dolor sit amet consectetur, adipisicing elit. Soluta aut, vel ipsum maxime quam quia, quaerat tempore minus odio exercitationem illum et eos, quas ipsa aperiam magnam officiis libero expedita quo voluptas deleniti sit dolore? Praesentium tempora cumque facere consectetur quia, molestiae quam, accusamus eius corrupti laudantium aliquid! Tempore laudantium unde labore voluptates repellat, dignissimos aperiam ad ipsum laborum recusandae voluptatem non dolore. Reiciendis cum quo illum. Dolorem, molestiae corporis.\",\r\n            \"images\": [\r\n                {\r\n                    \"imageId\": 1,\r\n                    \"poster\": \"https://images.unsplash.com/photo-1639322537228-f710d846310a?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=650&q=40\",\r\n                    \"main\" : true,\r\n                    \"active\":true,\r\n                    \"orderBy\": 1\r\n                }\r\n            ]\r\n        },\r\n        {\r\n            \"productId\": 1007,\r\n            \"tenantId\": 10,\r\n            \"productName\": \"JackFruit\",\r\n            \"productDescription\": \"JackFruit\",\r\n            \"productCode\": \"app001\",\r\n            \"fullDescription\": \"\",\r\n            \"specification\": \"\",\r\n            \"story\": \"\",\r\n            \"packQuantity\": 10,\r\n            \"quantity\": 70,\r\n            \"total\": 70,\r\n            \"price\": 70,\r\n            \"category\": 1,\r\n            \"rating\": 1,\r\n            \"active\": true,\r\n            \"trending\": 1,\r\n            \"userBuyCount\": 50,\r\n            \"return\": 2,\r\n            \"created\": \"date\",\r\n            \"modified\": \"date\",\r\n            \"in_stock\": true,\r\n            \"best_seller\": false,\r\n            \"deleveryDate\": 5,\r\n            \"offer\": \"20%\",\r\n            \"orderBy\": 6,\r\n            \"userId\": 1,\r\n            \"overview\": \"Lorem ipsum dolor sit amet consectetur adipisicing elit. Error unde quisquam magni vel eligendi nam.\",\r\n            \"long_description\": \"Lorem ipsum dolor sit amet consectetur, adipisicing elit. Soluta aut, vel ipsum maxime quam quia, quaerat tempore minus odio exercitationem illum et eos, quas ipsa aperiam magnam officiis libero expedita quo voluptas deleniti sit dolore? Praesentium tempora cumque facere consectetur quia, molestiae quam, accusamus eius corrupti laudantium aliquid! Tempore laudantium unde labore voluptates repellat, dignissimos aperiam ad ipsum laborum recusandae voluptatem non dolore. Reiciendis cum quo illum. Dolorem, molestiae corporis.\",\r\n            \"images\": [\r\n                {\r\n                    \"imageId\": 1,\r\n                    \"poster\": \"https://images.unsplash.com/photo-1522542550221-31fd19575a2d?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=650&q=40\",\r\n                    \"main\" : true,\r\n                    \"active\":true,\r\n                    \"orderBy\": 1\r\n                }\r\n            ]\r\n        },\r\n        {\r\n            \"productId\": 1008,\r\n            \"tenantId\": 10,\r\n            \"productName\": \"Banana\",\r\n            \"productDescription\": \"Banana\",\r\n            \"productCode\": \"app001\",\r\n            \"fullDescription\": \"\",\r\n            \"specification\": \"\",\r\n            \"story\": \"\",\r\n            \"packQuantity\": 10,\r\n            \"quantity\": 80,\r\n            \"total\": 80,\r\n            \"price\": 80,\r\n            \"category\": 1,\r\n            \"rating\": 4,\r\n            \"active\": true,\r\n            \"trending\": 1,\r\n            \"userBuyCount\": 50,\r\n            \"return\": 2,\r\n            \"created\": \"date\",\r\n            \"modified\": \"date\",\r\n            \"in_stock\": true,\r\n            \"best_seller\": false,\r\n            \"deleveryDate\": 5,\r\n            \"offer\": \"40%\",\r\n            \"orderBy\": 7,\r\n            \"userId\": 1,\r\n            \"overview\": \"Lorem ipsum dolor sit amet consectetur adipisicing elit. Error unde quisquam magni vel eligendi nam.\",\r\n            \"long_description\": \"Lorem ipsum dolor sit amet consectetur, adipisicing elit. Soluta aut, vel ipsum maxime quam quia, quaerat tempore minus odio exercitationem illum et eos, quas ipsa aperiam magnam officiis libero expedita quo voluptas deleniti sit dolore? Praesentium tempora cumque facere consectetur quia, molestiae quam, accusamus eius corrupti laudantium aliquid! Tempore laudantium unde labore voluptates repellat, dignissimos aperiam ad ipsum laborum recusandae voluptatem non dolore. Reiciendis cum quo illum. Dolorem, molestiae corporis.\",\r\n            \"images\": [\r\n                {\r\n                    \"imageId\": 1,\r\n                    \"poster\": \"https://images.unsplash.com/photo-1621839673705-6617adf9e890?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=650&q=40\",\r\n                    \"main\" : true,\r\n                    \"active\":true,\r\n                    \"orderBy\": 1\r\n                }\r\n            ]\r\n        },\r\n        {\r\n            \"productId\": 1009,\r\n            \"tenantId\": 10,\r\n            \"productName\": \"Cherry\",\r\n            \"productDescription\": \"Cherry\",\r\n            \"productCode\": \"app001\",\r\n            \"fullDescription\": \"\",\r\n            \"specification\": \"\",\r\n            \"story\": \"\",\r\n            \"packQuantity\": 10,\r\n            \"quantity\": 90,\r\n            \"total\": 90,\r\n            \"price\": 90,\r\n            \"category\": 1,\r\n            \"rating\": 5,\r\n            \"active\": true,\r\n            \"trending\": 1,\r\n            \"userBuyCount\": 50,\r\n            \"return\": 1,\r\n            \"created\": \"date\",\r\n            \"modified\": \"date\",\r\n            \"in_stock\": true,\r\n            \"best_seller\": false,\r\n            \"deleveryDate\": 5,\r\n            \"offer\": \"20%\",\r\n            \"orderBy\": 8,\r\n            \"userId\": 1,\r\n            \"overview\": \"Lorem ipsum dolor sit amet consectetur adipisicing elit. Error unde quisquam magni vel eligendi nam.\",\r\n            \"long_description\": \"Lorem ipsum dolor sit amet consectetur, adipisicing elit. Soluta aut, vel ipsum maxime quam quia, quaerat tempore minus odio exercitationem illum et eos, quas ipsa aperiam magnam officiis libero expedita quo voluptas deleniti sit dolore? Praesentium tempora cumque facere consectetur quia, molestiae quam, accusamus eius corrupti laudantium aliquid! Tempore laudantium unde labore voluptates repellat, dignissimos aperiam ad ipsum laborum recusandae voluptatem non dolore. Reiciendis cum quo illum. Dolorem, molestiae corporis.\",\r\n            \"images\": [\r\n                {\r\n                    \"imageId\": 1,\r\n                    \"poster\": \"https://images.unsplash.com/photo-1613490900233-141c5560d75d?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=650&q=40\",\r\n                    \"main\" : true,\r\n                    \"active\":true,\r\n                    \"orderBy\": 1\r\n                }\r\n            ]\r\n        },\r\n        {\r\n            \"productId\": 1010,\r\n            \"tenantId\": 10,\r\n            \"productName\": \"Water Apple\",\r\n            \"productDescription\": \"Water Apple\",\r\n            \"productCode\": \"app001\",\r\n            \"fullDescription\": \"\",\r\n            \"specification\": \"\",\r\n            \"story\": \"\",\r\n            \"packQuantity\": 10,\r\n            \"quantity\": 10,\r\n            \"total\": 10,\r\n            \"price\": 20,\r\n            \"category\": 1,\r\n            \"rating\": 1,\r\n            \"active\": true,\r\n            \"trending\": 1,\r\n            \"userBuyCount\": 50,\r\n            \"return\": 2,\r\n            \"created\": \"date\",\r\n            \"modified\": \"date\",\r\n            \"in_stock\": true,\r\n            \"best_seller\": false,\r\n            \"deleveryDate\": 5,\r\n            \"offer\": \"50%\",\r\n            \"orderBy\": 9,\r\n            \"userId\": 1,\r\n            \"overview\": \"Lorem ipsum dolor sit amet consectetur adipisicing elit. Error unde quisquam magni vel eligendi nam.\",\r\n            \"long_description\": \"Lorem ipsum dolor sit amet consectetur, adipisicing elit. Soluta aut, vel ipsum maxime quam quia, quaerat tempore minus odio exercitationem illum et eos, quas ipsa aperiam magnam officiis libero expedita quo voluptas deleniti sit dolore? Praesentium tempora cumque facere consectetur quia, molestiae quam, accusamus eius corrupti laudantium aliquid! Tempore laudantium unde labore voluptates repellat, dignissimos aperiam ad ipsum laborum recusandae voluptatem non dolore. Reiciendis cum quo illum. Dolorem, molestiae corporis.\",\r\n            \"images\": [\r\n                {\r\n                    \"imageId\": 1,\r\n                    \"poster\": \"https://images.unsplash.com/photo-1624953587687-daf255b6b80a?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=650&q=40\",\r\n                    \"main\" : true,\r\n                    \"active\":true,\r\n                    \"orderBy\": 1\r\n                }\r\n            ]\r\n        },\r\n        {\r\n            \"productId\": 1011,\r\n            \"tenantId\": 10,\r\n            \"productName\": \"Pappaya\",\r\n            \"productDescription\": \"Pappaya\",\r\n            \"productCode\": \"app001\",\r\n            \"fullDescription\": \"\",\r\n            \"specification\": \"\",\r\n            \"story\": \"\",\r\n            \"packQuantity\": 10,\r\n            \"quantity\": 110,\r\n            \"total\": 110,\r\n            \"price\": 210,\r\n            \"category\": 1,\r\n            \"rating\": 3,\r\n            \"active\": true,\r\n            \"trending\": 1,\r\n            \"userBuyCount\": 50,\r\n            \"return\": 2,\r\n            \"created\": \"date\",\r\n            \"modified\": \"date\",\r\n            \"in_stock\": true,\r\n            \"best_seller\": false,\r\n            \"deleveryDate\": 5,\r\n            \"offer\": \"50%\",\r\n            \"orderBy\": 10,\r\n            \"userId\": 1,\r\n            \"overview\": \"Lorem ipsum dolor sit amet consectetur adipisicing elit. Error unde quisquam magni vel eligendi nam.\",\r\n            \"long_description\": \"Lorem ipsum dolor sit amet consectetur, adipisicing elit. Soluta aut, vel ipsum maxime quam quia, quaerat tempore minus odio exercitationem illum et eos, quas ipsa aperiam magnam officiis libero expedita quo voluptas deleniti sit dolore? Praesentium tempora cumque facere consectetur quia, molestiae quam, accusamus eius corrupti laudantium aliquid! Tempore laudantium unde labore voluptates repellat, dignissimos aperiam ad ipsum laborum recusandae voluptatem non dolore. Reiciendis cum quo illum. Dolorem, molestiae corporis.\",\r\n            \"images\": [\r\n                {\r\n                    \"imageId\": 1,\r\n                    \"poster\": \"https://images.unsplash.com/photo-1623479322729-28b25c16b011?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=650&q=40\",\r\n                    \"main\" : true,\r\n                    \"active\":true,\r\n                    \"orderBy\": 1\r\n                }\r\n            ]\r\n        },\r\n        {\r\n            \"productId\": 1012,\r\n            \"tenantId\": 10,\r\n            \"productName\": \"Sapotta\",\r\n            \"productDescription\": \"Sapotta\",\r\n            \"productCode\": \"app001\",\r\n            \"fullDescription\": \"\",\r\n            \"specification\": \"\",\r\n            \"story\": \"\",\r\n            \"packQuantity\": 10,\r\n            \"quantity\": 120,\r\n            \"total\": 120,\r\n            \"price\": 220,\r\n            \"category\": 1,\r\n            \"rating\": 4,\r\n            \"active\": true,\r\n            \"trending\": 1,\r\n            \"userBuyCount\": 50,\r\n            \"return\": 2,\r\n            \"created\": \"date\",\r\n            \"modified\": \"date\",\r\n            \"in_stock\": true,\r\n            \"best_seller\": false,\r\n            \"deleveryDate\": 5,\r\n            \"offer\": \"50%\",\r\n            \"orderBy\": 11,\r\n            \"userId\": 1,\r\n            \"overview\": \"Lorem ipsum dolor sit amet consectetur adipisicing elit. Error unde quisquam magni vel eligendi nam.\",\r\n            \"long_description\": \"Lorem ipsum dolor sit amet consectetur, adipisicing elit. Soluta aut, vel ipsum maxime quam quia, quaerat tempore minus odio exercitationem illum et eos, quas ipsa aperiam magnam officiis libero expedita quo voluptas deleniti sit dolore? Praesentium tempora cumque facere consectetur quia, molestiae quam, accusamus eius corrupti laudantium aliquid! Tempore laudantium unde labore voluptates repellat, dignissimos aperiam ad ipsum laborum recusandae voluptatem non dolore. Reiciendis cum quo illum. Dolorem, molestiae corporis.\",\r\n            \"images\": [\r\n                {\r\n                    \"imageId\": 1,\r\n                    \"poster\": \"https://images.unsplash.com/photo-1507721999472-8ed4421c4af2?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=650&q=40\",\r\n                    \"main\" : true,\r\n                    \"active\":true,\r\n                    \"orderBy\": 1\r\n                }\r\n            ]\r\n        },\r\n        {\r\n            \"productId\": 1013,\r\n            \"tenantId\": 10,\r\n            \"productName\": \"Pomagrante\",\r\n            \"productDescription\": \"Pomagrante\",\r\n            \"productCode\": \"app001\",\r\n            \"fullDescription\": \"\",\r\n            \"specification\": \"\",\r\n            \"story\": \"\",\r\n            \"packQuantity\": 10,\r\n            \"quantity\": 130,\r\n            \"total\": 130,\r\n            \"price\": 230,\r\n            \"category\": 1,\r\n            \"rating\": 1,\r\n            \"active\": true,\r\n            \"trending\": 1,\r\n            \"userBuyCount\": 50,\r\n            \"return\": 2,\r\n            \"created\": \"date\",\r\n            \"modified\": \"date\",\r\n            \"in_stock\": true,\r\n            \"best_seller\": false,\r\n            \"deleveryDate\": 5,\r\n            \"offer\": \"50%\",\r\n            \"orderBy\": 12,\r\n            \"userId\": 1,\r\n            \"overview\": \"Lorem ipsum dolor sit amet consectetur adipisicing elit. Error unde quisquam magni vel eligendi nam.\",\r\n            \"long_description\": \"Lorem ipsum dolor sit amet consectetur, adipisicing elit. Soluta aut, vel ipsum maxime quam quia, quaerat tempore minus odio exercitationem illum et eos, quas ipsa aperiam magnam officiis libero expedita quo voluptas deleniti sit dolore? Praesentium tempora cumque facere consectetur quia, molestiae quam, accusamus eius corrupti laudantium aliquid! Tempore laudantium unde labore voluptates repellat, dignissimos aperiam ad ipsum laborum recusandae voluptatem non dolore. Reiciendis cum quo illum. Dolorem, molestiae corporis.\",\r\n            \"images\": [\r\n                {\r\n                    \"imageId\": 1,\r\n                    \"poster\": \"https://images.unsplash.com/photo-1633356122102-3fe601e05bd2?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=650&q=40\",\r\n                    \"main\" : true,\r\n                    \"active\":true,\r\n                    \"orderBy\": 1\r\n                }\r\n            ]\r\n        },\r\n        {\r\n            \"productId\": 1014,\r\n            \"tenantId\": 10,\r\n            \"productName\": \"Apple\",\r\n            \"productDescription\": \"Ooty Apple\",\r\n            \"productCode\": \"app001\",\r\n            \"fullDescription\": \"\",\r\n            \"specification\": \"\",\r\n            \"story\": \"\",\r\n            \"packQuantity\": 10,\r\n            \"quantity\": 140,\r\n            \"total\": 140,\r\n            \"price\": 140,\r\n            \"category\": 1,\r\n            \"rating\": 5,\r\n            \"active\": true,\r\n            \"trending\": 1,\r\n            \"userBuyCount\": 50,\r\n            \"return\": 2,\r\n            \"created\": \"date\",\r\n            \"modified\": \"date\",\r\n            \"in_stock\": true,\r\n            \"best_seller\": false,\r\n            \"deleveryDate\": 5,\r\n            \"offer\": \"50%\",\r\n            \"orderBy\": 13,\r\n            \"userId\": 1,\r\n            \"overview\": \"Lorem ipsum dolor sit amet consectetur adipisicing elit. Error unde quisquam magni vel eligendi nam.\",\r\n            \"long_description\": \"Lorem ipsum dolor sit amet consectetur, adipisicing elit. Soluta aut, vel ipsum maxime quam quia, quaerat tempore minus odio exercitationem illum et eos, quas ipsa aperiam magnam officiis libero expedita quo voluptas deleniti sit dolore? Praesentium tempora cumque facere consectetur quia, molestiae quam, accusamus eius corrupti laudantium aliquid! Tempore laudantium unde labore voluptates repellat, dignissimos aperiam ad ipsum laborum recusandae voluptatem non dolore. Reiciendis cum quo illum. Dolorem, molestiae corporis.\",\r\n            \"images\": [\r\n                {\r\n                    \"imageId\": 1,\r\n                    \"poster\": \"https://images.unsplash.com/photo-1515879218367-8466d910aaa4?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=650&q=40\",\r\n                    \"main\" : true,\r\n                    \"active\":true,\r\n                    \"orderBy\": 1\r\n                }\r\n            ]\r\n        },\r\n        {\r\n            \"productId\": 1015,\r\n            \"tenantId\": 10,\r\n            \"productName\": \"Apple\",\r\n            \"productDescription\": \"Ooty Apple\",\r\n            \"productCode\": \"app001\",\r\n            \"fullDescription\": \"\",\r\n            \"specification\": \"\",\r\n            \"story\": \"\",\r\n            \"packQuantity\": 10,\r\n            \"quantity\": 100,\r\n            \"total\": 100,\r\n            \"price\": 200,\r\n            \"category\": 1,\r\n            \"rating\": 1,\r\n            \"active\": true,\r\n            \"trending\": 1,\r\n            \"userBuyCount\": 50,\r\n            \"return\": 1,\r\n            \"created\": \"date\",\r\n            \"modified\": \"date\",\r\n            \"in_stock\": true,\r\n            \"best_seller\": false,\r\n            \"deleveryDate\": 5,\r\n            \"offer\": \"50%\",\r\n            \"orderBy\": 14,\r\n            \"userId\": 1,\r\n            \"overview\": \"Lorem ipsum dolor sit amet consectetur adipisicing elit. Error unde quisquam magni vel eligendi nam.\",\r\n            \"long_description\": \"Lorem ipsum dolor sit amet consectetur, adipisicing elit. Soluta aut, vel ipsum maxime quam quia, quaerat tempore minus odio exercitationem illum et eos, quas ipsa aperiam magnam officiis libero expedita quo voluptas deleniti sit dolore? Praesentium tempora cumque facere consectetur quia, molestiae quam, accusamus eius corrupti laudantium aliquid! Tempore laudantium unde labore voluptates repellat, dignissimos aperiam ad ipsum laborum recusandae voluptatem non dolore. Reiciendis cum quo illum. Dolorem, molestiae corporis.\",\r\n            \"images\": [\r\n                {\r\n                    \"imageId\": 1,\r\n                    \"poster\": \"https://images.unsplash.com/photo-1587440871875-191322ee64b0?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=650&q=40\",\r\n                    \"main\" : true,\r\n                    \"active\":true,\r\n                    \"orderBy\": 1\r\n                }\r\n            ]\r\n        }\r\n    ]", typeof(List<Model.Response.ProductMaster.ProductMaster>));

            return menu.OrderBy(x => x.OrderBy).ToList();
        }

        public Model.Response.ProductMaster.ProductMaster GetProductDetails(string productId)
        {
            List<Model.Response.ProductMaster.ProductMaster> productMaster = GetProductMaster();

            return productMaster.Where(x => x.ProductId == Convert.ToInt64(productId)).FirstOrDefault();
        }

        public List<Model.Response.CartList> GetCartProduct()
        {
            List<Model.Response.CartList> cartLists = (List<Model.Response.CartList>)Newtonsoft.Json.JsonConvert.DeserializeObject("[\r\n        {\r\n            \"productId\": 1001,\r\n            \"productName\": \"Apple\",\r\n            \"tenantId\": 10,\r\n            \"quantity\": 9,\r\n            \"orderBy\": 15,\r\n            \"userId\": 1,\r\n            \"price\": 200,\r\n            \"images\": [\r\n                {\r\n                    \"imageId\": 1,\r\n                    \"poster\": \"https://images.unsplash.com/photo-1580894894513-541e068a3e2b?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=650&q=40\",\r\n                    \"orderBy\": 1,\r\n                    \"main\": true,\r\n                    \"active\": true\r\n                }\r\n            ]\r\n        }\r\n    ]", typeof(List<Model.Response.CartList>));

            return cartLists.OrderBy(x => x.OrderBy).ToList();
        }

        public List<Model.Response.Category> GetCategory(string tenantId)
        {
            try
            {
                List<Model.Product.ProductCategory> productCategories = this.productRepository.GetCategory(tenantId);

                List<Model.Response.Category> categories = new List<Model.Response.Category>();

                productCategories.Where(x => x.SubCategory == 0).OrderBy(x => x.Id).ToList().ForEach(category =>
                {
                    Model.Response.Category cate = new Model.Response.Category();
                    List<Model.Response.SubCategory> categoryList = new List<Model.Response.SubCategory>();

                    List<Model.Product.ProductCategory> subCategories = productCategories.Where(x => x.SubCategory == category.Id).ToList();

                    cate.Id = category.Id;
                    cate.Name = category.Category;

                    subCategories.ForEach(sub => {
                        Model.Response.SubCategory subCategory = new Model.Response.SubCategory();
                        subCategory.Id = sub.Id;
                        subCategory.Name = sub.Category;
                        subCategory.Order = sub.Id;
                        categoryList.Add(subCategory);
                    });
                    cate.subCategories = categoryList;

                    categories.Add(cate);
                });

                //string psw = EnDecrypt("I0FEO09BK0VEEN9DGJ9FGU8BMQ8DKTSP", false);

                return categories;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Model.Product.Response GetInvoiceItemsForMapping(string tenantId, Model.Product.InvoiceItemMappingPayload payload)
        {
            try
            {

                string spName = Model.Constant.Constant.SA_GET_PRODUCTS_FOR_CATEGORY_MAPPING;
                string[] filters = payload.OrderBy?.Split(',');
                string orderBy = string.IsNullOrEmpty(filters?.ElementAtOrDefault(0)) ? Model.Constant.Constant.VendorName : filters[0];
                string order = string.IsNullOrEmpty(filters?.ElementAtOrDefault(1)) ? Model.Constant.Constant.ASC : filters[1];

                List<Model.Product.SpInvoiceItemMappingResponse> invoiceItemsForMapping = this.productRepository.GetInvoiceItemsForMapping(tenantId, payload, orderBy, order, spName);
                Model.Product.Response response = new Model.Product.Response();
                if (invoiceItemsForMapping == null || invoiceItemsForMapping.Count < 1)
                    return response;
                Model.Product.InvoiceItemMappingDetails mapitem;
                foreach (Model.Product.SpInvoiceItemMappingResponse invoiceitems in invoiceItemsForMapping)
                {
                    mapitem = new Model.Product.InvoiceItemMappingDetails()
                    {
                        RowNumber = invoiceitems.RowNumber,
                        ProductId = invoiceitems.ProductId,
                        VendorProductCode = invoiceitems.VendorProductCode,
                        ProductDescription = invoiceitems.ProductDescription,
                        ExtractPreference = invoiceitems.ExtractPreference,
                        VendorId = invoiceitems.VendorId,
                        VendorName = invoiceitems.VendorName,
                        SizePack = invoiceitems.SizePack,
                        UnitOfMeasurement = invoiceitems.UnitOfMeasurement,
                        TenantVendorId = invoiceitems.TenantVendorId,
                        ProductExtentedId = invoiceitems.ProductExtentedId,
                    };
                    if (response.InvoiceItemMappingDetails is null)
                    {
                        response.TotalRowCount = invoiceitems.TotalRowCount;
                        response.InvoiceItemMappingDetails = new List<InvoiceItemMappingDetails>();
                    }
                    response.InvoiceItemMappingDetails.Add(mapitem);
                }
                
                return response;
            }
            catch (Exception ex)
            {
                //logger
                //this.Logger.LogError($"Error while calling GetInvoiceItemsForMapping for TenantId: {tenantId}:" + ex);
                throw ex;
            }
        }

        public Model.Product.ProductMaster GetPartnerEventDetails(string RestaurantGuid, string LocationGuid)
        {
            try
            {
                string spName = this.Configuration["StoredProcedure:GetPartnerEventDetail"];
                var detail = this.productRepository.GetPartnerEventDetails(RestaurantGuid: RestaurantGuid, LocationGuid: LocationGuid, storeProcedureName: spName).Result;

                if (detail == null)
                    this.Logger.LogWarning($"Called service : Partner detail not exists for Restaurant Guid {RestaurantGuid} and Location Guid {LocationGuid} ");
                else
                    this.Logger.LogWarning($"Called service: Received partner detail for Restaurant Guid {RestaurantGuid} and Location Guid {LocationGuid} ");

                return detail;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Product details by filter in sp route
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        internal List<Model.Product.ProductMaster> GetProducts(string tenantId, Model.Product.Search search)
        {
            try
            {
                this.Logger.LogInformation($"getting the the list of prduct detils by filter in sp route for ( tenant :{tenantId} )");

                string spName = String.Empty;
                List<Model.Product.ProductMaster> products = new List<Model.Product.ProductMaster>();

                if (search.SubCategory == null)
                    search.SubCategory = new List<string>();
                if (search.Category == null)
                    search.Category = new List<string>();

                //product catalog sp name
                spName = "XC_GET_PRODUCT_DETAILS";
                //calling the product repository to get the list of prduct detils
                products = this.productRepository.GetProductDtailsUsingSp(tenantId, spName, search);

                return products;

            }
            catch (KeyNotFoundException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static string EnDecrypt(string input, bool decrypt = false)
        {
            string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ984023";

            if (decrypt)
            {
                Dictionary<string, uint> _index = null;
                Dictionary<string, Dictionary<string, uint>> _indexes =
                    new Dictionary<string, Dictionary<string, uint>>(2, StringComparer.InvariantCulture);

                if (_index == null)
                {
                    Dictionary<string, uint> cidx;

                    string indexKey = "I" + _alphabet;

                    if (!_indexes.TryGetValue(indexKey, out cidx))
                    {
                        lock (_indexes)
                        {
                            if (!_indexes.TryGetValue(indexKey, out cidx))
                            {
                                cidx = new Dictionary<string, uint>(_alphabet.Length, StringComparer.InvariantCulture);
                                for (int i = 0; i < _alphabet.Length; i++)
                                {
                                    cidx[_alphabet.Substring(i, 1)] = (uint)i;
                                }

                                _indexes.Add(indexKey, cidx);
                            }
                        }
                    }

                    _index = cidx;
                }

                MemoryStream ms = new MemoryStream(Math.Max((int)Math.Ceiling(input.Length * 5 / 8.0), 1));

                for (int i = 0; i < input.Length; i += 8)
                {
                    int chars = Math.Min(input.Length - i, 8);

                    ulong val = 0;

                    int bytes = (int)Math.Floor(chars * (5 / 8.0));

                    for (int charOffset = 0; charOffset < chars; charOffset++)
                    {
                        uint cbyte;
                        if (!_index.TryGetValue(input.Substring(i + charOffset, 1), out cbyte))
                        {
                            throw new ArgumentException(string.Format("Invalid character {0} valid characters are: {1}",
                                input.Substring(i + charOffset, 1), _alphabet));
                        }

                        val |= (((ulong)cbyte) << ((((bytes + 1) * 8) - (charOffset * 5)) - 5));
                    }

                    byte[] buff = BitConverter.GetBytes(val);
                    Array.Reverse(buff);
                    ms.Write(buff, buff.Length - (bytes + 1), bytes);
                }

                return System.Text.ASCIIEncoding.ASCII.GetString(ms.ToArray());
            }
            else
            {
                byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes(input);

                StringBuilder result = new StringBuilder(Math.Max((int)Math.Ceiling(data.Length * 8 / 5.0), 1));

                byte[] emptyBuff = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
                byte[] buff = new byte[8];

                for (int i = 0; i < data.Length; i += 5)
                {
                    int bytes = Math.Min(data.Length - i, 5);

                    Array.Copy(emptyBuff, buff, emptyBuff.Length);
                    Array.Copy(data, i, buff, buff.Length - (bytes + 1), bytes);
                    Array.Reverse(buff);
                    ulong val = BitConverter.ToUInt64(buff, 0);

                    for (int bitOffset = ((bytes + 1) * 8) - 5; bitOffset > 3; bitOffset -= 5)
                    {
                        result.Append(_alphabet[(int)((val >> bitOffset) & 0x1f)]);
                    }
                }

                return result.ToString();
            }
        }
    }
}
