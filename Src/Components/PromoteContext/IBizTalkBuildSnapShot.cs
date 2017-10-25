using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;

namespace BizTalk.PipelineComponents
{
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("8fc58bc6-e6af-401d-901d-2d6d963c0d87"), CLSCompliant(false)]
    internal interface IBizTalkBuildSnapShot
    {
        object GetCompilableFiles();
        [return: MarshalAs(UnmanagedType.IUnknown)]
        object GetCommonProjectProperties();
        [return: MarshalAs(UnmanagedType.IUnknown)]
        object GetConfigProperties();
        object GetReferences();
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetTempAssemblyPath();
    }
}
