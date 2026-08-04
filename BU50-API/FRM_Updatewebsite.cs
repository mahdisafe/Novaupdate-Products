using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BU50_API.BL;
using BU50_API.Properties;
using COMPLib;
using FOCUSAPILib;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Diagnostics;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Collections.Concurrent;
using System.Globalization;
using System.Configuration;
using System.Runtime.InteropServices;
using System.IO;

namespace BU50_API
{
    public partial class FRM_Updatewebsite : Form
    {
        private static int muincresi = 1;

        private readonly DataTable Dtproducts = new DataTable();
        private readonly string id = Settings.Default.UserId;
        private readonly int number = Settings.Default.pagenumbering;
        private readonly string pass = Settings.Default.Password;
        private readonly string pass2 = Settings.Default.password2;
        private readonly string Servername = Settings.Default.ServerName;
        private readonly string ServerNameExternal = Settings.Default.ServerNameExternal;
        private readonly StringBuilder st = new StringBuilder();


        public SqlConnection con;
        public SqlConnection cons;
        public string DataBase = Settings.Default.DataBase;
        public string db = Settings.Default.DB;
        private bool ischeck;
        private int ist;
        private string MasterId;

        private List<Products> or;
        private string ProductId;
        private string Productname;

        // Accumulator for batch updates
        private readonly List<ProductSyncUpdateDto> _pendingBatch = new List<ProductSyncUpdateDto>();
        private readonly List<string> _batchFailureLog = new List<string>();
        private readonly int _batchSize = ReadIntConfig("BatchSize", 100);

        private static int ReadIntConfig(string key, int defaultValue)
        {
            return int.TryParse(ConfigurationManager.AppSettings[key], out var value) ? value : defaultValue;
        }


        // Stopwatches to track durations
        private readonly Stopwatch _fetchSw = new Stopwatch();
        private readonly Stopwatch _updateSw = new Stopwatch();
        private CancellationTokenSource _skuSearchCts;
        private CancellationTokenSource _fullUpdateCts;
        private bool _autoUpdateStarted;

