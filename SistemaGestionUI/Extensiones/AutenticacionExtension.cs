﻿using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SistemaGestionEntities;
using System.Security.Claims;

namespace SistemaGestionUI.Extensiones;

public class AutenticacionExtension : AuthenticationStateProvider
{
    private readonly ISessionStorageService _sessionStorage;
    private readonly NavigationManager _navigationManager;
    private ClaimsPrincipal _sinInformacion = new ClaimsPrincipal(new ClaimsIdentity());
    private bool _isPrerendering = true;

    public AutenticacionExtension(ISessionStorageService sessionStorage, NavigationManager navigationManager)
    {
        _sessionStorage = sessionStorage;
        _navigationManager = navigationManager;

        if (_navigationManager.Uri.Contains("http"))
        {
            _isPrerendering = false;
        }
    }

    public async Task ActualizarEstadoAutenticacion(SesionDTO? sesionUsuario)
    {
        if (_isPrerendering)
        {
            return;
        }

        ClaimsPrincipal claimsPrincipal;

        if (sesionUsuario != null)
        {
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, sesionUsuario.Nombre),
                new Claim(ClaimTypes.Email, sesionUsuario.Correo)
            }, "JwtAuth"));

            await _sessionStorage.GuardarStorage("sesionUsuario", sesionUsuario);
        }
        else
        {
            claimsPrincipal = _sinInformacion;
            await _sessionStorage.RemoveItemAsync("sesionUsuario");
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        
        if (_isPrerendering)
        {
            return await Task.FromResult(new AuthenticationState(_sinInformacion));
        }

        var sesionUsuario = await _sessionStorage.ObtenerStorage<SesionDTO>("sesionUsuario");

        if (sesionUsuario == null)
            return await Task.FromResult(new AuthenticationState(_sinInformacion));

        var claimPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.Name, sesionUsuario.Nombre),
            new Claim(ClaimTypes.Email, sesionUsuario.Correo)
        }, "JwtAuth"));

        return await Task.FromResult(new AuthenticationState(claimPrincipal));
    }
}
