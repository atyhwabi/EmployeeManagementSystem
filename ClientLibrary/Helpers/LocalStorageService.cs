using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary.Helpers
{
    public class LocalStorageService(ILocalStorageService localStorageService)
    {

        private const string StorageKey = "authentication-token";
        public async Task<string> GetToken() => await localStorageService.GetItemAsync<string>(StorageKey);
        public async Task SetToken(string token) => await localStorageService.SetItemAsync(StorageKey, token);
        public async Task RemoveToken() => await localStorageService.RemoveItemAsync(StorageKey);
    }
}
