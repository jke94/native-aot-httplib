namespace MyHttpLib
{
    using System;
    using System.Net.Http;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;

    public static class NativeExports
    {
        private static readonly HttpClient httpClient = new HttpClient();

        [UnmanagedCallersOnly(EntryPoint = "MakeHttpRequest")]
        public static unsafe int MakeHttpRequest(IntPtr urlPtr, IntPtr* resultPtr, int* resultSize)
        {
            try
            {
                string url = Marshal.PtrToStringAnsi(urlPtr);
                Task<string> task = httpClient.GetStringAsync(url);
                task.Wait();

                string result = task.Result;
                byte[] resultBytes = System.Text.Encoding.UTF8.GetBytes(result);
                *resultSize = resultBytes.Length;
                *resultPtr = Marshal.AllocHGlobal(*resultSize);
                Marshal.Copy(resultBytes, 0, *resultPtr, *resultSize);

                return 0; // Success
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                *resultPtr = IntPtr.Zero;
                *resultSize = 0;
                return -1; // Failure
            }
        }

        [UnmanagedCallersOnly(EntryPoint = "FreeMemory")]
        public static void FreeMemory(IntPtr ptr)
        {
            Marshal.FreeHGlobal(ptr);
        }
    }

}