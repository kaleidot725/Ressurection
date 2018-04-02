using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Prism.Mvvm;
using Ressurection.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace Ressurection
{
    public partial class NotifyIconWrapper : Component
    {
        readonly String settingPath = @".\setting.json";
        UnityContainer unityContainer;
        Shell shell = null;

        public NotifyIconWrapper()
        {
            InitializeComponent();

            unityContainer = new UnityContainer();
            unityContainer.RegisterType<Shell>();
            unityContainer.RegisterType<ProcessServiceList>(new ContainerControlledLifetimeManager());
            ViewModelLocationProvider.SetDefaultViewModelFactory(x => unityContainer.Resolve(x));

            var settings = new List<ProcessSetting>();
            if (System.IO.File.Exists(settingPath))
            {
                using (var stream = new System.IO.StreamReader(settingPath))
                {
                    var str = stream.ReadToEnd();
                    settings = JsonConvert.DeserializeObject<List<ProcessSetting>>(str);

                    if (settings != null)
                    {
                        var pm = unityContainer.Resolve<ProcessServiceList>();
                        foreach (var setting in settings)
                        {
                            try
                            {
                                var p = new ProcessService(setting) as IProcessService;
                                p.Start();
                                pm.Add(p);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        }
                    }
                }
            }

            this.toolStripMenuItem_Open.Click += this.toolStripMenuShowSettingClick;
            this.toolStripMenuItem_Exit.Click += this.toolStropMenuExitClick;
        }

        private void toolStripMenuShowSettingClick(object sender, EventArgs e)
        {
            if (shell == null)
            {
                this.shell = this.unityContainer.Resolve<Shell>();
                this.shell.Closed += windowClosed;
                this.shell.Show();
            }
            else
            {
                this.shell.Activate();
            }
        }

        private void toolStropMenuExitClick(object sender, EventArgs e)
        {
            var pm = unityContainer.Resolve<ProcessServiceList>();
            foreach (IProcessService p in pm)
            {
                if (p.IsActive)
                {
                    p.Stop();
                }
            }

            try
            {
                using (var stream = new System.IO.StreamWriter(settingPath, false, Encoding.UTF8))
                {
                    var settings = new List<IProcessSetting>();
                    foreach (IProcessService p in pm)
                    {
                        settings.Add(p.Setting);
                    }

                    var serialize = JsonConvert.SerializeObject(settings, Formatting.Indented);
                    stream.Write(serialize);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Application.Current.Shutdown();
        }

        private void windowClosed(object sender, EventArgs args)
        {
            this.shell = null;
        }
    }
}
