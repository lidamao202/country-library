using CountryLibraryApi.DTOs;

namespace CountryLibraryApi.Services.Interfaces;

public interface ICountryService
{
    Task<List<CountryInfos>> GetByCountryName(string name);
    Task<List<CountryInfos>> GetCountryByRegion(string region);
    
}