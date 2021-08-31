using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using System;

namespace BlazorSplitters
{
    public partial class HSplitter : Microsoft.AspNetCore.Components.ComponentBase
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

        string m_guid;
        protected override void OnInitialized()
        {
            m_guid = Guid.NewGuid().ToString();
        }

        JsInterop GetJs()
        {
            if (jsInterop == null)
                jsInterop = new JsInterop(JSRuntime);
            return jsInterop;
        }

        int state = -1;
        double posY = -1;
        JsInterop.DOMRect rect;
        async Task onpointerdown(PointerEventArgs e)
        {
            Console.WriteLine("mousedown");
            state = 1;
            posY = e.OffsetY;
            await GetJs().SPCapturePointer(m_guid + "_hslider", e.PointerId);
            rect = await GetJs().SPGetBoundingRect(m_guid);           
        }

        Task onpointermove(PointerEventArgs e)
        {
            Console.WriteLine("mousemove");
            if (state == 1 && rect!=null)
            {
                double percent = 100 * ((e.OffsetY - posY) / rect.height);
                if (percent < 5)
                    percent = 5;
                if (percent > 95)
                    percent = 95;
                double finalPercent = Math.Round(percent, 2);
                SizePanel1 = $"{finalPercent.ToString(System.Globalization.CultureInfo.InvariantCulture)}%";
                SizePanel2 = $"{(100-finalPercent).ToString(System.Globalization.CultureInfo.InvariantCulture)}%";
                Console.WriteLine($"{e.ClientY};{e.ScreenY};{e.OffsetY};{percent};{finalPercent};{SizePanel1};{SizePanel2}");
            }
            return Task.CompletedTask;
        }

        async Task onpointerup(PointerEventArgs e)
        {
            Console.WriteLine("mouseup");
            if (state==1)
            {
                await GetJs().SPReleasePointer(m_guid + "_hslider", e.PointerId);
                state = -1;
            }
        }
    }
}
