using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using System;

namespace BlazorSplitters
{
    public partial class VSplitter : SplitterCommon
    {
        public VSplitter()
        {
            orientation = 1;
            mode = 0;
        }
    }
}
