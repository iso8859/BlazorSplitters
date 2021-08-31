using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorSplitters
{
    public partial class HSplitter
    {
        [Parameter]
        public RenderFragment TopChild { get; set; }

        [Parameter]
        public RenderFragment BottomChild { get; set; }

        [Parameter]
        public string SizePanel1 { get; set; } = "50%";

        [Parameter]
        public string SizePanel2 { get; set; } = "50%";

        [Inject]
        IJSRuntime JSRuntime { get; set; }

        JsInterop jsInterop;

        JsInterop GetJs()
        {
            if (jsInterop == null)
                jsInterop = new JsInterop(JSRuntime);
            return jsInterop;
        }

        int state = -1;
        double pos = -1;
        async Task mousedown(MouseEventArgs e)
        {
            state = 1;
            pos = e.ScreenY;
            var size = await GetJs().SPGetBoundingRect("splitterH01");
        }

        void mousemove(MouseEventArgs e)
        {

        }

        void mouseup(MouseEventArgs e)
        {

        }
    }
}
