using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using Ressurection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reactive.Bindings.Extensions;
using System.Threading;

namespace Ressurection.ViewModels
{
    class ProcessServiceViewModel : BindableBase
    {
        public ProcessService ProcessService;
        public DelegateCommand RunCommand { get; set; }

        private String name;
        public String Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private bool onoff;
        public bool Onoff
        {
            get { return onoff; }
            set { SetProperty(ref onoff, value); }
        }

        private String upTime;
        public String UpTime
        {
            get { return upTime; }
            set { SetProperty(ref upTime, value); }
        }

        private String restartCount;
        public String RestartCount
        {
            get { return restartCount; }
            set
            {
                SetProperty(ref restartCount, value);
            }
        }

        private Timer updateTimer;

        public ProcessServiceViewModel(ProcessService processService)
        {
            Onoff = processService.IsActive;
            this.ProcessService = processService;

            updateTimer = new System.Threading.Timer(Update, null, 0, 100);
            RunCommand = new DelegateCommand(Run);
        }

        ~ProcessServiceViewModel()
        {
            updateTimer.Dispose();
        }

        public void Run()
        {
            if (this.Onoff)
            {
                this.Onoff = true;
                ProcessService.Start();
            }
            else
            {
                this.Onoff = false;
                try { ProcessService.Stop(); }
                catch (Exception) { }
            }
        }

        public void Update(object obj)
        {
            this.Name = ProcessService.Name;

            var span = ProcessService.UpTimeSpan;
            this.UpTime = String.Format("{0:D2}d {1:D2}h {2:D2}m {3:D2}s", span.Days, span.Hours, span.Minutes, span.Seconds);
        }
    }
}
