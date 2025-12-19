using Microsoft.UI.Xaml;
using System;
using System.Runtime.InteropServices;

namespace JournalApp.WinUI;

public class Program
{
    [DllImport("kernel32.dll")]
    static extern bool AllocConsole();

    [DllImport("kernel32.dll")]
    static extern bool AttachConsole(int dwProcessId);

    [STAThread]
    static void Main(string[] args)
    {
        try
        {
            // Attach to parent console or create new one for debugging
#if DEBUG
            if (!AttachConsole(-1))
                AllocConsole();

            Console.WriteLine("Journal App starting...");
#endif

            WinRT.ComWrappersSupport.InitializeComWrappers();

#if DEBUG
            Console.WriteLine("COM wrappers initialized");
#endif

            Microsoft.UI.Xaml.Application.Start((p) =>
            {
                try
                {
#if DEBUG
                    Console.WriteLine("Application.Start callback executing...");
#endif
                    var context = new Microsoft.UI.Dispatching.DispatcherQueueSynchronizationContext(
                        Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread());
                    System.Threading.SynchronizationContext.SetSynchronizationContext(context);

#if DEBUG
                    Console.WriteLine("Creating App instance...");
#endif
                    _ = new App();

#if DEBUG
                    Console.WriteLine("App instance created successfully");
#endif
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in Application.Start callback: {ex.Message}");
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    }
                    throw;
                }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fatal error: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }
#if DEBUG
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
#endif
            Environment.Exit(1);
        }
    }
}
