using CountryLibraryApi.DTOs;
using System.Text.Json;
using CountryLibraryApi.Services.Interfaces;

namespace CountryLibraryApi.Services;


public class CountryService:ICountryService
{
    public readonly IHttpClientFactory _HttpClientFactory;
    public IConfiguration _configuration;
    public CountryService(IHttpClientFactory clientFactory,IConfiguration configuration)
    {
        _HttpClientFactory = clientFactory;
        _configuration = configuration;
    }

    
    public async Task<List<CountryInfos>> GetByCountryName(string name)
    {
        var client = _HttpClientFactory.CreateClient();

        var response = await client.GetAsync($"{_configuration["DataSourceUrl"]?.ToString()}/name/{name}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return PopuplateData(content);
    }

    private static List<CountryInfos> PopuplateData(string content)
    {
        using JsonDocument doc = JsonDocument.Parse(content);
        JsonElement root = doc.RootElement;
        List<CountryInfos> countryInfos = new List<CountryInfos>();
        foreach (var item in root.EnumerateArray())
        {
            var countryInfo = new CountryInfos
            {
                Name = item.GetProperty("name").GetProperty("common").GetString(),
                //Alpha2Code = item.GetProperty("alpha2Code").GetString(),
                Capital = item.GetProperty("capital")[0].GetString(),
                CallingCodes = int.Parse(item.GetProperty("idd").GetProperty("root")
                + item.GetProperty("idd").GetProperty("suffixes")[0].GetString()),
                FlagUrl = item.GetProperty("flags").GetProperty("png").GetString(),
                Timezones = item.GetProperty("timezones")[0].GetString()
            };
            countryInfos.Add(countryInfo);
        }
        return countryInfos;
    }

    public async Task<List<CountryInfos>> GetCountryByRegion(string region)
    {    
        var client = _HttpClientFactory.CreateClient();

        var response = await client.GetAsync($"{_configuration["DataSourceUrl"]?.ToString()}/region/{region}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return PopuplateData(content);
    }
    
}