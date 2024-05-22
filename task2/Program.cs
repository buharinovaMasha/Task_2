using task2.DTO;
using Newtonsoft.Json;
public static class Program
{
    public static List<DeviceInfo> ReadFromFile(string path)
    {
        var devicesInfo = new List<DeviceInfo>();

        if (!File.Exists(path))
        {
            Console.WriteLine("Файл не существует");
            return null;
        }

        var json = File.ReadAllText(path);
        if (!string.IsNullOrEmpty(json))
        {
            devicesInfo = JsonConvert.DeserializeObject<List<DeviceInfo>>(json);
        }
        return devicesInfo;
    }

    public static bool WriteToFile(string path, List<Conflict> conflicts)
    {
        try
        {
            if (string.IsNullOrEmpty(path))
                return false;
            var json = JsonConvert.SerializeObject(conflicts);
            File.WriteAllText(path, json);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public static bool MergeDevicesIfConflict(List<DeviceInfo> devicesInfo, out List<Conflict> conflicts)
    {
        conflicts = new List<Conflict>();
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
        if (conflicts.Any())
            return true;
        return false;
    }

    private static void Main()
    {
        var devicesInfo = ReadFromFile("Devices.json");

        MergeDevicesIfConflict(devicesInfo, out var conflicts);

        WriteToFile("Conflicts.json", conflicts);
    }
}
