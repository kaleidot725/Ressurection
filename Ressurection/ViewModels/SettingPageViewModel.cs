using Microsoft.Practices.Unity;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using Ressurection.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Ressurection.ViewModels
{
    class SettingPageViewModel : BindableBase
    {
        public ProcessManageService ProcessManageService { private get; set; }
        public DelegateCommand SelectCommand { get; set; }
        public DelegateCommand AddCommand { get; set; }
        public DelegateCommand<ProcessServiceViewModel> RemoveCommand { get; set; }

        public string path;
        public string Path
        {
            get { return path; }
            set { SetProperty(ref path, value); }
        }

        public ObservableCollection<ProcessServiceViewModel> processServiceViewModels;
        public ObservableCollection<ProcessServiceViewModel> ProcessServiceViewModels
        {
            get { return processServiceViewModels; }
            set { SetProperty(ref processServiceViewModels, value); }
        }

        public SettingPageViewModel(ProcessManageService ProcessManageService)
        {
            Path = "";
            this.ProcessManageService = ProcessManageService;
            ProcessServiceViewModels = new ObservableCollection<ProcessServiceViewModel>();
            foreach (ProcessService i in ProcessManageService)
            {
                ProcessServiceViewModels.Add(new ProcessServiceViewModel(i));
            }

            SelectCommand = new DelegateCommand(Select);
            AddCommand = new DelegateCommand(Add);
            RemoveCommand = new DelegateCommand<ProcessServiceViewModel>(Remove);
        }

        public void Select()
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                Path = dialog.FileName;
            }
        }

        public void Add()
        {
            if (!File.Exists(this.Path))
                return;

            var processService = new ProcessService(new ProcessSetting(this.Path));
            ProcessManageService.Add(processService);
            ProcessServiceViewModels.Add(new ProcessServiceViewModel(processService));
        }

        public void Remove(ProcessServiceViewModel vm)
        {
            ProcessServiceViewModels.Remove(vm);

            if (vm.ProcessService.IsActive)
                vm.ProcessService.Stop();

            ProcessManageService.Remove(vm.ProcessService);
        }
    }
}
