using Newtonsoft.Json;
using System.Collections.Generic;

namespace Tenant.Query.Model.Product
{
    public class InvoiceItemMappingPayload
    {
        public InvoiceItemMappingPayload()
        {
            Page = 1;
            PageSize = 10;
        }

        [JsonProperty("locationId")]
        public string LocationId { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("roleId")]
        public string RoleId { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        [JsonProperty("orderBy")]
        public string OrderBy { get; set; }

        [JsonProperty("search")]
        public string Search { get; set; }

        [JsonProperty("vendor")]
        public List<long> Vendor { get; set; }
    }
}
