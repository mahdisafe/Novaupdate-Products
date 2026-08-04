using System;
using System.Collections.Generic;
using System.Linq;

namespace BU50_API.BL
{
    public class Products
    {
        public string id { get; set; }
        public string Name { get; set; }

        //public DateTime date_created { get; set; }
        //public DateTime date_created_gmt { get; set; }
        //public DateTime date_modified { get; set; }
        //public DateTime date_modified_gmt { get; set; }
        //public string description { get; set; }

        //public string permalink { get; set; }
        public string sku { get; set; }

        public categories Categorie { get; set; }
        public string price { get; set; }
        public string _custom_skn { get; set; }
        public string regular_price { get; set; }
        public string sale_price { get; set; }

        public bool? on_sale { get; set; }

        //public object date_on_sale_from { get; set; }
        //public object date_on_sale_from_gmt { get; set; }
        //public object date_on_sale_to { get; set; }
        //public object date_on_sale_to_gmt { get; set; }
        //public bool on_sale { get; set; }

        public string status { get; set; }

        //public bool purchasable { get; set; }
        ////public bool virtual { get; set; }
        //public bool downloadable { get; set; }
        //public IList<object> downloads { get; set; }

        //public int download_limit { get; set; }
        //public int download_expiry { get; set; }
        //public string tax_status { get; set; }
        //public string tax_class { get; set; }
        public bool manage_stock { get; set; }

        public int? stock_quantity { get; set; }

        public Category[] Categories { get; set; }

        public string stock_status { get; set; }

        // Read-only helper for DataGridView display
        public string CategoriesText
        {
            get
            {
                if (Categories == null || Categories.Length == 0) return string.Empty;
                return string.Join(", ", Categories.Select(c => c?.Name).Where(n => !string.IsNullOrWhiteSpace(n)));
            }
        }
    }

    public class Category
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }
    }

    public class ProductSyncDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public string Barcode { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int StockQuantity { get; set; }
        public bool ManageStock { get; set; }
        public string Status { get; set; }
        public List<int> CategoryIds { get; set; } = new List<int>();
    }

    public class ProductSyncUpdateDto
    {
        public int ProductId { get; set; }
        public string Barcode { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int StockQuantity { get; set; }
        public bool ManageStock { get; set; }
        public string Status { get; set; }
    }
}