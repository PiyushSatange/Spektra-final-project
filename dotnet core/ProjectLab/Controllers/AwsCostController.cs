using Amazon.CostExplorer.Model;
using Amazon.CostExplorer;
using Amazon.Runtime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Amazon;


namespace ProjectLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AwsCostController : ControllerBase
    {
        private readonly IAmazonCostExplorer _costExplorerClient;

        public AwsCostController()
        {
            var awsCredentials = new BasicAWSCredentials("AKIA47CRZ3S3CDEEMR56", "mGpkM9FP6+OQqq1y63TQb0Kkqu+qWj8I26/BKWRe");
            _costExplorerClient = new AmazonCostExplorerClient(awsCredentials, RegionEndpoint.USEast1);
        }

        [HttpPost("calculateTotalCost")]
        public async Task<ActionResult<CostDetails>> CalculateTotalCost(CostRequest request)
        {
            try
            {
                var costData = await GetCostData(request.StartDate, request.EndDate);
                var costDetails = GetServiceCostDetails(costData);
                costDetails.TotalCost = CalculateTotalCost(costData);
                return Ok(costDetails);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error calculating total cost: {ex.Message}");
            }
        }

        private async Task<GetCostAndUsageResponse> GetCostData(DateTime startDate, DateTime endDate)
        {
            var request = new GetCostAndUsageRequest
            {
                TimePeriod = new DateInterval
                {
                    Start = startDate.ToString("yyyy-MM-dd"),
                    End = endDate.ToString("yyyy-MM-dd")
                },
                Granularity = Granularity.DAILY,
                Metrics = new List<string> { "UnblendedCost" },
                GroupBy = new List<GroupDefinition>
                {
                    new GroupDefinition
                    {
                        Type = GroupDefinitionType.DIMENSION,
                        Key = "SERVICE"
                    }
                }
            };

            return await _costExplorerClient.GetCostAndUsageAsync(request);
        }

        private CostDetails GetServiceCostDetails(GetCostAndUsageResponse costData)
        {
            var costDetails = new CostDetails();

            foreach (var result in costData.ResultsByTime)
            {
                foreach (var group in result.Groups)
                {
                    var service = group.Keys[0];
                    var amount = double.Parse(group.Metrics["UnblendedCost"].Amount);

                    // Check if the service key already exists
                    if (costDetails.ServiceCosts.ContainsKey(service))
                    {
                        // If exists, add the amount to the existing value
                        costDetails.ServiceCosts[service] += amount;
                    }
                    else
                    {
                        // Otherwise, add a new entry
                        costDetails.ServiceCosts.Add(service, amount);
                    }
                }
            }

            return costDetails;
        }

        private double CalculateTotalCost(GetCostAndUsageResponse costData)
        {
            double totalCost = 0;

            foreach (var result in costData.ResultsByTime)
            {
                foreach (var group in result.Groups)
                {
                    var amount = double.Parse(group.Metrics["UnblendedCost"].Amount);
                    totalCost += amount;
                }
            }

            return totalCost;
        }
    }

    public class CostRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class CostDetails
    {
        public Dictionary<string, double> ServiceCosts { get; set; } = new Dictionary<string, double>();
        public double TotalCost { get; set; }
    }
}