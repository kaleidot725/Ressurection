using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ressurection.Models
{
    interface IProcessService
    {
        IProcessSetting Setting { get; }
        string Name { get; }
        string Path { get; }
        UInt32 RestartCount { get; }
        bool IsActive { get; }
        void Start();
        void Stop();
    }
}
