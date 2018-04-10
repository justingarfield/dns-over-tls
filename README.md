# dns-over-tls

A quick DNS-over-TLS Proxy prototype using C# / .NET Core / .NET Framework / UWP / Windows Services.

My goal is to create something that can run locally as a Windows Service that proxies your local DNS queries to DNS-over-TLS versions, allowing users to encrypt their DNS queries on Windows 10.

Disclaimer: I'm in no way an expert when it comes to socket programming or building high-performance listening servers. I'm sure a lot can be done to optimize my lower-level calls. Since this is a prototype, this is of no concern right now. If you do have ideas and can show my literature to learn from though, I'm all ears!

## Projects

### ConsoleService

This is simply used to run the DNSResolver in the Shared project while Debugging and testing locally. This saves time by not having to go through the entire Windows Installer process for the Windows Service wrapper itself.

### dns-over-tls

Original Universal Windows Program that I wanted to try building this with. Unfortunately, due to the way Network Isolation works in UWP, I was unable to communicate via the loopback or local address, which made it impossible to route local DNS queries this way. I may still entertain UWP for a UI-side component to all of this down the road.

### Shared

NetStandard 2.0 Class Library that contains any functionality that can be shared with other projects. Pretty much using this to house any code that isn't Wrapper / Shell related right now.

### WindowsService

.NET Framework 4.6.1 project that houses the Windows Service related components and code.

## References

A huge thanks goes out to the creators of the content below. These helped spark ideas, fix some bugs, and get this prototype to where it is extremely quick!

* [RFC7858 - DNS over Transport Layer Security (TLS)](https://tools.ietf.org/html/rfc7858)
* [Cloudflare 1.1.1.1 - DNS over TLS](https://developers.cloudflare.com/1.1.1.1/dns-over-tls/)
* [SPKI/SDSI Certificates](http://world.std.com/~cme/html/spki.html)
* [Calculate Public Key Pin (.Net)](https://stackoverflow.com/questions/39441425/calculate-public-key-pin-net)
* [PortQry Command Line Port Scanner Version 2.0](https://www.microsoft.com/en-us/download/details.aspx?id=17148)
* [Creating .NET Core Windows Services](https://stackify.com/creating-net-core-windows-services/)
* [Win32Exception Class](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.win32exception?view=netframework-4.6.1)
* [ServiceBase Class](https://docs.microsoft.com/en-us/dotnet/api/system.serviceprocess.servicebase?view=netframework-4.7.2)
* [ServiceController Class](https://docs.microsoft.com/en-us/dotnet/api/system.serviceprocess.servicecontroller?view=netframework-4.7.2)
* [RFC768 - User Datagram Protocol](https://www.ietf.org/rfc/rfc768.txt)
* [RFC1035 - Domain Implementation and Specification](https://www.ietf.org/rfc/rfc1035.txt)
* [.NET API Browser](https://docs.microsoft.com/en-us/dotnet/api/)
* [Best Practices for System.Net Classes](https://docs.microsoft.com/en-us/dotnet/framework/network-programming/best-practices-for-system-net-classes)
* [How to use SSL in TcpClient class](https://stackoverflow.com/questions/8375013/how-to-use-ssl-in-tcpclient-class)
* [SslStream Sample](https://leastprivilege.com/2005/02/28/sslstream-sample/)
* [How can I display a system tray icon for C# window service.?](https://stackoverflow.com/questions/2652254/how-can-i-display-a-system-tray-icon-for-c-sharp-window-service)
* [UWP APP WITH SYSTRAY EXTENSION](https://stefanwick.com/2017/06/24/uwp-app-with-systray-extension/)
