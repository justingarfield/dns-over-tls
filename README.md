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
