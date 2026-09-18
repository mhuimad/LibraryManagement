using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Dac;
using Reqnroll;
using Testcontainers.MsSql;

namespace LibraryManagement.IntegrationTests.Hooks;

[Binding]
public static class DatabaseHooks
{
    private static MsSqlContainer? _container;

    public static string ConnectionString { get; private set; } = string.Empty;

    [BeforeTestRun]
    public static async Task StartDatabaseAsync()
    {
        _container = new MsSqlBuilder().Build();
        await _container.StartAsync();

        var masterConnectionString = _container.GetConnectionString();
        var connectionStringBuilder = new SqlConnectionStringBuilder(masterConnectionString)
        {
            InitialCatalog = "LibraryManagement"
        };
        ConnectionString = connectionStringBuilder.ConnectionString;

        var dacpacPath = Path.Combine(
            AppContext.BaseDirectory,
            "..", "..", "..", "..", "..",
            "database", "src", "LibraryManagement.Database.Build", "bin", "Release",
            "LibraryManagement.Database.Build.dacpac");

        var dacpac = DacPackage.Load(Path.GetFullPath(dacpacPath));
        var services = new DacServices(masterConnectionString);
        services.Deploy(dacpac, "LibraryManagement", upgradeExisting: true);
    }

    [AfterTestRun]
    public static async Task StopDatabaseAsync()
    {
        if (_container is not null)
            await _container.DisposeAsync();
    }
}
