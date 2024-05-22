using task2.DTO;
using Newtonsoft.Json;

var conflicts = new List<Conflict>();

var pathDevices = "Devices.json";
if (!File.Exists(pathDevices))
{
    Console.WriteLine("Файл не существует");
    return;
}

var devicesInfo = new List<DeviceInfo>();
var json=File.ReadAllText(pathDevices);
if (!string.IsNullOrEmpty(json))
{
    devicesInfo=JsonConvert.DeserializeObject<List<DeviceInfo>>(json);
}

foreach (var device in devicesInfo)
    foreach (var device1 in devicesInfo)
        if (device.Device.SerialNumber != device1.Device.SerialNumber &&
            device.Brigade.Code == device1.Brigade.Code && (device.Device.IsOnline ||
            device1.Device.IsOnline))
        {
            var conflict = new Conflict
            {
                BrigadeCode = device1.Brigade.Code,
                DevicesSerials = new string[] { device.Device.SerialNumber, device1.Device.SerialNumber }
            };
            conflicts.Add(conflict);
        }
var pathConflicts = "Conflicts.json";
json= JsonConvert.SerializeObject(conflicts);
File.WriteAllText(pathConflicts, json);
