using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ressurection.Models
{
    class ProcessService : IProcessService
    {
        public IProcessSetting Setting { get; private set; }
        public string Name { get; private set; }
        public string Path { get; private set; }
        public UInt32 RestartCount { get; private set; }

        public TimeSpan UpTimeSpan
        {
            get
            {
                if (StartTime != null)
                    return TimeSpan.FromSeconds((DateTime.Now - StartTime.Value).TotalSeconds);
                else
                    return TimeSpan.FromSeconds(0);
            }
        }

        public bool IsActive
        {
            get
            {
                if (process == null)
                    return false;

                return process.HasExited ? (false) : (true);
            }
        }

        private Process process;
        private Thread monitor;
        private DateTime? StartTime { get; set; }
        private bool monitoring;

        public ProcessService(IProcessSetting setting)
        {
            if (setting == null)
                throw new ArgumentException("setting");

            if (String.IsNullOrEmpty(setting.Path))
                throw new ArgumentException("setting path");

            if (!File.Exists(setting.Path))
                throw new ArgumentException("setting path");

            this.Setting = setting;
            this.Name = System.IO.Path.GetFileName(setting.Path);
            this.Path = setting.Path;
            this.RestartCount = 0;
        }

        public void Start()
        {
            if (IsActive)
                throw new InvalidOperationException("process already start");

            this.process = Process.Start(this.Path);
            this.monitor = new Thread(Monitroing);
            this.monitoring = true;
            this.monitor.Start();

            this.RestartCount = 0;
            this.StartTime = DateTime.Now;

            while (!IsActive)
                System.Threading.Thread.Sleep(10);
        }

        public void Stop()
        {
            if (!IsActive)
                throw new InvalidOperationException("process already stop");

            this.monitoring = false;
            this.monitor.Join();

            try
            {
                this.process.CloseMainWindow();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            this.process.Dispose();
            this.process = null;
            this.StartTime = null;

            while (IsActive)
                System.Threading.Thread.Sleep(10);
        }

        private void Monitroing()
        {
            while (this.monitoring)
            {
                System.Threading.Thread.Sleep(1000);
                
                if (!this.process.HasExited)
                {
                    continue;
                }

                this.process.Dispose();
                this.process = null;
                this.RestartCount++;
                this.process = Process.Start(this.Path);
                this.StartTime = DateTime.Now;
            }
        }
    }
}
