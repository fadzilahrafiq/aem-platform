// Controllers/CustomerController.cs

using AemAssessment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace AemAssessment.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SyncController : ControllerBase
  {
    private readonly DataContext _context;
    private string bearer;

    public SyncController(DataContext context)
    {
        _context = context;
    }

    public async Task<string> initiateLogin()
    {
        return bearer = await LoginUser("user@aemenersol.com", "Test@123");
    }

    private async Task<string> LoginUser(string username, string password)
    {
      using (var http = new HttpClient()) 
      {
        http.BaseAddress = new Uri("http://test-demo.aemenersol.com/api/Account/Login");

        var loginData = new { username = username, password = password };
        var content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");

        var response = await http.PostAsync("", content);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Login done");
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString.Trim('"');
        }
        else
        {
          throw new Exception($"Login failed with status code: {response.StatusCode}");
        }

      }

    }

    // GET: api/Platform
    [HttpGet]
    public Task<IEnumerable<Platform>> GetPlatforms()
    {
        return syncPlatforms();
    }

    public async Task<IEnumerable<Platform>> syncPlatforms()
    {
        await initiateLogin();
        await fetchPlatform();

        return _context.Platform.ToList();
    }

    public async Task<IActionResult> fetchPlatform()
    {
        using (var http = new HttpClient())
        {
            var url = "http://test-demo.aemenersol.com/api/PlatformWell/GetPlatformWellActual";

            var bearerToken = bearer;

            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            var response = await http.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string externalData = await response.Content.ReadAsStringAsync();

                List<ExternalPlatformModel> platforms = JsonSerializer.Deserialize<List<ExternalPlatformModel>>(externalData);
                processSyncData(platforms);

                return Ok(externalData);
            } 
            else 
            {
                return null;

            }
        }
    }

    private void processSyncData(List<ExternalPlatformModel> platforms)
    {   

        foreach (var plat in platforms) 
        {
            List<Platform> existingPlatform = _context.Platform.Where(p => p.PlatformId == plat.id).ToList();

            if (existingPlatform.Count() > 0)
            {
                foreach (var pp in existingPlatform)
                {
                    pp.Name = plat.uniqueName;
                    pp.Latitude = plat.latitude;
                    pp.Longitude = plat.longitude;
                    pp.CreatedAt = plat.createdAt;
                    pp.UpdatedAt = plat.updatedAt;
                }
            }
            else
            {
                Platform platform = new Platform();
                platform.PlatformId = plat.id;
                platform.Name = plat.uniqueName;
                platform.Latitude = plat.latitude;
                platform.Longitude = plat.longitude;
                platform.CreatedAt = plat.createdAt;
                platform.UpdatedAt = plat.updatedAt;
                _context.Platform.Add(platform);
            }

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex) 
            {
                Console.WriteLine("Error occured -> Database operation failed. Please check manually");
            }

            foreach (var wel in plat.well)
            {
                List<Well> existingWell = _context.Well.Where(w => w.WellId == wel.id).ToList();

                if (existingWell.Count() > 0)
                {
                    foreach (var ww in existingWell)
                    {
                        ww.Name = wel.uniqueName;
                        ww.Latitude = wel.latitude;
                        ww.Longitude = wel.longitude;
                        ww.CreatedAt = wel.createdAt;
                        ww.UpdatedAt = wel.updatedAt;
                    }
                }
                else
                {
                    Well well = new Well();
                    well.WellId = wel.id;
                    well.PlatformId = wel.platformId;
                    well.Name = wel.uniqueName;
                    well.Latitude = wel.latitude;
                    well.Longitude = wel.longitude;
                    well.CreatedAt = wel.createdAt;
                    well.UpdatedAt = wel.updatedAt;

                    _context.Well.Add(well);
                }
                

                try 
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine("Error occured -> Database operation failed. Please check manually");
                }
            }
            
        }
    }

    // POST: api/Customer
    // [HttpPost]
    // public ActionResult<Customer> CreateCustomer(Customer customer)
    // {
    //   if (customer == null)
    //   {
    //     return BadRequest();
    //   }
    //   _context.Customer.Add(customer);
    //   _context.SaveChanges();
    //   return CreatedAtAction(nameof(GetCustomer), new { id = customer.CustomerId }, customer);
    // }
  }
}