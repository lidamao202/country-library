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
            var countryInfo = new CountryInfos();

            if (item.TryGetProperty("name", out JsonElement nameElement) &&
                nameElement.TryGetProperty("common", out JsonElement commonName))
            {
                countryInfo.Name = commonName.GetString();
            }
            //Alpha2Code = item.GetProperty("alpha2Code").GetString()

            if (item.TryGetProperty("capital", out JsonElement capitalElement) &&
                capitalElement.ValueKind == JsonValueKind.Array &&
                capitalElement.GetArrayLength() > 0)
            {
                countryInfo.Capital = capitalElement[0].GetString();
            }

            if (item.TryGetProperty("idd", out JsonElement iddElement))
            {
                if (iddElement.TryGetProperty("root", out JsonElement root2) &&
                    iddElement.TryGetProperty("suffixes", out JsonElement suffixes) &&
                    suffixes.ValueKind == JsonValueKind.Array &&
                    suffixes.GetArrayLength() > 0)
                {
                    countryInfo.CallingCodes = int.Parse(root2.GetString() + suffixes[0].GetString());
                }
            }

            if (item.TryGetProperty("flags", out JsonElement flagsElement) &&
                flagsElement.TryGetProperty("png", out JsonElement flagUrl))
            {
                countryInfo.FlagUrl = flagUrl.GetString();
            }

            if (item.TryGetProperty("timezones", out JsonElement timezonesElement) &&
                timezonesElement.ValueKind == JsonValueKind.Array &&
                timezonesElement.GetArrayLength() > 0)
            {
                countryInfo.Timezones = timezonesElement[0].GetString();
            }
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