using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ressurection.Models
{
    class ProcessSetting : IProcessSetting
    {
        public string Path { get; private set; }

        public ProcessSetting(string path)
        {
            this.Path = path;
        }
    }
}
