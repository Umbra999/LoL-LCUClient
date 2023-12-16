using System.Diagnostics;
using System.Text;

namespace Hexed.Wrappers
{
    internal class Utils
    {
        public static Process GetProcessByName(string Name)
        {
            Process[] AllProcesses = Process.GetProcessesByName(Name);
            if (AllProcesses != null && AllProcesses.Length > 0) return AllProcesses[0];

            return null;
        }

        public static string ToBase64(string Data)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(Data);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
