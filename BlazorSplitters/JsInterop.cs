using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace BlazorSplitters
{
    // This class provides an example of how JavaScript functionality can be wrapped
    // in a .NET class for easy consumption. The associated JavaScript module is
    // loaded on demand when first needed.
    //
    // This class can be registered as scoped DI service and then injected into Blazor
    // components for use.

    public class JsInterop : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public class DOMRect
        {
            public double bottom { get; set; }
            public double height{ get; set; }
            public double left{ get; set; }
            public double right{ get; set; }
            public double top{ get; set; }
            public double width{ get; set; }
            public double x{ get; set; }
            public double y{ get; set; }
        }

        public JsInterop(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", "./_content/BlazorSplitters/JsInterop.js").AsTask());
        }

        public async ValueTask<DOMRect> SPGetBoundingRect(string elementId)
        {
            var module = await moduleTask.Value;
            string tmp = await module.InvokeAsync<string>("SPGetBoundingRect", elementId);
            return System.Text.Json.JsonSerializer.Deserialize<DOMRect>(tmp);
        }

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
