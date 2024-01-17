using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BookStoreApp.Blazor.Server.UI.Providers
{
	public class ApiAuthenticationStateProvider : AuthenticationStateProvider
	{
		private readonly ILocalStorageService localStorage;
		private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler;
		
		public ApiAuthenticationStateProvider(ILocalStorageService localStorage)
		{
			this.localStorage = localStorage;
			jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
		}
		
		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			var notLoggedIn = new ClaimsPrincipal(new ClaimsIdentity());
			var savedToken = await localStorage.GetItemAsync<string>("accessToken");
			
			// If token is empty, return an empty authentication state
			if (savedToken == null)
			{
				return new AuthenticationState(notLoggedIn);
			}

			
			var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(savedToken);

			// Check if the token is expired
			if(tokenContent.ValidTo < DateTime.Now)
			{
				await localStorage.RemoveItemAsync("accessToken");
				return new AuthenticationState(notLoggedIn);
			}

			var claims = tokenContent.Claims;

			var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

			return new AuthenticationState(user);
		}


		public async Task LoggedIn()
		{
			var savedToken = await localStorage.GetItemAsync<string>("accessToken");
			var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(savedToken);
			var claims = tokenContent.Claims;
			var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
			var authState = Task.FromResult(new AuthenticationState(user));
			NotifyAuthenticationStateChanged(authState);
		}

		public async Task LoggedOut()
		{
			var nobody = new ClaimsPrincipal(new ClaimsIdentity());
			var authState = Task.FromResult(new AuthenticationState(nobody));
			await localStorage.RemoveItemAsync("accessToken");
			NotifyAuthenticationStateChanged(authState);
		}
	}
}
