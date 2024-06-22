using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectLab.Models;
using ProjectLab.Services;
using System.Diagnostics;

namespace ProjectLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScriptController : ControllerBase
    {
        [HttpPost("uploadUrl")]
        public IActionResult uploadScriptUrl(string awsurl, string gcpurl, string azureurl)
        {
            Console.WriteLine($"{awsurl}, { gcpurl},{ azureurl}");
            // Specify the path to PowerShell.exe
            string powerShellPath = @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe";

            // Start PowerShell process
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = powerShellPath;
            psi.UseShellExecute = false; // Don't use OS shell
            psi.CreateNoWindow = true; // Don't create a window
            psi.RedirectStandardInput = true; // Redirect input

            Process psProcess = Process.Start(psi);

            // Wait for the process to exit
            psProcess.WaitForExit();
            return Ok();
        }

        [HttpPost("uploadfile")]
        public IActionResult uploadScriptFile(IFormFile awsfile, IFormFile gcpfile, IFormFile azurefile)
        {
            if (awsfile == null || awsfile.Length == 0)
            {
                return BadRequest("No file uploaded for aws");
            }
            // Define the directory and ensure it exists
            var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            // Define the path to save the file
            var filePath = Path.Combine(uploadDirectory, awsfile.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                awsfile.CopyTo(stream);
            }

            //for gcp
            if (gcpfile == null || gcpfile.Length == 0)
            {
                return BadRequest("No file uploaded for gcp");
            }
            // Define the directory and ensure it exists

            // Define the path to save the file
            filePath = Path.Combine(uploadDirectory, gcpfile.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                gcpfile.CopyTo(stream);
            }

            //gor azure
            if (azurefile == null || azurefile.Length == 0)
            {
                return BadRequest("No file uploaded for gcp");
            }
            // Define the directory and ensure it exists

            // Define the path to save the file
            filePath = Path.Combine(uploadDirectory, azurefile.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                azurefile.CopyTo(stream);
            }
            return Ok("Done");
        }

        [HttpPost("uploadAwsfile/{email}")]
        public IActionResult uploadAwsFile(string email)
        {
            
            Console.WriteLine("It is comming in aws controller");
            Console.WriteLine(email);
            var awsfile = Request.Form.Files[0];
            Console.WriteLine(awsfile.Name+" "+awsfile.FileName+" "+awsfile.ContentType);
            LabContext lc = new LabContext();
            ScriptFile scriptFile = new ScriptFile(lc);
            int result = scriptFile.uploadAwsFile(awsfile, email);
            if (result == 0)
            {
                return NotFound();
            }
            return Ok();
        }


        [HttpPost("uploadGcpfile")]
        public IActionResult uploadgcpFile(IFormFile awsfile, string email)
        {
            LabContext lc = new LabContext();
            ScriptFile scriptFile = new ScriptFile(lc);
            int result = scriptFile.uploadGcpFile(awsfile, email);
            if (result == 0)
            {
                return NotFound();
            }

            return Ok();
        }



        [HttpPost("uploadAzurefile/{email}")]
        public IActionResult uploadAzureFile(string email)
        {
            Console.WriteLine("It is comming in azure controller");
            var azurefile = Request.Form.Files[0];
            Console.WriteLine(azurefile.Name + " " + azurefile.FileName + " " + azurefile.ContentType);
            LabContext lc = new LabContext();
            ScriptFile scriptFile = new ScriptFile(lc);
            int result = scriptFile.uploadAzureFile(azurefile, email);
            if (result == 0)
            {
                return NotFound();
            }

            return Ok();
        }


    }

    
}
