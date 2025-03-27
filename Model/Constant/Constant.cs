namespace Tenant.Query.Model.Constant
{
    public static class Constant
    {
        public const string SA_GET_PRODUCTS_FOR_CATEGORY_MAPPING = "SA_GET_PRODUCTS_FOR_CATEGORY_MAPPING";

        public const string VendorName = "VendorName";

        public const string ASC = "asc";

        //Stock
        public const string Product_In_stock = "In stock";
        public const string Product_Out_of_stock = "Out of stock";


        public static class StoredProcedures
        {
            public const string SA_REALTIMEOCR_ADD_INVOICE = "[dbo].[SA_REALTIMEOCR_ADD_INVOICE]";
            public const string SA_REALTIMEOCR_GET_TENANT_VENDOR_DETAIL = "[dbo].[SA_REALTIMEOCR_GET_TENANT_VENDOR_DETAIL]";
            public const string SA_REALTIMEOCR_VALIDATE_VENDOR = "[dbo].[SA_REALTIMEOCR_VALIDATE_VENDOR]";
            public const string SP_ADD_IMAGES = "[dbo].[XC_ADD_IMAGE]";
        }
    }
}
