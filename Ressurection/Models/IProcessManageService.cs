using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ressurection.Models
{
    interface IProcessManageService : IEnumerable
    {
        void Add(IProcessService processService);
        void Remove(IProcessService processService);
    }
}
