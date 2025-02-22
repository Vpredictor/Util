using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Util.Http;
using Xunit.DependencyInjection.Logging;

namespace Util.AspNetCore.Tests;

/// <summary>
/// ��������
/// </summary>
public class Startup {
    /// <summary>
    /// ��������
    /// </summary>
    public void ConfigureHost( IHostBuilder hostBuilder ) {
        hostBuilder.ConfigureWebHostDefaults( webHostBuilder => {
            webHostBuilder.UseTestServer()
                .Configure( t => {
                    t.UseRouting();
                    t.UseAuthentication();
                    t.UseAuthorization();
                    t.UseEndpoints( endpoints => {
                        endpoints.MapControllers();
                    } );
                } );
        } )
            .AsBuild()
            .AddAcl()
            .AddUtil();
    }

    /// <summary>
    /// ���÷���
    /// </summary>
    public void ConfigureServices( IServiceCollection services ) {
        services.AddLogging( logBuilder => logBuilder.AddXunitOutput() );
        services.AddControllers();
        services.AddTransient<IHttpClient>( t => {
            var client = new HttpClientService();
            client.SetHttpClient( t.GetService<IHost>().GetTestClient() );
            return client;
        } );
    }
}