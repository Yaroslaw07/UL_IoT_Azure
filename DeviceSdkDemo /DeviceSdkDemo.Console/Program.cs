using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using System;
using Microsoft.Azure.Devices.Client;
using DeviceSdkDemo.Device;


var configuration = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

string deviceConnectionString = configuration["HOST_HUB_CONNECTION_STRING"];

if (string.IsNullOrWhiteSpace(deviceConnectionString))
{
    Console.WriteLine("Device connection string not found in user secrets!");
    return;
}


using var deviceClient = DeviceClient.CreateFromConnectionString(deviceConnectionString, TransportType.Mqtt);
await deviceClient.OpenAsync();
var device = new VirtualDevice(deviceClient);

await device.InitializeHandlers();
await device.UpdateTwinAsync();

Console.WriteLine($"Connection success!");

await device.SendMessages(10, 1000);

Console.WriteLine("Finished! Press Enter to close...");
Console.ReadLine();
