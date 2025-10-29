# Artfol.cs
Mobile-API for Artfol an application which is more than just an art platform - it's a community where artists can share their work, get feedback, and connect with fellow creators

## Example
```cs
using ArfolApi;

namespace Application
{
    internal class Program
    {
        static async Task Main()
        {
            var api = new Artfol();
            await api.Login("example@gmail.com", "password");
            string accountInfo = await api.GetAccountInfo();
            Console.WriteLine(accountInfo);
        }
    }
}
```
