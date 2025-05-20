using Microsoft.AspNetCore.Antiforgery;

namespace Backend.API.Delegates
{
    public static class ServiceCollectionActions
    {
        public static void SetupAntiForgery(AntiforgeryOptions options)
        {
            options.SuppressXFrameOptionsHeader = true;
        }
    }
}
