using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ressurection.ViewModels
{
    class VersionPageViewModel : BindableBase
    {
        private String versionString;
        public String VersionString
        {
            get { return versionString; }
            set { SetProperty(ref versionString, value); }
        }

        public VersionPageViewModel()
        {
            VersionString = Ressurection.Models.Version.Infomation;
        }
    }
}
