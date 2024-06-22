using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;

namespace ProjectLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CostController : ControllerBase
    {
        private static string subscriptionId = "c196a0b3-dde0-4c93-b661-551c2f55b6c6";
        private static string apiUrl = $"https://management.azure.com/subscriptions/{subscriptionId}/providers/Microsoft.CostManagement/query?api-version=2019-11-01";
        private static string tenantId = "a6f9c9e8-8623-43f5-a543-74108fd93a01";
        private static string clientId = "826ccb96-af44-47aa-b92f-0733e83869fb";
        private static string clientSecret = "3td8Q~0KDCpY~R6ufleofA4~z-Yyn0Fbaw1CLc4T";
        private static string authority = $"https://login.microsoftonline.com/{tenantId}";
        private static string[] scopes = new string[] { "https://management.azure.com/.default" };

        [HttpGet("GetAzureCostDateWise")]
        public IActionResult GetCostData([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            List<azureData> azureCostList = new List<azureData>();

            try
            {

                if (from > to)
                {
                    Console.WriteLine("\nEnter a Start Date which comes before end date.");
                    return BadRequest("start date should be less than end date");
                }

                string accessToken = GetAccessToken();
                var costData = GetCostAzureData(from, to, accessToken);

                
                Console.WriteLine(costData["properties"]["rows"]);
                var data = costData["properties"]["rows"];
                DateTime d = from;
                for (int i=0; i<data.Count(); i++)
                {
                    Console.WriteLine(data[i][0]);
                    Console.WriteLine(from);
                    azureData ad = new azureData();
                    ad.date = d;
                    ad.cost = Convert.ToInt32(data[i][0]);
                    azureCostList.Add(ad);
                    d = d.AddDays(1);
                }
                
                return Ok(azureCostList);
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nAn error occurred: {e.Message}");
            }
            return Ok(azureCostList);
        }




        private static string GetAccessToken()
        {
            var app = ConfidentialClientApplicationBuilder.Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(new Uri(authority))
                .Build();

            try
            {
                var result = app.AcquireTokenForClient(scopes).ExecuteAsync().Result;
                Console.WriteLine("\n\nAccess token generated successfully. please wait, your report is being Generated.");
                return result.AccessToken;
            }
            catch (MsalServiceException ex)
            {
                Console.WriteLine($"\n\nAn error occurred while generating the access token: {ex.Message}");
                throw;
            }
        }

        private static JObject GetCostAzureData(DateTime from, DateTime to, string accessToken)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var jsonContent = new JObject
            {
                ["type"] = "Usage",
                ["timeframe"] = "Custom",
                ["timePeriod"] = new JObject
                {
                    ["from"] = from.ToString("yyyy-MM-dd"),
                    ["to"] = to.ToString("yyyy-MM-dd")
                },
                ["dataset"] = new JObject
                {
                    ["granularity"] = "Daily",
                    ["aggregation"] = new JObject
                    {
                        ["totalCost"] = new JObject
                        {
                            ["name"] = "Cost",
                            ["function"] = "Sum"
                        }
                    }
                }
            };

            var content = new StringContent(jsonContent.ToString());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = httpClient.PostAsync(apiUrl, content).Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"\n\nRequest failed with status code {response.StatusCode}: {response.Content.ReadAsStringAsync().Result}");
            }

            string responseContent = response.Content.ReadAsStringAsync().Result;
            return JObject.Parse(responseContent);
        }

        //private static void ExportToExcel(JObject costData, string filePath)
        //{
        //    using var workbook = new XLWorkbook();
        //    var worksheet = workbook.Worksheets.Add("Cost Report");

        //    worksheet.Cell(1, 1).Value = "Total Cost";
        //    worksheet.Cell(1, 2).Value = "Date";

        //    var rows = costData["properties"]?["rows"];
        //    if (rows == null)
        //    {
        //        Console.WriteLine("\n\nSorry Data is not Available.");
        //        return;
        //    }

        //    int row = 2;
        //    foreach (var item in rows)
        //    {
        //        worksheet.Cell(row, 1).Value = item[0]?.ToString();
        //        if (DateTime.TryParseExact(item[1]?.ToString(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
        //        {
        //            worksheet.Cell(row, 2).Value = parsedDate.ToString("yyyy-MM-dd");
        //            Console.WriteLine(parsedDate.ToString("yyyy-MM-dd")+"Date in console");
        //        }
        //        else
        //        {
        //            worksheet.Cell(row, 2).Value = item[1]?.ToString();
        //        }
        //        row++;
        //    }

        //    workbook.SaveAs(filePath);
        //    Console.WriteLine($"\nCost report exported to: {filePath}");
        //}
    }
    
    public class azureData
    {
        public DateTime date { get; set; }
        public int cost { get; set; }
    }
}