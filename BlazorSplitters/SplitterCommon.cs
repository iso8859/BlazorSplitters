using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSplitters
{
    public enum Visibility { Both, First, Second, None };
    public class SplitterCommon : Microsoft.AspNetCore.Components.ComponentBase
    {
        public int orientation = 0; // 0 = horizontal, 1 = vertical
        public int mode = 0; // 0 = percent, 1 = pixel

        [Parameter]
        public RenderFragment FirstPanel { get; set; }

        [Parameter]
        public RenderFragment SecondPanel { get; set; }

        [Parameter]
        public string SizeFirstPanel { get; set; } = "50%";

        [Parameter]
        public EventCallback<string> SizeFirstPanelChanged { get; set; }

        [Parameter]
        public Visibility VisiblePanel { get; set; } = Visibility.Both;

        [Inject]
        IJSRuntime JSRuntime { get; set; }

        JsInterop jsInterop;

        public string m_guid;
        protected override void OnInitialized()
        {
            m_guid = Guid.NewGuid().ToString();
        }

        protected string m_sizeFirstPanel;
        protected string m_sizeSecondPanel;
        bool m_parameterSet = false;
        protected override void OnParametersSet()
        {
            if (!m_parameterSet)
            {
                m_sizeFirstPanel = SizeFirstPanel;
                ComputeFinalSize();
                m_parameterSet = true;
            }
        }

        void ComputeFinalSize()
        {
            double finalPercent = double.Parse(m_sizeFirstPanel.Trim('%'), System.Globalization.CultureInfo.InvariantCulture);
            m_sizeSecondPanel = $"{(100 - finalPercent).ToString(System.Globalization.CultureInfo.InvariantCulture)}%";
        }

        JsInterop GetJs()
        {
            if (jsInterop == null)
                jsInterop = new JsInterop(JSRuntime);
            return jsInterop;
        }

        int state = -1;
        JsInterop.DOMRect rect;
        protected async Task onpointerdown(PointerEventArgs e)
        {
            state = 1;
            await GetJs().SPCapturePointer(m_guid + "_slider", e.PointerId);
            rect = await GetJs().SPGetBoundingRect(m_guid);
        }

        protected void onpointermove(PointerEventArgs e)
        {
            if (state == 1 && rect != null)
            {
                if (mode == 0)
                {
                    double pos = 0, percent = 0;
                    if (orientation == 0)
                    {
                        pos = e.ClientY - rect.top;
                        percent = 100 * (pos / rect.height);
                    }
                    else
                    {
                        pos = e.ClientX - rect.left;
                        percent = 100 * (pos / rect.width);
                    }

                    if (percent < 0)
                        percent = 0;
                    if (percent > 100)
                        percent = 100;
                    double finalPercent = Math.Round(percent, 2);
                    m_sizeFirstPanel = $"{finalPercent.ToString(System.Globalization.CultureInfo.InvariantCulture)}%";
                    ComputeFinalSize();
                }
                else
                {
                    double pos = 0, size=0;
                    if (orientation == 0)
                    {
                        pos = e.ClientY - rect.top;
                        size = rect.width;
                    }
                    else
                    {
                        pos = e.ClientX - rect.left;
                        size = rect.height;
                    }
                    pos = Math.Round(pos, 2);
                    m_sizeFirstPanel = $"{pos.ToString(System.Globalization.CultureInfo.InvariantCulture)}px";
                    ComputeFinalSize();
                }
            }
        }

        protected async Task onpointerup(PointerEventArgs e)
        {
            if (state == 1)
            {
                await GetJs().SPReleasePointer(m_guid + "_slider", e.PointerId);
                state = -1;
            }
        }

        protected void Direct(int percent)
        {
            m_sizeFirstPanel = $"{percent}%";
            ComputeFinalSize();
        }
    }
}
