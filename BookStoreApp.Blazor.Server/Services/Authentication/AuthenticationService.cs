using Blazored.LocalStorage;
using BookStoreApp.Blazor.Server.UI.Providers;
using BookStoreApp.Blazor.Server.UI.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStoreApp.Blazor.Server.UI.Services.Authentication
{
	public class AuthenticationService : IAuthenticationService
	{
		private readonly IClient client;
		private readonly ILocalStorageService localStorage;
		private readonly AuthenticationStateProvider authenticationStateProvider;

		public AuthenticationService(IClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider)
		{
			this.client = httpClient;
			this.localStorage = localStorage;
			this.authenticationStateProvider = authenticationStateProvider;
		}

		public async Task<bool> AuthenticateAsync(LoginUserDto loginModel)
		{
			var response = await client.LoginAsync(loginModel);

			// Store token
			 await localStorage.SetItemAsync("accessToken", response.Token);

			// Change auth state of the app
			await ((ApiAuthenticationStateProvider)authenticationStateProvider).LoggedIn();

			return true;
		}
	}
}
