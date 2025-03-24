using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tenant.Query.Model.Response
{
    public class Category
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("category")]
        public string Name { get; set; }        

        [JsonProperty("order")]
        public long Order { get; set; }

        [JsonProperty("subCategory")]
        public List<Model.Response.SubCategory> subCategories { get; set; }
    }
}
