public class SmartHome
{
    public interface ITemperatureControllable
    {
        void SetTemperature(double temperature);
    }

    public interface IDimmable
    {
        void SetLight(LightType lighttype);
    }

    public interface ISchedulable { }

    public abstract class SmartDevice
    {
        protected SmartDevice(int id, double version, bool ison)
        {
            ID = id;
            FirmwareVersion = version;
            IsOn = ison;
        }

        public int ID { get; protected set; }
        public double FirmwareVersion { get; protected set; }
        public bool IsOn { get; private set; }

        public void TurnOn()
        {
            if (this.IsOn == false)
            {
                this.IsOn = true;
            }
        }

        public void TurnOff()
        {
            if (this.IsOn == true)
            {
                this.IsOn = false;
            }
        }

        public abstract void GetStatusReport();
    }

    public class SmartThermostat(int id, double version, bool ison)
        : SmartDevice(id, version, ison),
            ITemperatureControllable
    {
        protected double Temperature;

        public override void GetStatusReport()
        {
            Console.WriteLine(
                $"My ID is {ID}, firmware version is {FirmwareVersion}, I am {(IsOn ? "Online" : "Offline")}"
            );
        }

        public void SetTemperature(double temp)
        {
            this.Temperature = temp;
        }
    }

    public enum LightType
    {
        ExtraHot,
        VeryWarm,
        SlightlyWarm,
        Neutral,
        SlightlyCold,
        VeryCold,
    }

    public class SmartLamp(int id, double version, bool ison)
        : SmartDevice(id, version, ison),
            IDimmable
    {
        public LightType LType { get; private set; }

        public override void GetStatusReport()
        {
            Console.WriteLine(
                $"My ID is {ID}, firmware version is {FirmwareVersion}, I am {(IsOn ? "Online" : "Offline")}"
            );
        }

        public void SetLight(LightType lighttype)
        {
            this.LType = lighttype;
        }
    }

    public static void Run()
    {
        SmartThermostat termostat_1 = new(67, 98.1, true);
        termostat_1.GetStatusReport();
        SmartLamp lamp_1 = new(13, 54.78, false);
        lamp_1.GetStatusReport();
        lamp_1.TurnOn();
        lamp_1.GetStatusReport();
    }
}
