using CountryLibraryApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CountryLibraryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountryController : ControllerBase
{
    private readonly ICountryService _countryService;
    public CountryController(ICountryService countryService)
    {
        _countryService = countryService;
    }
    
    [HttpPost]
    [Route("GetCountryByName")]
    public async Task<IActionResult> GetCountryByName([FromBody]string name)
    {
        var infos = await _countryService.GetByCountryName(name);
        
        return Ok(infos);
    }
    
    [HttpPost]
    [Route("GetCountryByRegion")]
    public async Task<IActionResult> GetCountryByRegion([FromBody]string region)
    {
        var infos = await _countryService.GetCountryByRegion(region);
        return Ok(infos);
    }
}