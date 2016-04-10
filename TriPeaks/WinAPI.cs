using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace TriPeaks
{
    // Source: http://stackoverflow.com/a/2341385
    internal static class IconHelper
    {
        const int GWL_EXSTYLE = -20;
        const int WS_EX_DLGMODALFRAME = 0x0001;
        const int SWP_NOSIZE = 0x0001;
        const int SWP_NOMOVE = 0x0002;
        const int SWP_NOZORDER = 0x0004;
        const int SWP_FRAMECHANGED = 0x0020;
        const uint WM_SETICON = 0x0080;

        public static void RemoveIcon(Window window)
        {
            IntPtr hwnd = new WindowInteropHelper(window).Handle;
            var extendedStyle = NativeMethods.GetWindowLong(hwnd, GWL_EXSTYLE);
            if (extendedStyle == 0) {
                // 0 -> method failed; don't attempt to change anything.
                return;
            }

            if (NativeMethods.SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_DLGMODALFRAME) != 0) {
                // same thing: 0 means the call has failed.
                NativeMethods.SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
            }
            
        }

    }

    [ExcludeFromCodeCoverage]
    internal static class NativeMethods
    {
        [DllImport("user32.dll")]
        internal static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        internal static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)] // SetWindowPos is defined as BOOL, which corresponds to Bool according to the CA1414 docs.
        internal static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int width, int height, uint flags);
        
    }
}
