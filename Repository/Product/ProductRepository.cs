using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Tenant.API.Base.Repository;
using Tenant.Query.Context.Product; 
using Tenant.Query.Model.Product;
using Microsoft.Extensions.Configuration;
using Sa.Common.ADO.DataAccess;
using Microsoft.CodeAnalysis;
using System.Xml.Linq;
using Tenant.Query.Uitility;

namespace Tenant.Query.Repository.Product
{
    public class ProductRepository : TnBaseQueryRepository<Model.Product.ProductMaster, Context.Product.ProductContext>
    {

        #region Variable
        DataAccess _dataAccess;
        private string dbConnectionString = string.Empty;
        #endregion

        public ProductRepository(ProductContext dbContext, ILoggerFactory loggerFactory, IConfiguration configuration, DataAccess dataAccess) : base(dbContext, loggerFactory)
        {
            dbConnectionString = this.DbContext.Database.GetDbConnection().ConnectionString;
            _dataAccess = dataAccess;
        }

        public override Task<ProductMaster> GetById(string tenantId, string id)
        {
            throw new NotImplementedException();
        }

        internal async Task<List<ProductMaster>> GetProductMaster(string tenantId, PorductFilter filter)
        {
            try
            {
                //Local variable
                List<Model.Product.ProductMaster> productMasters = new List<Model.Product.ProductMaster>();

                #region Query Builder

                if (filter.IncludeReviews && filter.IncludeImages)
                {
                    productMasters = await this.DbContext.productMasters.Include("ProductImages").
                                                                            Include("ProductReviews")
                                                                            .Where(x => (!filter.IncludeInactive ? x.Active.Equals(!filter.IncludeInactive) : filter.IncludeInactive))
                                                                            .AsNoTracking().ToListAsync();
                }
                else if (filter.IncludeImages)
                {
                    productMasters = await this.DbContext.productMasters.Include("ProductImages").Where(x => (!filter.IncludeInactive ? x.Active.Equals(!filter.IncludeInactive) : filter.IncludeInactive))
                                                                            .AsNoTracking().ToListAsync();
                }
                else if (filter.IncludeReviews)
                {
                    productMasters = await this.DbContext.productMasters.Include("ProductReviews").Where(x => (!filter.IncludeInactive ? x.Active.Equals(!filter.IncludeInactive) : filter.IncludeInactive))
                                                                            .AsNoTracking().ToListAsync();
                }
                else
                {
                    productMasters = await this.DbContext.productMasters.Where(x => (!filter.IncludeInactive ? x.Active.Equals(!filter.IncludeInactive) : filter.IncludeInactive))
                                                        .AsNoTracking().ToListAsync();
                }

                // Null check
                if (productMasters == null || productMasters.Count == 0)
                {
                    this.Logger.LogWarning($"product not available for tenantId : {tenantId}");
                    throw new KeyNotFoundException($"product not available f");
                }

                return productMasters;
            }
            catch (KeyNotFoundException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        internal List<ProductCategory> GetCategory(string tenantId)
        {
            try
            {
                // method 1
                List<Model.Product.ProductCategory> productCategories = this.DbContext.productCategories
                .Where(x => x.TenantId.Equals(tenantId)).ToList();


                SqlParameter Tenant = new SqlParameter
                {
                    ParameterName = "@TenantId",
                    SqlDbType = SqlDbType.BigInt,
                    Value = Convert.ToInt64(tenantId)
                };

                // method 2
                string spName = "XC_GET_CATEGORY_TEST";
                List<Model.Product.ProductCategory> productCategories1 = this.DbContext.productCategories.FromSqlRaw($"exec {spName} @TenantId", Tenant).AsNoTracking().ToList();

                return productCategories1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetProductVerificationDataToEdit(string tenantId, string productId, string spname, string connectionstring)
        {
            try
            {
                //Logger 
                this.Logger.LogInformation($"Calling the GetProductVerificationDataToEdit for tenant : {tenantId}, product : {productId}");

                this.Logger.LogInformation($"Executing {spname} to fetch product verification item details for tenant : {tenantId}, product : {productId}");

                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    SqlCommand sqlComm = new SqlCommand(spname, conn);
                    sqlComm.Parameters.AddWithValue("@PRODUCTID", Convert.ToInt64(productId));
                    sqlComm.Parameters.AddWithValue("@TENANT_ID", Convert.ToInt64(tenantId));

                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = sqlComm;

                    da.Fill(ds);
                }
                //return reacord
                return ds;
            }
            catch (Exception ex)
            {
                this.Logger.LogError($"error while calling the GetProductVerificationDataToEdit :{ex.Message}");
                this.Logger.LogError($"Inner exception: {ex.InnerException}");
                this.Logger.LogError($"Stack trace{ex.StackTrace}");
                throw ex;
            }
        }

        /// <summary>
        /// // method 3
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="payload"></param>
        /// <param name="orderBy"></param>
        /// <param name="order"></param>
        /// <param name="spName"></param>
        /// <returns></returns>
        public List<Model.Product.SpInvoiceItemMappingResponse> GetInvoiceItemsForMapping(string tenantId, Model.Product.InvoiceItemMappingPayload payload, string orderBy, string order, string spName)
        {
            try
            {
                //logger
                //this.Logger.LogInformation($"Calling GetInvoiceItemsForMapping() - Calling stored procedure {spName} to get invoice item(s) for mapping of tenant: {tenantId}");

                DataTable vendorList = new DataTable();
                vendorList.Columns.Add("VALUE", typeof(Int64));

                DataRow dr;
                payload.Vendor.ForEach(x =>
                {
                    dr = vendorList.NewRow();
                    dr["VALUE"] = x;
                    vendorList.Rows.Add(dr);
                });

                //Executing query
                List<Model.Product.SpInvoiceItemMappingResponse> invoiceItemMappingResult = _dataAccess.ExecuteGenericList<Model.Product.SpInvoiceItemMappingResponse>(spName, Convert.ToInt64(tenantId), Convert.ToInt64(payload.LocationId), Convert.ToInt64(payload.UserId), Convert.ToInt64(payload.RoleId), Convert.ToInt32(payload.Page), Convert.ToInt32(payload.PageSize), orderBy, order, payload.Search ?? string.Empty, vendorList);

                //logger
                //this.Logger.LogInformation($"Called GetInvoiceItemsForMapping() - Called stored procedure {spName} to get invoice item(s) for mapping of tenant: {tenantId}");

                return invoiceItemMappingResult;

            }
            catch (Exception ex)
            {
                //logger
                //this.Logger.LogInformation($"Error in GetInvoiceItemsForMapping() - While calling stored procedure {spName} to get invoice item(s) for mapping of tenant: {tenantId} : {ex.Message}");
                //this.Logger.LogError($"Inner exception : {ex.InnerException}");
                //this.Logger.LogError($"Stack trace : {ex.StackTrace}");
                throw ex;
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="spName"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        internal List<Model.Product.ProductMaster> GetProductDtailsUsingSp(string tenantId, string spName, Model.Product.Search search)
        {
            try
            {
                #region Vendor
                //vendor
                DataTable subcategoryCollection = new DataTable();
                subcategoryCollection.Columns.Add("VALUE", typeof(Int64));

                if (search.SubCategory != null && search.SubCategory.Count() > 0)
                {
                    DataRow dr;
                    search.SubCategory.ForEach(x =>
                    {
                        dr = subcategoryCollection.NewRow();
                        dr["VALUE"] = Convert.ToInt64(x);
                        subcategoryCollection.Rows.Add(dr);
                    });
                }
                #endregion

                #region Category
                //Category
                DataTable categoryCollection = new DataTable();
                categoryCollection.Columns.Add("VALUE", typeof(Int64));

                if (search.Category != null && search.Category.Count() > 0)
                {
                    DataRow dr;
                    search.Category.ForEach(x =>
                    {
                        dr = categoryCollection.NewRow();
                        dr["VALUE"] = Convert.ToInt64(x);
                        categoryCollection.Rows.Add(dr);
                    });
                }
                #endregion

                //Executing query
                List<Model.Product.ProductMaster> productDetails = _dataAccess.ExecuteGenericList<Model.Product.ProductMaster>(spName, tenantId, search.isActive, categoryCollection, subcategoryCollection);

                //return product details
                return productDetails;

            }
            catch (Exception ex)
            {
                this.Logger.LogError($"Inner exception : {ex.InnerException}");
                this.Logger.LogError($"Stack trace : {ex.StackTrace}");
                throw ex;
            }
        }

        public async Task<Model.Product.ProductMaster> GetPartnerEventDetails(string RestaurantGuid, string LocationGuid, string storeProcedureName)
        {
            SqlParameter RestaurantGuidParam = Utility.PrepareParametersForStoreProcedure("@RESTAURANTGUID", SqlDbType.VarChar, string.Empty, RestaurantGuid);
            SqlParameter LocationGuidParam = Utility.PrepareParametersForStoreProcedure("@LOCATIONGUID", SqlDbType.VarChar, string.Empty, LocationGuid);
            Model.Product.ProductMaster partnerEventDetails = this.DbContext.productMasters.FromSqlRaw($"exec {storeProcedureName} @RESTAURANTGUID, @LOCATIONGUID", RestaurantGuidParam, LocationGuidParam).AsNoTracking().AsEnumerable().FirstOrDefault();
            
            return partnerEventDetails;
        }

        // only sample
        public async Task<List<Model.Product.ProductMaster>> GetStandardUoms(string spname)
        {
            try
            {
                //get standarduom list
                List<Model.Product.ProductMaster> uomstandard = _dataAccess.ExecuteGenericList<Model.Product.ProductMaster>(spname);

                return uomstandard;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
