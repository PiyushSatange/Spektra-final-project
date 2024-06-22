
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjectLab.Controllers;
using ProjectLab.Models;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;
using System.Text;

namespace ProjectLab.Services
{
    public class ScriptFile
    {
        private readonly LabContext _context;
        public ScriptFile(LabContext _dbContext) 
        {
            _context = _dbContext;
        }
        public int uploadAwsFile(IFormFile awsfile, string email)
        {
            if (awsfile == null || awsfile.Length == 0)
            {
                return 0;
            }

            if (!IsAwsScript(awsfile))
            {
                Console.WriteLine("The script is not for aws");
                return 0;
            }

            // Define the directory
            var uploadDirectory = @"C:\terra\aws";

            // Ensure the directory exists
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
                Console.WriteLine("Creating the directory");
            }
            else
            {
                // Clear the directory if it exists
                DirectoryInfo directoryInfo = new DirectoryInfo(uploadDirectory);
                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    file.Delete();
                    Console.WriteLine("Deleting the files...");
                }
                foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
                {
                    dir.Delete(true);
                    Console.WriteLine("Deleting the Directory...");
                }
            }

            // Define the path to save the file with the new name "main"
            var fileExtension = Path.GetExtension(awsfile.FileName);
            var filePath = Path.Combine(uploadDirectory, "main" + fileExtension);
            Console.WriteLine("Renaming the file as main.ts...");

