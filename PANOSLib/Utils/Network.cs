namespace PANOS
{
    using System;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;

    public static class Network
    {
        public static IPAddress GetLocalIPv4(NetworkInterfaceType interfaceType)
        {
            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.NetworkInterfaceType == interfaceType && networkInterface.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in networkInterface.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            return ip.Address;
                        }
                    }
                }
            }

            throw new Exception("Unable to determine local IP Address");
        }
    }
}
