using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using AudioSwitcher.AudioApi.CoreAudio;

namespace SmallAudioManager
{
    class Program
    {
        static public CoreAudioController controller = new CoreAudioController();

        static int print_usage()
        {
            System.Console.WriteLine("usage : " + System.AppDomain.CurrentDomain.FriendlyName + " <Device Name> <Action> <Action-Val>");
            System.Console.WriteLine("or " + System.AppDomain.CurrentDomain.FriendlyName + " <API-Action>");
            System.Console.WriteLine("--------------------------------------");
            System.Console.WriteLine(">Device Name = Input OR Output Device");
            System.Console.WriteLine(">Action = Mute, Toggle, +, -, plus, minus, +<val>, -<val>");
            System.Console.WriteLine(">Action-Val = intval or bool for Mute");
            System.Console.WriteLine("----                              ----");
            System.Console.WriteLine(">API-Action = API-ListAll, API-ListInputs or API-ListOutputs");
            System.Console.WriteLine("Press a key to continue ...");
            System.Console.ReadKey();
            return (-1);
        }

        static int API_List(string Type)
        {
            IEnumerable<CoreAudioDevice> Target;
            if (Type.Equals("API-ListInputs"))
                Target = controller.GetPlaybackDevices();
            if (Type.Equals("API-ListOutputs"))
                Target = controller.GetCaptureDevices();
            else
                Target = controller.GetDevices();
            foreach (CoreAudioDevice device in Target)
            {
                System.Console.WriteLine("Name : " + device.Name + " Type : " + device.DeviceType.ToString());
            }
            return 0;
        }

        static int ManageDevice(string[] args)
        {
            bool warning_flag = false;

            foreach (CoreAudioDevice device in controller.GetDevices())
            {
                if (device.Name.Equals(args[0]))
                {
                    if (warning_flag)
                    {
                        System.Console.WriteLine("Warning, multiple device with same name");
                        System.Console.WriteLine("Press a key to continue ...");
                        System.Console.ReadKey();
                    }
                    if ((args[1].StartsWith("+") || args[1].StartsWith("-")) && args.Length == 2)
                    {
                        int val;
                        if (int.TryParse(args[1], out val))
                            device.Volume += val;
                    }
                    else if (!args[1].Equals("Toggle", StringComparison.OrdinalIgnoreCase)
                                && !args[1].Equals("Mute", StringComparison.OrdinalIgnoreCase)
                                && args.Length < 3)
                        return print_usage();
                    else if (args[1].Equals("+") || args[1].Equals("plus", StringComparison.OrdinalIgnoreCase))
                    {
                        int val;
                        if (int.TryParse(args[2], out val))
                            device.Volume += val;
                    }
                    else if (args[1].Equals("-") || args[1].Equals("minus", StringComparison.OrdinalIgnoreCase))
                    {
                        int val;
                        if (int.TryParse(args[2], out val))
                            device.Volume -= val;
                    }
                    else if (args[1].Equals("Mute", StringComparison.OrdinalIgnoreCase))
                    {
                        bool val;
                        if (bool.TryParse(args[2], out val))
                            device.Mute(val);
                        else
                            device.ToggleMute();
                    }
                    else if (args[1].Equals("Toggle", StringComparison.OrdinalIgnoreCase))
                        device.ToggleMute();
                    else
                        return print_usage();
                    warning_flag = true;
                }
            }
            if (!warning_flag)
            {
                System.Console.WriteLine("Warning, device not found ! Please use an <API-Action> to list your devices");
                System.Console.WriteLine("Press a key to continue ...");
                System.Console.ReadKey();
            }
            return 0;
        }

        static int Main(string[] args)
        {
            if (args.Length == 0)
                return print_usage();
            if (args[0].StartsWith("API"))
                return (API_List(args[0]));
            else if (args.Length > 1)
                return (ManageDevice(args));
            return print_usage();
        }
    }
}
