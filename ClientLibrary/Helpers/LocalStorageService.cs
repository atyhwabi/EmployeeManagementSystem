using Blazored.LocalStorage;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace ClientLibrary.Helpers
{
    public class LocalStorageService
    {
        private const string StorageKey = "authentication-token";
        private readonly ILocalStorageService _localStorageService;
        private readonly IJSRuntime _jsRuntime;
        private bool _isClientSide;

        public LocalStorageService(ILocalStorageService localStorageService, IJSRuntime jsRuntime)
        {
            _localStorageService = localStorageService;
            _jsRuntime = jsRuntime;
        }

        public void MarkAsClientSide()
        {
            _isClientSide = true;
        }

        public async Task<string> GetToken()
        {
            if (!_isClientSide)
            {
                throw new InvalidOperationException("Cannot access localStorage during prerendering.");
            }
            return await _localStorageService.GetItemAsync<string>(StorageKey);
        }

        public async Task SetToken(string token)
        {
            if (!_isClientSide)
            {
                throw new InvalidOperationException("Cannot access localStorage during prerendering.");
            }
            await _localStorageService.SetItemAsync(StorageKey, token);
        }

        public async Task RemoveToken()
        {
            if (!_isClientSide)
            {
                throw new InvalidOperationException("Cannot access localStorage during prerendering.");
            }
            await _localStorageService.RemoveItemAsync(StorageKey);
        }
    }
}
