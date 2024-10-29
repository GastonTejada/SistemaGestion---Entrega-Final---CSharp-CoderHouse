using Blazored.SessionStorage;
using SistemaGestionEntities;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.WebUtilities;

namespace SistemaGestionUI.ClientServices;

public class AutenticacionService
{
    private readonly HttpClient _httpClient;
    private readonly ISessionStorageService _sessionStorageService;

    public AutenticacionService(HttpClient httpClient, ISessionStorageService sessionStorageService)
    {
        _httpClient = httpClient;
        _sessionStorageService = sessionStorageService;
    }

    public async Task<bool> AutenticarUsuario(LoginDTO login)
    {
        var response = await _httpClient.PostAsJsonAsync("Autenticar", login);
        if (response.IsSuccessStatusCode)
        {
            await _sessionStorageService.SetItemAsync("loginDTO", login);
            return true;
        }
        else
        {
            return false;
        }
    }
}
