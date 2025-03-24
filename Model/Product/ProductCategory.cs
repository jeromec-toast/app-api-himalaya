using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Tenant.API.Base.Core;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace Tenant.Query.Model.Product
{
    [Table("ProductCategory")]
    public class ProductCategory : TnBase
    {
        [JsonProperty("Category"), Column("Category")]
        public string Category { get; set; }

        [JsonProperty("subCategory"), Column("SubCategory")]
        public long SubCategory { get; set; }

        [JsonProperty("active"), Column("Active")]
        public bool Active { get; set; }

        [JsonIgnore, NotMapped]
        public override string Guid { get; set; }

        [JsonIgnore, NotMapped]
        public override DateTime Created { get; set; }

        [JsonIgnore, NotMapped]
        public override string CreatedBy { get; set; }

        [JsonIgnore, NotMapped]
        public override DateTime LastModified { get; set; }

        [JsonIgnore, NotMapped]
        public override string LastModifiedBy { get; set; }

        [JsonProperty("tenantId"), Column("TenantId")]
        public override string TenantId { get; set; }
    }
}
