using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Implementation.ServiceInterfaces;
using Server.Services.DatabaseTravel;
using Server.Services.MapBoxAddresses;

namespace BlazorClient
{
    public class Program
    {
        // TODO: ��-�� ����� ������ ���������� ��������� ����������� �� Server
        // ������� ������� ���� ����� � ����� ����� � ��������� ������, �� ��� ����� �� ����������

        public static async Task Main(string[] args)
        {
            // �������� �������
            var builder = WebAssemblyHostBuilder.CreateDefault(args);


            // ������� ���������
            builder.RootComponents.Add<App>("app");


            // �������
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<IAddressesService, MapBoxAddressesService>(); // � ���� ��� ������ ������������!
            builder.Services.AddScoped<ITravelService, DatabaseTravelService>();


            // ��������������
            AuthFactory.BuildAuthService(builder);


            await builder.Build().RunAsync();
        }
    }


    public static class AuthFactory
    {
        public static void BuildAuthService(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddOidcAuthentication(options =>
            {
                // Configure your authentication provider options here.
                // For more information, see https://aka.ms/blazor-standalone-auth

                // Google console: https://console.cloud.google.com/apis/credentials?project=travel-helper-311009&folder=&organizationId=
                builder.Configuration.Bind("Google", options.ProviderOptions);

                options.ProviderOptions.DefaultScopes.Add("profile");
                options.ProviderOptions.DefaultScopes.Add("email");

            });
        }
    }



}
