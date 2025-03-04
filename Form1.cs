using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using MapsScraper.Models;
using Google.Apis.Services;
using System.Text.RegularExpressions;

namespace MapsScraper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await LoadCitiesAsync();
        }

        private async void cmbIl_SelectedIndexChanged(object sender, EventArgs e)
        {
            await LoadDistrictsAsync();
        }

        private async Task LoadDistrictsAsync()
        {
            string selectedCity = cmbIl.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedCity))
            {
                MessageBox.Show("Lütfen geçerli bir şehir seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string provincesUrl = "https://turkiyeapi.dev/api/v1/provinces";
            var provincesData = await FetchDataAsync(provincesUrl);
            int? cityId = null;

            if (provincesData != null && provincesData.data != null)
            {
                foreach (var item in provincesData.data)
                {
                    if (item.name.ToString() == selectedCity)
                    {
                        cityId = item.id;
                        break;
                    }
                }

                if (cityId != null)
                {
                    string districtUrl = $"https://turkiyeapi.dev/api/v1/provinces/{cityId}";
                    var districtsData = await FetchDataAsync(districtUrl);

                    if (districtsData != null && districtsData.data != null && districtsData.data.districts != null)
                    {
                        cmbIlce.Items.Clear();
                        foreach (var item in districtsData.data.districts)
                        {
                            cmbIlce.Items.Add(item.name);
                        }
                    }
                    else
                    {
                        MessageBox.Show("İlçeler verisi alınamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("İller verisi doğru şekilde alınamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("İller verisi alınırken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadCitiesAsync()
        {
            string url = "https://turkiyeapi.dev/api/v1/provinces";
            var data = await FetchDataAsync(url);
            if (data != null && data.data != null)
            {
                foreach (var item in data.data)
                {
                    cmbIl.Items.Add(item.name.ToString());
                }
            }
            else
            {
                MessageBox.Show("Şehirler yüklenemedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<dynamic> FetchDataAsync(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetStringAsync(url);
                    return JsonConvert.DeserializeObject(response);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veri çekme hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            string apiKey = "AIzaSyBQAvkLKfY2tGlT3gwhjybKkrvqMsnj6TI";
            string keyword = txtKeyword.Text;
            string city = cmbIl.SelectedItem?.ToString();
            string district = cmbIlce.SelectedItem?.ToString();

            string query = $"{keyword} {city} {district}";
            string url = $"https://maps.googleapis.com/maps/api/place/textsearch/json?query={Uri.EscapeDataString(query)}&key={apiKey}";

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetStringAsync(url);
                    Console.WriteLine($"API Response: {response}"); 
                    var result = JsonConvert.DeserializeObject<GooglePlacesApiResponse>(response);

                    if (result?.Results != null && result.Results.Count > 0)
                    {
                        var googlePlaces = ConvertToGooglePlaceList(result.Results);
                        ExportExcel(googlePlaces);
                    }
                    else
                    {
                        MessageBox.Show("Hiçbir sonuç bulunamadı.", "Sonuçlar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<GooglePlace> ConvertToGooglePlaceList(List<GooglePlaceDetails> detailsList)
        {
            return detailsList.Select(detail => new GooglePlace
            {
                Name = detail.Name,
                FormattedAddress = detail.FormattedAddress,
                FormattedPhoneNumber = detail.FormattedPhoneNumber,
                Website = detail.Website
            }).ToList();
        }

        private string ExtractEmailFromWebsite(string website)
        {
            if (string.IsNullOrWhiteSpace(website) || website == "N/A")
                return "N/A";

            try
            {
                using (var client = new HttpClient())
                {
                    string pageContent = client.GetStringAsync(website).Result;

                    Match match = Regex.Match(pageContent, @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");
                    return match.Success ? match.Value : "E-posta bulunamadı";
                }
            }
            catch
            {
                return "E-posta alınamadı";
            }
        }


        private void ExportExcel(List<GooglePlace> places)
        {
            Excel.Application excelApp = new Excel.Application();
            if (excelApp == null)
            {
                MessageBox.Show("Excel yüklü değil!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Excel.Workbook workbook = excelApp.Workbooks.Add();
            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1];

            worksheet.Cells[1, 1] = "Firma Adı";
            worksheet.Cells[1, 2] = "Telefon";
            worksheet.Cells[1, 3] = "Adres";
            worksheet.Cells[1, 4] = "E-posta";

            int row = 2;

            foreach (var place in places)
            {
                worksheet.Cells[row, 1] = place.Name;
                worksheet.Cells[row, 2] = place.FormattedPhoneNumber ?? "N/A";
                worksheet.Cells[row, 3] = place.FormattedAddress;
                worksheet.Cells[row, 4] = ExtractEmailFromWebsite(place.Website ?? "N/A");
                row++;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Dosyası|*.xlsx",
                Title = "Excel Dosyası Kaydet",
                FileName = "FirmaListesi.xlsx"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                workbook.SaveAs(saveFileDialog.FileName);
                workbook.Close();
                excelApp.Quit();
                MessageBox.Show("Veriler Excel'e aktarıldı!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
