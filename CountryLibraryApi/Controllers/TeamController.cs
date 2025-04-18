using CountryLibraryApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CountryLibraryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamController : ControllerBase
{
    // GET
    [HttpGet]
    public IActionResult Index()
    {
        return Ok(new TeamMembers[]
        {
            new TeamMembers()
            {
                Name = "James",
                Email = "james@gmail.com",
                Age = 20,
                Address = "123 Main Street",
                Gender = "male"
            }
        });    
    }
}