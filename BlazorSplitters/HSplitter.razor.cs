using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using System;

namespace BlazorSplitters
{
    public partial class HSplitter : SplitterCommon
    {
        public HSplitter()
        {
            orientation = 0;
            mode = 0;
        }
    }
}
