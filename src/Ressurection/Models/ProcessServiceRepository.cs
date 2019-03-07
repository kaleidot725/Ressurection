using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ressurection.Models
{
    class ProcessServiceRepository : IProcessServiceRepository
    {
        private List<IProcessService> list = new List<IProcessService>();
        
        public void Add(IProcessService processService)
        {
            list.Add(processService);
        }

        public void Remove(IProcessService processService)
        {
            list.Remove(processService);
        }

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < list.Count; i++)
            {
                yield return list[i];
            }
        }
    }
}