        public FRM_Updatewebsite()
        {
            InitializeComponent();


            if (Settings.Default.CompCode == "")
            {
                var frm = new FRM_Config();
                frm.ShowDialog();
            }

            con = new SqlConnection(
                $@"Server={Servername};Database={DataBase};Integrated Security=false;User ID={id};Password={pass}");


            cons = new SqlConnection(
                $@"Server={ServerNameExternal};Database={db};Integrated Security=false;User ID={id};Password={pass2}");


            try
            {
                con.Open();
                //JobManager.Initialize();

                //object sender = null;
                //EventArgs e = null;
                //JobManager.AddJob(() => btnupdate_Click(sender, e), s => s.ToRunEvery(300).Seconds()
                //); 

                #region iniFocust

                sCompCode = Settings.Default.CompCode;
                seqId = 0;

                cd = new CompanyDetails();
                fm = new FMaster();
                cmp = new Init();
                ft = new Transaction();
                fr = new FReport();
                frs = new FRateMaster();

                cmp.InitComp(sCompCode);
                cd.Open(0);
                var companyName = cd.CompanyName;

                #endregion
            }
            catch (Exception)
            {
                MessageBox.Show(@"تأكد من الاتصال للسيرفر", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Application.ExitThread();
            }
        }

 
        //Add a prepared update object to the batch (deduped by Woo product id within pending batch)
        private void AddUpdateToBatch(string productIdStr, double wqty, double saleingprice, decimal schemeRateWithVat, string skn, bool trackStock)
        {
            if (!int.TryParse(productIdStr, out var productId))
            {
                UpdateProduct(productIdStr, wqty, saleingprice / 1.15, skn, trackStock, schemeRateWithVat);
                return;
            }

            var updateObj = new ProductSyncUpdateDto
            {
                ProductId = productId,
                Barcode = skn ?? string.Empty,
                Price = saleingprice > 0 ? (decimal)saleingprice : 0m,
                DiscountPrice = schemeRateWithVat > 0 ? (decimal?)schemeRateWithVat : null,
                StockQuantity = trackStock ? Convert.ToInt32(Math.Round(wqty)) : 0,
                ManageStock = trackStock,
                Status = (saleingprice <= 0) ? "draft" : "published"
            };

            var existingIdx = _pendingBatch.FindIndex(d => d.ProductId == productId);
            if (existingIdx >= 0)
                _pendingBatch[existingIdx] = updateObj;
            else
                _pendingBatch.Add(updateObj);
        }


        // WooCommerce API helpers removed as they are no longer needed for direct DB synchronization.

        //Send current batch to NovaShopDB
        //Send current batch to NovaShop via API
        private async Task SendBatchUpdatesAsync()
        {
            if (_pendingBatch.Count == 0) return;

            var batchCopy = new List<ProductSyncUpdateDto>(_pendingBatch);
            _pendingBatch.Clear();

            try
            {
                var baseUrl = BU50_API.Properties.Settings.Default.WebsiteUrl;
                if (!baseUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && !baseUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    baseUrl = "http://" + baseUrl;
                }
                baseUrl = baseUrl.TrimEnd('/');
                if (baseUrl.Contains("efifty.com"))
                {
                    if (!baseUrl.Contains("www.efifty.com"))
                    {
                        baseUrl = baseUrl.Replace("efifty.com", "www.efifty.com");
                    }
                    if (baseUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                    {
                        baseUrl = "https://" + baseUrl.Substring(7);
                    }
                }

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("X-Api-Key", Settings.Default.ApiKey);

                    var jsonPayload = JsonConvert.SerializeObject(batchCopy);
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync($"{baseUrl}/api/admin/products/bulk-sync", content);
                    response.EnsureSuccessStatusCode();
                }

                Console.WriteLine($"Successfully updated {batchCopy.Count} products in NovaShop via API.");
            }
            catch (Exception ex)
            {
                _batchFailureLog.Add($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | batch API update error | count={batchCopy.Count} | {ex.Message}");
                Console.WriteLine($"Batch update API error: {ex.Message}");
            }
        }

        private async Task<List<Products>> FetchAllProductsAsync(CancellationToken cancellationToken = default)
        {
            _fetchSw.Restart();
            var filteredProducts = new List<Products>();

            try
            {
                lblStatus.Text = @"جاري تحميل المنتجات من الموقع...";
                lblTotalProducts.Text = @"0";
                progressBar1.Visible = true;
                progressBar1.Style = ProgressBarStyle.Marquee;

                var skuFilter = (txtsku?.Text ?? string.Empty).Trim();
                var baseUrl = BU50_API.Properties.Settings.Default.WebsiteUrl;
                if (!baseUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && !baseUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    baseUrl = "http://" + baseUrl;
                }
                baseUrl = baseUrl.TrimEnd('/');
                if (baseUrl.Contains("efifty.com"))
                {
                    if (!baseUrl.Contains("www.efifty.com"))
                    {
                        baseUrl = baseUrl.Replace("efifty.com", "www.efifty.com");
                    }
                    if (baseUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                    {
                        baseUrl = "https://" + baseUrl.Substring(7);
                    }
                }

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("X-Api-Key", Settings.Default.ApiKey);

                    var response = await client.GetAsync($"{baseUrl}/api/admin/products/sync-list", cancellationToken);
                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();
                    var jObj = JObject.Parse(json);
                    var syncList = jObj["data"]?.ToObject<List<ProductSyncDto>>();

                    if (syncList != null)
                    {
                        foreach (var item in syncList)
                        {
                            var categoriesList = new List<BU50_API.BL.Category>();
                            if (item.CategoryIds != null)
                            {
                                foreach (var catId in item.CategoryIds)
                                {
                                    categoriesList.Add(new BU50_API.BL.Category
                                    {
                                        Id = catId,
                                        Name = "Category",
                                        Slug = "category"
                                    });
                                }
                            }

                            var p = new Products
                            {
                                id = item.ProductId.ToString(),
                                Name = item.Name ?? string.Empty,
                                sku = item.SKU ?? string.Empty,
                                _custom_skn = item.Barcode ?? string.Empty,
                                price = item.Price.ToString(CultureInfo.InvariantCulture),
                                regular_price = item.Price.ToString(CultureInfo.InvariantCulture),
                                sale_price = item.DiscountPrice?.ToString(CultureInfo.InvariantCulture) ?? string.Empty,
                                stock_quantity = item.StockQuantity,
                                manage_stock = item.ManageStock,
                                status = item.Status ?? string.Empty,
                                on_sale = item.DiscountPrice.HasValue && item.DiscountPrice.Value > 0,
                                Categories = categoriesList.ToArray()
                            };

                            if (!string.IsNullOrWhiteSpace(skuFilter))
                            {
                                if (!p.sku.Contains(skuFilter, StringComparison.OrdinalIgnoreCase) &&
                                    !p._custom_skn.Contains(skuFilter, StringComparison.OrdinalIgnoreCase) &&
                                    !p.Name.Contains(skuFilter, StringComparison.OrdinalIgnoreCase))
                                {
                                    continue;
                                }
                            }

                            filteredProducts.Add(p);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"خطأ أثناء تحميل المنتجات: {ex.Message}");
                MessageBox.Show($"خطأ أثناء تحميل المنتجات: {ex.Message}");
            }
            finally
            {
                _fetchSw.Stop();
                lblStatus.Text = $"تم تحميل {filteredProducts.Count} منتج في {_fetchSw.Elapsed:mm\\:ss}.";
                progressBar1.Visible = false;
                progressBar1.Style = ProgressBarStyle.Continuous;
            }

            lblProducts.Text = filteredProducts.Count.ToString();
            return filteredProducts;
        }


        private async Task LoadProductsIntoDataGridViewAsync(CancellationToken cancellationToken = default)
        {
            var products = await FetchAllProductsAsync(cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            // Bind products to DataGridView
            grdproduct.AutoGenerateColumns = false; // we will define columns explicitly
            grdproduct.Columns.Clear();

            // Define columns explicitly for better control/order
            grdproduct.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "المعرف",
                DataPropertyName = "id",
                Name = "colId",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            grdproduct.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "الاسم",
                DataPropertyName = "Name",
                Name = "colName",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            grdproduct.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "SKU",
                DataPropertyName = "sku",
                Name = "colSku",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            grdproduct.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "التصنيف",
                DataPropertyName = "CategoriesText",
                Name = "colCategories",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            grdproduct.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "السعر (Price)",
                DataPropertyName = "price",
                Name = "colPrice",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            grdproduct.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "سعر البيع (Regular)",
                DataPropertyName = "regular_price",
                Name = "colRegular",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            grdproduct.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "سعر العرض (Sale)",
                DataPropertyName = "sale_price",
                Name = "colSale",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            grdproduct.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "المخزون",
                DataPropertyName = "stock_quantity",
                Name = "colStock",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            grdproduct.DataSource = products;
            lblTotalProducts.Text = products.Count.ToString();
            lblProducts.Text = products.Count.ToString();
            lblStatus.Text = $"تم تحميل {products.Count} منتج — يبدأ التحديث...";

            // Start the update process only after all products are loaded
            if (products.Count > 0)
            {
                var debugSingle = !string.IsNullOrWhiteSpace((txtsku?.Text ?? string.Empty).Trim());
                await UpdateUIAfterProductsLoaded(products, debugSingle, cancellationToken);
            }
        }

        // Lightweight Focus product info for prefetch
        private sealed class FocusProductInfo
        {
            public int MasterId { get; set; }
            public string Name { get; set; }
            public string BarcodeYH { get; set; }
        }

        // Prefetch Focus mapping for a list of MasterIds in chunks to reduce per-item SQL calls
        private Dictionary<int, FocusProductInfo> LoadFocusProductInfoMap(SqlConnection sql, IEnumerable<int> masterIds)
        {
            var result = new Dictionary<int, FocusProductInfo>();
            var ids = masterIds?.Distinct().ToList() ?? new List<int>();
            if (ids.Count == 0) return result;

            const int chunk = 500; // SQL parameter limit safe chunk
            for (int i = 0; i < ids.Count; i += chunk)
            {
                var slice = ids.Skip(i).Take(chunk).ToList();
                var sb = new StringBuilder();
                // Qualify columns and filter by mr001.MasterId (u0001 uses ExtraId)
                sb.Append("SELECT mr001.MasterId, mr001.Name, u0001.BarcodeYH FROM u0001 INNER JOIN mr001 ON u0001.ExtraId = mr001.MasterId WHERE mr001.MasterId IN (");
                for (int p = 0; p < slice.Count; p++)
                {
                    if (p > 0) sb.Append(',');
                    sb.Append("@p").Append(p);
                }
                sb.Append(")");

                using (var cmd = new SqlCommand(sb.ToString(), sql))
                {
                    for (int p = 0; p < slice.Count; p++)
                    {
                        cmd.Parameters.AddWithValue("@p" + p, slice[p]);
                    }
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var mid = reader.GetInt32(0);
                            result[mid] = new FocusProductInfo
                            {
                                MasterId = mid,
                                Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                BarcodeYH = reader.IsDBNull(2) ? string.Empty : reader.GetString(2)
                            };
                        }
                    }
                }
            }
            return result;
        }

        // Prefetch Focus mapping by BarcodeYH (Woo SKU) for products with empty/non-numeric _custom_skn
        private Dictionary<string, FocusProductInfo> LoadFocusProductInfoByBarcodeMap(SqlConnection sql, IEnumerable<string> skus)
        {
            var result = new Dictionary<string, FocusProductInfo>(StringComparer.OrdinalIgnoreCase);
            var values = (skus ?? Enumerable.Empty<string>())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
            if (values.Count == 0) return result;

            const int chunk = 400; // keep SQL params in safe range
            for (int i = 0; i < values.Count; i += chunk)
            {
                var slice = values.Skip(i).Take(chunk).ToList();
                var sb = new StringBuilder();
                sb.Append("SELECT mr001.MasterId, mr001.Name, u0001.BarcodeYH FROM u0001 INNER JOIN mr001 ON u0001.ExtraId = mr001.MasterId WHERE u0001.BarcodeYH IN (");
                for (int p = 0; p < slice.Count; p++)
                {
                    if (p > 0) sb.Append(',');
                    sb.Append("@b").Append(p);
                }
                sb.Append(")");

                using (var cmd = new SqlCommand(sb.ToString(), sql))
                {
                    for (int p = 0; p < slice.Count; p++)
                    {
                        cmd.Parameters.AddWithValue("@b" + p, slice[p]);
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var barcode = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                            if (string.IsNullOrWhiteSpace(barcode)) continue;
                            barcode = barcode.Trim();
                            result[barcode] = new FocusProductInfo
                            {
                                MasterId = reader.GetInt32(0),
                                Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                BarcodeYH = barcode
                            };
                        }
                    }
                }
            }

            return result;
        }

        // Faster stock processor without DataRow dependency
        private List<string> ProcessStockFast(int masterId, string name, string barcodeYH, string productId, ref double pQty, ref double avgVal)
        {
            var storeRules = new List<(int StoreId, string StoreName, int MinBuffer)>
            {
                (354157, "Online Store Hassa", 0),
                (111, "MAIN STORE-OK", 0),
                (5, "Maisam store", 1),
                (24, "HOFUFSTORE-OK", 0),
                (91, "Zahran Shop", 1),
                (354177, "FRDUSE", 1),
                (89, "Techno 60ST", 2),
                (92, "Khaldia Store", 0),
                (20, "Qatef Store", 1),
                (16, "Ali2 Shop", 1),
                (127, "Mjbmega Store", 1),
                (354174, "Ryadh", 0),
                (1114, "H Mega Hasa", 1),
                (14, "Usra-OK", 1),
                (354178, "Khbar Store", 1),
                (7398, "Menezla Store", 2),
                (125, "kulibyah Store", 1),
                (123, "rashdia  wh", 1),
                (123, "faisalya  store", 1),
            };

            double totalPQty = 0;
            var wh = new List<string>();
            foreach (var rule in storeRules)
            {
                double storePQty = 0;
                double storeAvgVal = 0;
                fr.GetStockOn(masterId, DateTime.Now.Date, rule.StoreId, ref storePQty, ref storeAvgVal);
                if (storePQty >= rule.MinBuffer)
                {
                    var usable = Math.Max(0, storePQty - rule.MinBuffer);
                    if (usable > 0)
                    {
                        totalPQty += usable;
                        wh.Add(rule.StoreName);
                    }
                }
            }
            pQty = totalPQty;
            return wh;
        }

        // Compare current site values with new values to skip unchanged updates
        private static bool ShouldIncludeUpdate(Products p, double newQty, double newRegularWithVat, decimal newSaleWithVat)
        {
            try
            {
                // stock
                var curQty = p.stock_quantity ?? 0;
                if (Math.Abs(curQty - newQty) > 0.01) return true;

                // prices are strings; parse if possible
                double curReg = 0; double.TryParse(p.regular_price, NumberStyles.Any, CultureInfo.InvariantCulture, out curReg);
                double curSale = 0; double.TryParse(p.sale_price, NumberStyles.Any, CultureInfo.InvariantCulture, out curSale);
                var ns = (double)newSaleWithVat;
                // allow small formatting differences (0.1 SAR)
                if (Math.Abs(curReg - newRegularWithVat) > 0.1) return true;
                if (Math.Abs(curSale - ns) > 0.1) return true;

                // Site has no sale price but Focus has an active scheme offer
                if (ns > 0 && string.IsNullOrWhiteSpace(p.sale_price)) return true;

                // API reports not on sale while Focus has an active scheme (sale_price missing/stale)
                if (ns > 0 && p.on_sale == false) return true;

                return false;
            }
            catch { return true; }
        }

        // Update UI after products are loaded
        private async Task UpdateUIAfterProductsLoaded(List<Products> products, bool debugSingle = false, CancellationToken cancellationToken = default)
        {
            try
            {
                var zeroPriceProducts = new List<string>();
                var schemeDiagnostics = new List<string>();
                _batchFailureLog.Clear();
                _updateSw.Restart();
                lblProducts.Text = products.Count.ToString();
                progressBar2.Maximum = products.Count;
                progressBar2.Value = 0;

                if (products.Count > 0)
                {
                    using (var con = new SqlConnection(
                               $@"Server={Servername};Database={DataBase};User ID={id};Password={pass};"))
                    using (var externalCon = new SqlConnection($@"Server={ServerNameExternal};Database={db};User ID={id};Password={pass2};"))
                    {
                        con.Open();
                        externalCon.Open();
                        using (var insertCmd = CreateUpdatedProductsInsertCommand(externalCon))
                        {

                        // Prefetch Focus mapping once in chunks
                        var numericSkus = products.Select(p =>
                        {
                            int x; return int.TryParse(p._custom_skn, out x) ? (int?)x : null;
                        }).Where(x => x.HasValue).Select(x => x.Value).Distinct().ToList();
                        var focusMap = LoadFocusProductInfoMap(con, numericSkus);
                        var emptySknSkus = products
                            .Where(p => !int.TryParse(p._custom_skn, out _))
                            .Select(p => p.sku)
                            .Where(s => !string.IsNullOrWhiteSpace(s))
                            .Distinct(StringComparer.OrdinalIgnoreCase)
                            .ToList();
                        var focusByBarcodeMap = LoadFocusProductInfoByBarcodeMap(con, emptySknSkus);

                        for (var i = 0; i < products.Count; i++)
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            // Keep the UI responsive so txtsku accepts keyboard input during long syncs.
                            if (i % 3 == 0) await Task.Yield();

                            var product = products[i];

                            // Skip excluded categories early to avoid costly COM/SQL calls
                            if (IsExcludedByCategory(product))
                            {
                                progressBar2.Value = Math.Min(progressBar2.Maximum, i + 1);
                                continue;
                            }

                            // Throttle UI highlight to reduce overhead; never steal focus while user types in txtsku
                            if (i % 5 == 0 && !ShouldSkipGridUiUpdates())
                            {
                                try
                                {
                                    grdproduct.ClearSelection();
                                    if (i >= 0 && i < grdproduct.Rows.Count)
                                    {
                                        var rowToSelect = grdproduct.Rows[i];
                                        rowToSelect.Selected = true;
                                        grdproduct.CurrentCell = rowToSelect.Cells[0];
                                        rowToSelect.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                                        rowToSelect.DefaultCellStyle.ForeColor = Color.Black;
                                    }
                                }
                                catch { }

                                // ETA
                                var done = i;
                                var total = products.Count;
                                if (done > 0)
                                {
                                    var elapsed = _updateSw.Elapsed;
                                    var perItem = TimeSpan.FromTicks(elapsed.Ticks / done);
                                    var remaining = total - done;
                                    var eta = TimeSpan.FromTicks(perItem.Ticks * remaining);
                                    lblStatus.Text = $"يتم التحديث... اكتمل {done}/{total}. الوقت المنقضي {elapsed:mm\\:ss}، المتوقع {eta:mm\\:ss}";
                                }

                                lblcurrentRow.Text = i.ToString();
                                lblcurrentRow.Update();
                            }

                            FocusProductInfo info = null;
                            int masterId;
                            if (int.TryParse(product._custom_skn, out masterId))
                            {
                                if (!focusMap.TryGetValue(masterId, out info))
                                    continue; // numeric _custom_skn but not found in Focus
                            }
                            else
                            {
                                // _custom_skn is empty/non-numeric on website: try resolving from current system SKU (BarcodeYH)
                                var skuKey = (product.sku ?? string.Empty).Trim();
                                if (string.IsNullOrWhiteSpace(skuKey) || !focusByBarcodeMap.TryGetValue(skuKey, out info))
                                    continue;
                                masterId = info.MasterId;
                            }

                            // Update product info from Focus
                            product._custom_skn = info.MasterId.ToString();
                            product.Name = info.Name;

                            // Respect Woo manage_stock early
                            bool trackStock = product.manage_stock;

                            // Price from FRateMaster (reuse COM object to avoid per-item construction overhead)
                            var r = frs.Open(1, 0, masterId, DateTime.Now);
                            if (r == null)
                            {
                                await UpdateProductChangeStatus(product.id);
                                continue;
                            }
                            var saleingprice = frs.GetRate(0); // base price
                            frs.Close();
                            if (saleingprice <= 0)
                            {
                                zeroPriceProducts.Add($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | sku={info.BarcodeYH} | masterId={masterId} | name={info.Name}");
                            }

                            double pQty = 0; double avgVal = 0;
                            List<string> warehouses = null;
                            if (trackStock)
                            {
                                warehouses = ProcessStockFast(masterId, info.Name, info.BarcodeYH, product.id, ref pQty, ref avgVal);
                            }

                            // Prepare values for site and compare with current
                            var vat = 1.15;
                            var regularWithVat = saleingprice * vat;
                            var scheme = GetSchemeDetails(con, product._custom_skn);
                            var schemeRateWithVat = scheme.Item1 * (decimal)vat;
                            if (scheme.Item1 <= 0)
                            {
                                schemeDiagnostics.Add($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | sku={info.BarcodeYH} | masterId={masterId} | name={info.Name} | reason={scheme.Item2}");
                            }
                            else if (regularWithVat > 0 && (double)schemeRateWithVat >= regularWithVat - 0.1)
                            {
                                schemeDiagnostics.Add($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | sku={info.BarcodeYH} | masterId={masterId} | name={info.Name} | reason=sale>={regularWithVat:F1} (schemeRate={scheme.Item1}, schemeWithVat={schemeRateWithVat:F1}, regularWithVat={regularWithVat:F1}, schemeName={scheme.Item2})");
                                schemeRateWithVat = 0; // Woo rejects sale_price >= regular_price
                            }

                            var warehouseStr = (trackStock && warehouses != null && warehouses.Count > 0) ? string.Join(", ", warehouses) : null;

                            // Save record with regular_price and SchemeName and saleprice and warehouse
                            UpdatetableRecord(insertCmd, info.BarcodeYH, pQty.ToString(CultureInfo.InvariantCulture), product.id, info.Name, info.MasterId.ToString(),
                                regularWithVat.ToString("F1", CultureInfo.InvariantCulture), scheme.Item2,
                                schemeRateWithVat > 0 ? schemeRateWithVat.ToString("F1", CultureInfo.InvariantCulture) : null,
                                warehouseStr);

                            if (ShouldIncludeUpdate(product, pQty, regularWithVat, schemeRateWithVat))
                            {
                                AddUpdateToBatch(product.id, trackStock ? pQty : 0, regularWithVat, schemeRateWithVat, product._custom_skn, trackStock);
                            }

                            // Flush batch every _batchSize products
                            if (_pendingBatch.Count >= _batchSize)
                            {
                                await SendBatchUpdatesAsync();
                            }

                            progressBar2.Value = i + 1;
                            if (i % 5 == 0 && !ShouldSkipGridUiUpdates())
                            {
                                try
                                {
                                    if (i >= 0 && i < grdproduct.Rows.Count)
                                    {
                                        var processedRow = grdproduct.Rows[i];
                                        processedRow.DefaultCellStyle.BackColor = Color.White;
                                        processedRow.DefaultCellStyle.ForeColor = Color.Black;
                                    }
                                }
                                catch { }
                            }
                        }
                        }
                    }

                    // Flush any remaining updates
                    await SendBatchUpdatesAsync();

                    if (schemeDiagnostics.Count > 0)
                    {
                        try
                        {
                            var logPath = Path.Combine(Application.StartupPath, $"scheme-sale-diagnostic-{DateTime.Now:yyyyMMdd-HHmmss}.log");
                            var lines = new List<string>
                            {
                                $"Scheme/sale price diagnostic report - {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                                $"Total products without valid sale price: {schemeDiagnostics.Count}",
                                "format: timestamp | sku | masterId | name | reason",
                                "------------------------------------------------------------"
                            };
                            lines.AddRange(schemeDiagnostics);
                            File.WriteAllLines(logPath, lines, Encoding.UTF8);
                            Console.WriteLine($"Scheme sale report saved: {logPath}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Failed to write scheme sale report: " + ex.Message);
                        }
                    }

                    if (zeroPriceProducts.Count > 0)
                    {
                        try
                        {
                            var logPath = Path.Combine(Application.StartupPath, $"zero-price-products-{DateTime.Now:yyyyMMdd-HHmmss}.log");
                            var lines = new List<string>
                            {
                                $"Zero price diagnostic report - {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                                $"Total products with Focus base price <= 0: {zeroPriceProducts.Count}",
                                "format: timestamp | sku | masterId | name",
                                "------------------------------------------------------------"
                            };
                            lines.AddRange(zeroPriceProducts);
                            File.WriteAllLines(logPath, lines, Encoding.UTF8);
                            Console.WriteLine($"Zero price report saved: {logPath}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Failed to write zero price report: " + ex.Message);
                        }
                    }

                    if (_batchFailureLog.Count > 0)
                    {
                        try
                        {
                            var logPath = Path.Combine(Application.StartupPath, $"batch-update-failures-{DateTime.Now:yyyyMMdd-HHmmss}.log");
                            var lines = new List<string>
                            {
                                $"WooCommerce batch/PUT failure report - {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                                $"Total failures/retries logged: {_batchFailureLog.Count}",
                                "------------------------------------------------------------"
                            };
                            lines.AddRange(_batchFailureLog);
                            File.WriteAllLines(logPath, lines, Encoding.UTF8);
                            Console.WriteLine($"Batch failure report saved: {logPath}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Failed to write batch failure report: " + ex.Message);
                        }
                    }

                    if (!debugSingle)
                    {
                        #region SendNoti
                        var hubConnection = new HubConnection("https://www.b50.app/");
                        var hubProxy = hubConnection.CreateHubProxy("NewOrder");
                        try
                        {
                            await hubConnection.Start();
                            await hubProxy.Invoke("SendNotification", $"Products Update has been done with Total {products.Count}");
                            Application.ExitThread();
                            Application.Exit();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Failed to send notification: " + ex.Message);
                        }
                        finally
                        {
                            hubConnection.Stop();
                        }
                        #endregion
                    }

                    _updateSw.Stop();
                    lblStatus.Text = $"اكتمل التحديث في {_updateSw.Elapsed:mm\\:ss}.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }


        public void SendToAdmin(string Mobile, string Content)
        {
            var taqnyt = new Taqnyat.Taqnyat();
            var bearer = "a4b09355b53a23b9b5fba5d2e415ed94";
            var sender2 = "BUKHMSEN";

            var message = taqnyt.SendMessage(bearer, Mobile, sender2, Content);

            Console.WriteLine(message);
        }


        public async Task getproducts(int? number)
        {
            _fullUpdateCts?.Cancel();
            _fullUpdateCts?.Dispose();
            _fullUpdateCts = new CancellationTokenSource();
            var token = _fullUpdateCts.Token;

            try
            {
                await LoadProductsIntoDataGridViewAsync(token);
            }
            catch (OperationCanceledException)
            {
                lblStatus.Text = @"تم إيقاف التحديث.";
            }
        }

        private async void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                await getproducts(Settings.Default.pagenumbering > 0 ? Settings.Default.pagenumbering : 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void ProcessStock(DataRow row, ref double PQty, ref double AvgVal, string productId)
        {
            // Ordered store rules with names and minimum buffer
            var storeRules = new List<(int StoreId, string StoreName, int MinBuffer)>
            {
                (354157, "Online Store Hassa", 0),
                (111, "MAIN STORE-OK", 0),
                (5, "Maisam store", 2),
                (24, "HOFUFSTORE-OK", 1),
                (91, "Zahran Shop", 2),
                (354177, "FRDUSE", 2),
                (89, "Techno 60ST", 2),
                (92, "Khaldia Store", 1),
                (20, "Qatef Store", 2),
                (16, "Ali2 Shop", 2),
                (127, "Mjbmega Store", 2),
                (354174, "Ryadh", 1),
                (1114, "H Mega Hasa", 2),
                (14, "Usra-OK", 2),
                (354178, "Khbar Store", 2),
                (7398, "Menezla Store", 2)
            };

            // Ensure that MasterId is correctly assigned
            var masterId = row["MasterId"].ToString();

            double totalPQty = 0;

            foreach (var rule in storeRules)
            {
                double storePQty = 0;
                double storeAvgVal = 0;

                // Get stock information for the current store
                fr.GetStockOn(Convert.ToInt32(masterId), DateTime.Now.Date, rule.StoreId, ref storePQty, ref storeAvgVal);

                if (storePQty >= rule.MinBuffer)
                {
                    totalPQty += Math.Max(0, storePQty - rule.MinBuffer);
                }
            }

            // Update the reference parameter PQty with the total adjusted quantity
            PQty = totalPQty;
            // Note: record insert moved to caller where we know prices/scheme
        }


        // Helper: check available qty on FOCUS by SKN (MasterId)
        public bool HasSufficientQty(string skn, double requestedQty, out double availableQty)
        {
            availableQty = 0;
            if (!int.TryParse(skn, out var masterId)) return false;
            availableQty = ComputeAvailableQty(masterId);
            return availableQty >= requestedQty;
        }

        // Compute total available quantity across stores minus min buffer
        private double ComputeAvailableQty(int masterId)
        {
            // Store rules: (StoreId, MinQtyBuffer)
            var storeRules = new List<Tuple<int, int>>
            {
                Tuple.Create(354157, 0), // Online Store Hassa
                Tuple.Create(111, 0),    // MAIN STORE-OK
                Tuple.Create(5, 2),
                Tuple.Create(24, 1),
                Tuple.Create(91, 2),
                Tuple.Create(354177, 2),
                Tuple.Create(89, 2),
                Tuple.Create(92, 1),
                Tuple.Create(20, 2),
                Tuple.Create(16, 2),
                Tuple.Create(127, 2),
                Tuple.Create(354174, 1),
                Tuple.Create(1114, 2),
                Tuple.Create(14, 2),
                Tuple.Create(354178, 2),
                Tuple.Create(7398, 2)
            };

            double total = 0;
            foreach (var rule in storeRules)
            {
                var storeId = rule.Item1;
                var minBuffer = rule.Item2;
                double qty = 0;
                double avg = 0;
                try
                {
                    fr.GetStockOn(masterId, DateTime.Now.Date, storeId, ref qty, ref avg);
                    var usable = qty - minBuffer;
                    if (usable > 0) total += usable;
                }
                catch
                {
                    // ignore store errors and continue
                }
            }
            return total;
        }


        // Synchronous version kept for fallback (when product ID is not numeric)
        public void UpdateProduct(string ProductId, double wqty, double _saleingprice, string SKN, bool trackStock = true, decimal schemeRateWithVat = -1)
        {
            var vat = 1.15;
            var saleingprice = _saleingprice * vat;
            decimal rateWithVat;
            if (schemeRateWithVat >= 0)
            {
                rateWithVat = schemeRateWithVat;
            }
            else
            {
                var result1 = GetSchemeDetails(con, SKN);
                rateWithVat = result1.Item1 * (decimal)vat;
            }

            try
            {
                using (var connection = new SqlConnection($@"Server={ServerNameExternal};Database=NovaShopDB;Integrated Security=false;User ID={id};Password={pass2};Encrypt=True;TrustServerCertificate=True"))
                {
                    connection.Open();

                    decimal finalPrice = (decimal)saleingprice;
                    if (rateWithVat > 0 && rateWithVat < finalPrice)
                    {
                        finalPrice = rateWithVat;
                    }

                    var query = @"
                        UPDATE Products
                        SET Price = @Price,
                            DiscountPrice = @DiscountPrice,
                            FinalPrice = @FinalPrice,
                            StockQuantity = @StockQuantity,
                            ManageStock = @ManageStock,
                            Barcode = @Barcode,
                            UpdatedAt = @UpdatedAt
                        WHERE ProductId = @ProductId";

                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ProductId", Convert.ToInt32(ProductId));
                        cmd.Parameters.AddWithValue("@Price", saleingprice);
                        cmd.Parameters.AddWithValue("@DiscountPrice", rateWithVat > 0 ? (object)rateWithVat : DBNull.Value);
                        cmd.Parameters.AddWithValue("@FinalPrice", finalPrice);
                        cmd.Parameters.AddWithValue("@StockQuantity", trackStock ? wqty : 0);
                        cmd.Parameters.AddWithValue("@ManageStock", trackStock);
                        cmd.Parameters.AddWithValue("@Barcode", SKN ?? string.Empty);
                        cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.UtcNow);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the product: {ex.Message}");
            }
        }

        public async Task UpdateProductChangeStatus(string ProductId)
        {
            try
            {
                using (var connection = new SqlConnection($@"Server={ServerNameExternal};Database=NovaShopDB;Integrated Security=false;User ID={id};Password={pass2};Encrypt=True;TrustServerCertificate=True"))
                {
                    await connection.OpenAsync();
                    var query = @"
                        UPDATE Products
                        SET Status = 'draft',
                            IsActive = 0,
                            UpdatedAt = @UpdatedAt
                        WHERE ProductId = @ProductId";

                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ProductId", Convert.ToInt32(ProductId));
                        cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.UtcNow);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while setting product status to draft: {ex.Message}");
            }
        }

        private SqlCommand CreateUpdatedProductsInsertCommand(SqlConnection externalConnection)
        {
            var cmdds = new SqlCommand(
                "INSERT INTO UpdatedProducts (SKU, QTY, isdate, Productid, name, time, SKN, regular_price, SchemeName, saleprice, warehouse) " +
                "VALUES (@SKU, @QTY, @isdate, @Productid, @name, @time, @SKN, @regular_price, @SchemeName, @saleprice, @warehouse)",
                externalConnection);

            cmdds.Parameters.Add(new SqlParameter("@SKU", SqlDbType.VarChar, 50));
            cmdds.Parameters.Add(new SqlParameter("@QTY", SqlDbType.VarChar, 50));
            cmdds.Parameters.Add(new SqlParameter("@isdate", SqlDbType.Date));
            cmdds.Parameters.Add(new SqlParameter("@Productid", SqlDbType.VarChar, 300));
            cmdds.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar, 200));
            cmdds.Parameters.Add(new SqlParameter("@time", SqlDbType.VarChar, 50));
            cmdds.Parameters.Add(new SqlParameter("@SKN", SqlDbType.VarChar, 50));
            cmdds.Parameters.Add(new SqlParameter("@regular_price", SqlDbType.VarChar, 50));
            cmdds.Parameters.Add(new SqlParameter("@SchemeName", SqlDbType.VarChar, 50));
            cmdds.Parameters.Add(new SqlParameter("@saleprice", SqlDbType.VarChar, 50));
            cmdds.Parameters.Add(new SqlParameter("@warehouse", SqlDbType.VarChar, 50));
            return cmdds;
        }

        private void UpdatetableRecord(SqlCommand cmdds, string sku, string qtys, string proid, string proname, string SKN, string regularPrice, string schemeName, string salePrice, string warehouse)
        {
            cmdds.Parameters["@SKU"].Value = (object)sku ?? DBNull.Value;
            cmdds.Parameters["@QTY"].Value = (object)qtys ?? DBNull.Value;
            cmdds.Parameters["@isdate"].Value = DateTime.Now.Date;
            cmdds.Parameters["@Productid"].Value = (object)proid ?? DBNull.Value;
            cmdds.Parameters["@name"].Value = (object)proname ?? DBNull.Value;
            cmdds.Parameters["@time"].Value = DateTime.Now.ToString("HH:mm:ss");
            cmdds.Parameters["@SKN"].Value = (object)SKN ?? DBNull.Value;
            cmdds.Parameters["@regular_price"].Value = (object)regularPrice ?? DBNull.Value;
            cmdds.Parameters["@SchemeName"].Value = (object)schemeName ?? DBNull.Value;
            cmdds.Parameters["@saleprice"].Value = (object)salePrice ?? DBNull.Value;
            cmdds.Parameters["@warehouse"].Value = (object)warehouse ?? DBNull.Value;

            try
            {
                cmdds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB insert error: " + ex.Message);
            }
        }

        public void UpdatetableRecord(string sku, string qtys, string proid, string proname, string SKN, string regularPrice, string schemeName, string salePrice, string warehouse)
        {
            using (var cons = new SqlConnection($"Server=198.12.252.164;Database={db};User ID={id};Password={pass2};"))
            {
                cons.Open();
                using (var cmdds = CreateUpdatedProductsInsertCommand(cons))
                {
                    UpdatetableRecord(cmdds, sku, qtys, proid, proname, SKN, regularPrice, schemeName, salePrice, warehouse);
                }
            }
        }

        private void btnnumber_Click(object sender, EventArgs e)
        {
            var mynumber = Convert.ToInt32(txtpagenumber.Text);
            getproducts(mynumber);
        }

        private void txtpagenumber_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.pagenumbering = Convert.ToInt32(txtpagenumber.Text);
            Settings.Default.Save();
        }

        private void FRM_Updatewebsite_Load(object sender, EventArgs e)
        {
            lblStatus.Text = @"جاري التحضير — سيتم تحميل المنتجات ثم التحديث تلقائيًا...";
        }

        private void FRM_Updatewebsite_Shown(object sender, EventArgs e)
        {
            txtsku.Enabled = true;
            txtsku.ReadOnly = false;

            if (_autoUpdateStarted) return;
            _autoUpdateStarted = true;

            // After the form is shown: fetch all products from WooCommerce, then run the update.
            BeginInvoke(new Action(() =>
            {
                var page = Settings.Default.pagenumbering > 0 ? Settings.Default.pagenumbering : 1;
                _ = getproducts(page);
            }));
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {
            var frm = new FRM_Config();
            frm.ShowDialog();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txtsku.Focus();
            txtsku.SelectAll();
        }

        private void txtsku_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            e.SuppressKeyPress = true;
            var filter = (txtsku?.Text ?? string.Empty).Trim();
            if (!string.IsNullOrWhiteSpace(filter))
            {
                _ = RunSkuSearchAsync();
            }
            else
            {
                getproducts(Settings.Default.pagenumbering > 0 ? Settings.Default.pagenumbering : 1);
            }
        }

        private bool ShouldSkipGridUiUpdates()
        {
            return txtsku != null && (txtsku.Focused || ActiveControl == txtsku);
        }

        private async Task RunSkuSearchAsync()
        {
            _fullUpdateCts?.Cancel();
            _skuSearchCts?.Cancel();
            _skuSearchCts?.Dispose();
            _skuSearchCts = new CancellationTokenSource();
            var token = _skuSearchCts.Token;

            try
            {
                await UpdateOnlySkuAsync(token);
            }
            catch (OperationCanceledException)
            {
                // User started a new search or closed the form.
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        #region iniFocustVar
        public string sCompCode;
        public int seqId;
        public CompanyDetails cd;
        public FMaster fm;
        public Init cmp;
        public Transaction ft;
        public FReport fr;
        public FRateMaster frs;
        private int _pQty;
        #endregion

        private void progressBar2_Click(object sender, EventArgs e)
        {
        }

        public Tuple<decimal, string> GetSchemeDetails(SqlConnection sqlConnection, string productId)
        {
            var currentDate = GetFocusDate(DateTime.Now);


            var query = @"
                SELECT TOP 1
                    SchemeBody.Rate,
                    SchemeBody.BodyId,
                    SchemeBody.Id AS SchemaId,
                    SchemeHeader.SchemeName
                FROM
                    SchemeHeader
                INNER JOIN
                    SchemeBody ON SchemeHeader.Id = SchemeBody.Id
                WHERE
                    SchemeBody.Product = @ProductId
                    AND @CurrentDate BETWEEN StDate AND EndDate and RTRIM(SchemeHeader.SchemeName) like '% ON'
                ORDER BY
                    BodyId DESC";

            using (var adapter = new SqlDataAdapter(query, sqlConnection))
            {
                adapter.SelectCommand.Parameters.AddWithValue("@ProductId", productId ?? (object)DBNull.Value);
                adapter.SelectCommand.Parameters.AddWithValue("@CurrentDate", currentDate);

                var resultTable = new DataTable();
                adapter.Fill(resultTable);

                if (resultTable.Rows.Count > 0)
                {
                    var rate = Convert.ToDecimal(resultTable.Rows[0]["Rate"]);
                    var schemeName = resultTable.Rows[0]["SchemeName"].ToString();

                    if (schemeName != null && schemeName.Trim().EndsWith("ON", StringComparison.OrdinalIgnoreCase))
                    {
                        return Tuple.Create(rate, schemeName);
                    }
                    else
                    {
                        return Tuple.Create(0m, "SchemeName does not end with 'ON'");
                    }
                }
            }

            return Tuple.Create(0m, "K/n");
        }

        public static int GetFocusDate(DateTime date)
        {
            int yearPart = (date.Year - 1950) * 416;
            int monthPart = date.Month * 32;
            int dayPart = date.Day;
            return yearPart + monthPart + dayPart;
        }

        private List<string> ComputeWarehousesList(int masterId)
        {
            var storeRules = new List<(int StoreId, string StoreName, int MinBuffer)>
            {
                (354157, "Online Store Hassa", 0),
                (111, "MAIN STORE-OK", 0),
                (5, "Maisam store", 2),
                (24, "HOFUFSTORE-OK", 1),
                (91, "Zahran Shop", 2),
                (354177, "FRDUSE", 2),
                (89, "Techno 60ST", 2),
                (92, "Khaldia Store", 1),
                (20, "Qatef Store", 2),
                (16, "Ali2 Shop", 2),
                (127, "Mjbmega Store", 2),
                (354174, "Ryadh", 1),
                (1114, "H Mega Hasa", 2),
                (14, "Usra-OK", 2),
                (354178, "Khbar Store", 2),
                (7398, "Menezla Store", 2)
            };

            var list = new List<string>();
            foreach (var rule in storeRules)
            {
                double storePQty = 0, storeAvgVal = 0;
                fr.GetStockOn(masterId, DateTime.Now.Date, rule.StoreId, ref storePQty, ref storeAvgVal);
                var usable = storePQty - rule.MinBuffer;
                if (usable > 0)
                {
                    list.Add(rule.StoreName);
                }
            }
            return list;
        }

        // IDs of Woo categories to exclude from updates (user setting: ExcludedCategoryIds, comma-separated)
        private readonly HashSet<long> _excludedCategoryIds = new HashSet<long>(
            ((BU50_API.Properties.Settings.Default.ExcludedCategoryIds ?? string.Empty)
                .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                .Select(s => { long x; return long.TryParse(s.Trim(), out x) ? x : (long?)null; })
                .Where(x => x.HasValue)
                .Select(x => x.Value)
        );

        // Names of Woo categories to exclude from updates (optional fallback)
        private readonly HashSet<string> _excludedCategories = new HashSet<string>(
            ((BU50_API.Properties.Settings.Default.ExcludedCategories ?? string.Empty).Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                .Select(s => s.Trim().ToLowerInvariant())
        );

        private bool IsExcludedByCategory(Products p)
        {
            if (_excludedCategoryIds != null && _excludedCategoryIds.Count > 0)
            {
                if (p.Categories != null && p.Categories.Any(c => _excludedCategoryIds.Contains(c.Id))) return true;
                if (p.Categorie != null && _excludedCategoryIds.Contains((long)p.Categorie.id)) return true;
                return false;
            }

            if (_excludedCategories == null || _excludedCategories.Count == 0) return false;
            var names = new List<string>();
            if (p.Categories != null)
            {
                foreach (var c in p.Categories)
                {
                    if (!string.IsNullOrWhiteSpace(c.Name)) names.Add(c.Name);
                }
            }
            else if (p.Categorie != null && !string.IsNullOrWhiteSpace(p.Categorie.name))
            {
                names.Add(p.Categorie.name);
            }
            return names.Any(n => _excludedCategories.Contains(n.Trim().ToLowerInvariant()));
        }

        // Update only the product specified in txtsku (SKU/ID/SKN); triggered by Enter key.
        private async Task UpdateOnlySkuAsync(CancellationToken cancellationToken = default)
        {
            var products = await FetchAllProductsAsync(cancellationToken); // respects txtsku filter
            cancellationToken.ThrowIfCancellationRequested();
            if (products == null || products.Count == 0)
            {
                MessageBox.Show("Product not found for the given input.");
                return;
            }
            // Process only this list
            await UpdateUIAfterProductsLoaded(products, true, cancellationToken);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                _skuSearchCts?.Cancel();
                _skuSearchCts?.Dispose();
                _fullUpdateCts?.Cancel();
                _fullUpdateCts?.Dispose();
                ReleaseComObjectSafely(fr);
                ReleaseComObjectSafely(frs);
                ReleaseComObjectSafely(ft);
                ReleaseComObjectSafely(fm);
                ReleaseComObjectSafely(cd);
                ReleaseComObjectSafely(cmp);
            }
            catch
            {
                // Ignore COM release errors during shutdown.
            }

            base.OnFormClosing(e);
        }

        private static void ReleaseComObjectSafely(object comObject)
        {
            if (comObject == null) return;
            if (!Marshal.IsComObject(comObject)) return;

            try
            {
                Marshal.FinalReleaseComObject(comObject);
            }
            catch
            {
                // Ignore release failures to avoid shutdown crashes.
            }
        }
    }
}