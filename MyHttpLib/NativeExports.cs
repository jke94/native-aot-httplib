namespace MyHttpLib
{
    using System;
    using System.Net.Http;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;

    public static class NativeExports
    {
        private static readonly IServiceCollection services = new ServiceCollection()
            .AddHttpClient();

        private static readonly IServiceProvider serviceProvider = services.BuildServiceProvider();

        [UnmanagedCallersOnly(EntryPoint = "MakeHttpGetRequest")]
        public static unsafe int MakeHttpGetRequest(IntPtr urlPtr, IntPtr* resultPtr, int* resultSize, int* ptrHttpStatusCode)
        {
            using var client = serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient();

            try
            {
                string url = Marshal.PtrToStringAnsi(urlPtr);
                var task = Task.Run(() => client.GetAsync(url)).Result;
                var httpStatusCode = (int)task.StatusCode;
                var result = Task.Run(task.Content.ReadAsStringAsync).Result;

                byte[] resultBytes = System.Text.Encoding.UTF8.GetBytes(result);
                *resultSize = resultBytes.Length;
                *resultPtr = Marshal.AllocHGlobal(*resultSize);

                *ptrHttpStatusCode = httpStatusCode;

                Marshal.Copy(resultBytes, 0, *resultPtr, *resultSize);

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                *resultPtr = IntPtr.Zero;
                *resultSize = 0;
                return -1;
            }
        }

        [UnmanagedCallersOnly(EntryPoint = "FreeMemory")]
        public static void FreeMemory(IntPtr ptr)
        {
            Marshal.FreeHGlobal(ptr);
        }
    }

}