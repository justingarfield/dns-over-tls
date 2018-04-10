using System.ComponentModel;
using System.Configuration.Install;

namespace JGarfield.DnsOverTls.WindowsService
{

    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {

        public ProjectInstaller()
        {
            InitializeComponent();
        }

    }

}
