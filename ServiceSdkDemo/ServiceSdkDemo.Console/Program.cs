using Microsoft.Azure.Devices;
using ServiceSdkDemo.Lib;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Configuration;


var configuration = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

string serviceConnectionString = configuration["HOST_HUB_CONNECTION_STRING"];

if (string.IsNullOrWhiteSpace(serviceConnectionString))
{
    Console.WriteLine("Device connection string not found in user secrets!");
    return;
}

using var serviceClient = ServiceClient.CreateFromConnectionString(serviceConnectionString);
using var registryManager = RegistryManager.CreateFromConnectionString(serviceConnectionString);

var manager = new IoTHubManager(serviceClient, registryManager);

int input;

do
{
    FeatureSelector.PrintMenu();
    input = FeatureSelector.ReadInput();
    await FeatureSelector.Execute(input, manager);
} while (input != 0);