            // Save the file to the specified path
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                awsfile.CopyTo(stream);
                Console.WriteLine("Storing the file in the directory...");
            }

            string ans = RunPowerShell("aws", email);
            Console.WriteLine("This is the result: " + ans);

            return 1;
        }

        public int uploadGcpFile(IFormFile gcpfile, string email)
        {
            if (gcpfile == null || gcpfile.Length == 0)
            {
                return 0;
            }

            if (!IsGcpScript(gcpfile))
            {
                Console.WriteLine("The script is not for gcp");
                return 0;
            }

            // Define the directory
            var uploadDirectory = @"C:\terra\gcp";

            // Ensure the directory exists
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }
            else
            {
                // Clear the directory if it exists
                DirectoryInfo directoryInfo = new DirectoryInfo(uploadDirectory);
                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
                {
                    dir.Delete(true);
                }
            }

            // Define the path to save the file with the new name "main"
            var fileExtension = Path.GetExtension(gcpfile.FileName);
            var filePath = Path.Combine(uploadDirectory, "main" + fileExtension);

            // Save the file to the specified path
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                gcpfile.CopyTo(stream);
            }

            string ans = RunPowerShell("gcp", email);
            Console.WriteLine("This is the result: " + ans);

            return 1;
        }


        public int uploadAzureFile(IFormFile azurefile, string email)
        {
            if (azurefile == null || azurefile.Length == 0)
            {
                return 0;
            }

            if (!IsAzureScript(azurefile))
            {
                Console.WriteLine("The script is not for azure");
                return 0;
            }

            // Define the directory
            var uploadDirectory = @"C:\terra\azure";

            // Ensure the directory exists
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }
            else
            {
                // Clear the directory if it exists
                DirectoryInfo directoryInfo = new DirectoryInfo(uploadDirectory);
                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
                {
                    dir.Delete(true);
                }
            }

            // Define the path to save the file with the new name "main"
            var fileExtension = Path.GetExtension(azurefile.FileName);
            var filePath = Path.Combine(uploadDirectory, "main" + fileExtension);

            // Save the file to the specified path
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                azurefile.CopyTo(stream);
            }

            string ans = RunPowerShell("azure", email);
            Console.WriteLine("This is the result: " + ans);

            return 1;
        }

        public string RunPowerShell(string cloudPlatform, string email)
        {
            Bucket bucket = new Bucket();
            UsersBucket usersBucket = new UsersBucket();
            usersBucket.UserEmail = email;
            try
            {
                PowerShell ps = PowerShell.Create();
                StringBuilder outputBuilder = new StringBuilder();

                // Set the working directory based on the cloud platform
                switch (cloudPlatform.ToLower())
                {
                    case "aws":
                        ps.AddScript("Set-Location -Path 'C:\\terra\\aws'");
                        bucket.Platform = "AWS";
                        Console.WriteLine("AWS script detected");
                        break;
                    case "gcp":
                        ps.AddScript("Set-Location -Path 'C:\\terra\\gcp'");
                        bucket.Platform = "GCP";
                        Console.WriteLine("GCP script detected");
                        break;
                    case "azure":
                        ps.AddScript("Set-Location -Path 'C:\\terra\\azure'");
                        bucket.Platform = "Azure";
                        Console.WriteLine("Azure script detected");
                        break;
                    default:
                        return "Invalid cloud platform specified.";
                }

                var setLocationResults = ps.Invoke();
                Console.WriteLine("Opening the console in the specific directory");

                // Capture the output and errors from setting the location
                if (ps.Streams.Error.Count > 0)
                {
                    StringBuilder errorBuilder = new StringBuilder();
                    foreach (var error in ps.Streams.Error)
                    {
                        errorBuilder.AppendLine(error.ToString());
                    }
                    Console.WriteLine($"Error setting location: {errorBuilder.ToString()}");
                    return $"Error setting location: {errorBuilder.ToString()}";
                }
                else
                {
                    outputBuilder.AppendLine("Set-Location executed successfully.");
                    Console.WriteLine("Set-Location executed successfully.");
                }

                // Run terraform init
                ps.Commands.Clear();
                ps.AddScript("terraform init");
                Console.WriteLine("Running terraform init...");
                var initResults = ps.Invoke();

                if (ps.Streams.Error.Count > 0)
                {
                    StringBuilder errorBuilder = new StringBuilder();
                    foreach (var error in ps.Streams.Error)
                    {
                        errorBuilder.AppendLine(error.ToString());
                    }
                    return $"Error during terraform init: {errorBuilder.ToString()}";
                }

                foreach (var result in initResults)
                {
                    outputBuilder.AppendLine(result.ToString());
                    Console.WriteLine(result.ToString());
                }

                // If terraform init is successful, run terraform apply -auto-approve
                if (ps.Streams.Error.Count == 0)
                {
                    ps.Commands.Clear();
                    ps.AddScript("terraform apply -auto-approve");
                    Console.WriteLine("Running terraform apply -auto-approve...");
                    var applyResults = ps.Invoke();

                    if (ps.Streams.Error.Count > 0)
                    {
                        StringBuilder errorBuilder = new StringBuilder();
                        foreach (var error in ps.Streams.Error)
                        {
                            errorBuilder.AppendLine(error.ToString());
                        }
                        return $"Error during terraform apply: {errorBuilder.ToString()}";
                    }

                    foreach (var result in applyResults)
                    {
                        outputBuilder.AppendLine(result.ToString());
                        Console.WriteLine(result.ToString());
                    }

                    // Read and convert terraform.tfstate to JSON if apply is successful
                    if (cloudPlatform == "aws")
                    {
                        //if (ps.Streams.Error.Count == 0)
                        //{
                        //    var jsonContent = ConvertTfStateToJson(cloudPlatform.ToLower());

                        //    bucket.BucketName = jsonContent.id;
                        //    bucket.Region = jsonContent.region;
                        //    LabContext lc = new LabContext();
                        //    BucketService bs = new BucketService(lc);

                        //    int id = bs.addDeployedScript(bucket);
                        //    usersBucket.BucketId = id;
                        //    Console.WriteLine("bucket id = " + id);
                        //    UserBucketController uc = new UserBucketController(lc);
                        //    uc.addUserBucket(usersBucket);
                        //}
                        ReadAwsData(email);
                    }
                    if(cloudPlatform == "azure")
                    {
                        ReadAzureData(email);
                    }
                }
                return outputBuilder.ToString();
            }
            catch (System.Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }



        public bool IsAwsScript(IFormFile awsFile)
        {
            try
            {
                // Read the content of the script file
                using (var reader = new StreamReader(awsFile.OpenReadStream()))
                {
                    var scriptContent = reader.ReadToEnd();

                    // Check for AWS-specific keywords
                    var awsKeywords = new List<string> { "aws", "provider \"aws\"", "resource \"aws_" };
                    foreach (var keyword in awsKeywords)
                    {
                        if (scriptContent.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                }
                Console.WriteLine("This is not aws script");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading the script file: {ex.Message}");
                return false;
            }
        }


        public bool IsAzureScript(IFormFile azureFile)
        {
            try
            {
                // Read the content of the script file
                using (var reader = new StreamReader(azureFile.OpenReadStream()))
                {
                    var scriptContent = reader.ReadToEnd();

                    // Check for Azure-specific keywords
                    var azureKeywords = new List<string> { "azurerm", "provider \"azurerm\"", "resource \"azurerm_" };
                    foreach (var keyword in azureKeywords)
                    {
                        if (scriptContent.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading the script file: {ex.Message}");
                return false;
            }
        }

        public bool IsGcpScript(IFormFile gcpFile)
        {
            try
            {
                // Read the content of the script file
                using (var reader = new StreamReader(gcpFile.OpenReadStream()))
                {
                    var scriptContent = reader.ReadToEnd();

                    // Check for GCP-specific keywords
                    var gcpKeywords = new List<string> { "google", "provider \"google\"", "resource \"google_" };
                    foreach (var keyword in gcpKeywords)
                    {
                        if (scriptContent.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading the script file: {ex.Message}");
                return false;
            }
        }

        public (string id, string region) ConvertTfStateToJson(string CloudPlatform)
        {
            try
            {
                // Define the path to the terraform.tfstate file
                var tfStatePath = Path.Combine(@"C:\terra\"+CloudPlatform, "terraform.tfstate");

                // Check if the file exists
                if (!File.Exists(tfStatePath))
                {
                    return (null, "terraform.tfstate file not found.");
                }

                // Read the content of the terraform.tfstate file
                var tfStateContent = File.ReadAllText(tfStatePath);

                // Parse the JSON content
                JObject tfStateJson = JObject.Parse(tfStateContent);

                // Extract specific fields from the JSON
                JArray resources = (JArray)tfStateJson["resources"];
                JArray resultArray = new JArray();
                string id = null;
                string region = null;

                foreach (JObject resource in resources)
                {
                    JArray instances = (JArray)resource["instances"];
                    foreach (JObject instance in instances)
                    {
                        JObject attributes = (JObject)instance["attributes"];
                        if (attributes != null)
                        {
                            id = attributes.Value<string>("id");
                            region = attributes.Value<string>("region");

                            JObject resultObject = new JObject();
                            resultObject["id"] = id;
                            resultObject["region"] = region;
                            resultArray.Add(resultObject);

                            // Assuming you only want the first id and region found
                            break;
                        }
                    }
                    if (id != null && region != null) break;
                }


                // Convert the extracted fields to JSON
                return (id, region);
            }
            catch (Exception ex)
            {
                return (null,$"Error extracting fields from terraform.tfstate: {ex.Message}");
            }
        }

        public List<TerraformResource> ReadAzureData(string email)
        {
            var result = new List<TerraformResource>();

            try
            {
                // Read the JSON file
                string filePath = @"C:\terra\azure\terraform.tfstate";
                string jsonData = File.ReadAllText(filePath);

                // Parse the JSON data
                var jsonObject = JObject.Parse(jsonData);

                // Extract the resources array
                var resources = jsonObject["resources"];
                foreach (var resource in resources)
                {
                    // Extract type, location, and name
                    var type = resource["type"].ToString();
                    var instances = resource["instances"];
                    foreach (var instance in instances)
                    {
                        var attributes = instance["attributes"];
                        var location = attributes["location"].ToString();
                        var name = attributes["name"].ToString();
                        result.Add(new TerraformResource
                        {
                            Type = type,
                            Location = location,
                            Name = name
                        });
                        Bucket bucket = new Bucket();
                        bucket.BucketName = name;
                        bucket.Platform = "Azure";
                        bucket.ResourceType = type;
                        bucket.Region = location;
                        _context.Buckets.Add(bucket);
                        _context.SaveChanges();
                        Console.WriteLine("bucket added successfully" + bucket);
                        UsersBucket ub = new UsersBucket();
                        ub.UserEmail = email;
                        ub.BucketId = bucket.BucketId;
                        _context.UsersBuckets.Add(ub);
                        _context.SaveChanges();
                        Console.WriteLine("bucket user connection added successfylly");
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"File not found: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error parsing JSON: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return result;
        }


        public List<TerraformResource> ReadAwsData(string email)
        {
            var result = new List<TerraformResource>();

            try
            {
                // Read the JSON file
                string filePath = @"C:\terra\aws\terraform.tfstate";
                string jsonData = File.ReadAllText(filePath);

                // Parse the JSON data
                var jsonObject = JObject.Parse(jsonData);

                // Extract the resources array
                var resources = jsonObject["resources"];
                foreach (var resource in resources)
                {
                    // Extract type and instances
                    var type = resource["type"].ToString();
                    var instances = resource["instances"];
                    foreach (var instance in instances)
                    {
                        var attributes = instance["attributes"];
                        var location = attributes["region"]?.ToString();
                        var name = attributes["bucket"]?.ToString() ?? attributes["name"]?.ToString();

                        if (location != null && name != null)
                        {
                            result.Add(new TerraformResource
                            {
                                Type = type,
                                Location = location,
                                Name = name
                            });

                            // Add to the database
                            var bucket = new Bucket
                            {
                                BucketName = name,
                                Platform = "AWS",
                                ResourceType = type,
                                Region = location
                            };
                            _context.Buckets.Add(bucket);
                            _context.SaveChanges();
                            Console.WriteLine($"Bucket added successfully: {bucket.BucketName}");

                            var userBucket = new UsersBucket
                            {
                                UserEmail = email,
                                BucketId = bucket.BucketId
                            };
                            _context.UsersBuckets.Add(userBucket);
                            _context.SaveChanges();
                            Console.WriteLine("Bucket user connection added successfully");
                        }
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"File not found: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error parsing JSON: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return result;
        }
    }

    public class TerraformResource
    {
        public string Type { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
    }
}