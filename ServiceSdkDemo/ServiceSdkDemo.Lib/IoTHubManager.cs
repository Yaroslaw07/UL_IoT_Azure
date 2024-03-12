namespace ServiceSdkDemo.Lib;
using System.Text;
using Microsoft.Azure.Devices;
using Newtonsoft.Json;

public class IoTHubManager
{
    private readonly ServiceClient client;
    private readonly RegistryManager registryManager;

    public IoTHubManager(ServiceClient client, RegistryManager registryManager)
    {
        this.client = client;
        this.registryManager = registryManager;
    }

    public async Task SendMessage(string messageText, string deviceId)
    {
        var messageBody = new { text = messageText };
        var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageBody)));
        message.MessageId = Guid.NewGuid().ToString();
        await client.SendAsync(deviceId, message);
    }
    public async Task<int> ExecuteDeviceMethod(string methodName, string deviceId)
    {
        var method = new CloudToDeviceMethod(methodName);

        var methodBody = new { nrOfMessages = 5, delay = 500 };
        method.SetPayloadJson(JsonConvert.SerializeObject(methodBody));

        var result = await client.InvokeDeviceMethodAsync(deviceId, method);
        return result.Status;
    }

    public async Task UpdateDesiredTwin(string deviceId, string propertyName, dynamic propertyValue)
    {
        var twin = await registryManager.GetTwinAsync(deviceId);
        twin.Properties.Desired[propertyName] = propertyValue;
        await registryManager.UpdateTwinAsync(twin.DeviceId, twin, twin.ETag);
    }
}