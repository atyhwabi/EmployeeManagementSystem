using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Client.Portal.Components.Layout
{
    public partial class FooterLayout : ComponentBase
    {
       
        public string Logo { get; set; } = string.Empty;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
