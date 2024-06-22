using Google.Apis.Auth.OAuth2;
using Google.Cloud.Billing.V1;
//using Google.Cloud.Billing.Budgets.V1;
//using Google.Cloud.Billing.Budgets.V1Beta1;
//using Google.Cloud.Billing.Budgets.V1Alpha1;
using Google.Api.Gax.ResourceNames;
using Grpc.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ProjectLab.Models;

namespace ProjectLab.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //public class GcpCostController : ControllerBase
    //{
    //    private readonly BudgetServiceClient _budgetServiceClient;
    //    private readonly CloudBillingClient _billingClient;
    //    private readonly string _projectId;

    //    public GcpCostController()
    //    {
    //        var credential = GoogleCredential.FromFile("path-to-your-service-account-key.json")
    //                                          .CreateScoped(BudgetServiceClient.DefaultScopes)
    //                                          .ToChannelCredentials();

    //        _budgetServiceClient = BudgetServiceClient.Create();
    //        _billingClient = CloudBillingClient.Create();
    //        _projectId = "your-project-id"; // Replace with your GCP project ID
    //    }

    //    [HttpPost("calculateTotalCost")]
    //    public async Task<ActionResult<CostDetails>> CalculateTotalCost(CostRequest request)
    //    {
    //        try
    //        {
    //            var costData = await GetCostData(request.StartDate, request.EndDate);
    //            var costDetails = GetServiceCostDetails(costData);
    //            costDetails.TotalCost = CalculateTotalCost(costData);
    //            return Ok(costDetails);
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest($"Error calculating total cost: {ex.Message}");
    //        }
    //    }

    //    private async Task<IEnumerable<Budget>> GetCostData(DateTime startDate, DateTime endDate)
    //    {
    //        var budgets = new List<Budget>();

    //        var request = new ListBudgetsRequest
    //        {
    //            ParentAsBillingAccountName = BillingAccountName.FromBillingAccount("your-billing-account-id"), // Replace with your billing account ID
    //            Filter = $"start_time>=\"{startDate:yyyy-MM-dd}\" AND end_time<=\"{endDate:yyyy-MM-dd}\""
    //        };

    //        var response = _budgetServiceClient.ListBudgets(request);
    //        budgets.AddRange(response);

    //        return budgets;
    //    }

    //    private CostDetails GetServiceCostDetails(IEnumerable<Budget> costData)
    //    {
    //        var costDetails = new CostDetails();

    //        foreach (var budget in costData)
    //        {
    //            foreach (var cost in budget.CostFilters)
    //            {
    //                var service = cost.Key;
    //                var amount = cost.Value.MicroUnits / 1e6; // Convert micro units to units (e.g., USD)

    //                // Check if the service key already exists
    //                if (costDetails.ServiceCosts.ContainsKey(service))
    //                {
    //                    // If exists, add the amount to the existing value
    //                    costDetails.ServiceCosts[service] += amount;
    //                }
    //                else
    //                {
    //                    // Otherwise, add a new entry
    //                    costDetails.ServiceCosts.Add(service, amount);
    //                }
    //            }
    //        }

    //        return costDetails;
    //    }

    //    private double CalculateTotalCost(IEnumerable<Budget> costData)
    //    {
    //        double totalCost = 0;

    //        foreach (var budget in costData)
    //        {
    //            foreach (var cost in budget.CostFilters)
    //            {
    //                var amount = cost.Value.MicroUnits / 1e6; // Convert micro units to units (e.g., USD)
    //                totalCost += amount;
    //            }
    //        }

    //        return totalCost;
    //    }
    //}

    //public class CostRequest
    //{
    //    public DateTime StartDate { get; set; }
    //    public DateTime EndDate { get; set; }
    //}

    //public class CostDetails
    //{
    //    public Dictionary<string, double> ServiceCosts { get; set; } = new Dictionary<string, double>();
    //    public double TotalCost { get; set; }
    //}
}
