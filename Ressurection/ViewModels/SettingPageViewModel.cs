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
        public ProcessServiceList ProcessManageService { private get; set; }
        public DelegateCommand AddCommand { get; set; }
        public DelegateCommand RemoveCommand { get; set; }

        private ProcessServiceViewModel selectedServiceViewModel;
        public ProcessServiceViewModel SelectedServiceViewModel
        {
            get { return selectedServiceViewModel; }
            set { SetProperty(ref selectedServiceViewModel, value); }
        }

        private ObservableCollection<ProcessServiceViewModel> processServiceViewModels;
        public ObservableCollection<ProcessServiceViewModel> ProcessServiceViewModels
        {
            get { return processServiceViewModels; }
            set { SetProperty(ref processServiceViewModels, value); }
        }

        public SettingPageViewModel(ProcessServiceList ProcessManageService)
        {
            this.ProcessManageService = ProcessManageService;
            ProcessServiceViewModels = new ObservableCollection<ProcessServiceViewModel>();
            foreach (ProcessService i in ProcessManageService)
            {
                ProcessServiceViewModels.Add(new ProcessServiceViewModel(i));
            }

            AddCommand = new DelegateCommand(Add);
            RemoveCommand = new DelegateCommand(Remove, CanRemove);
            RemoveCommand.ObservesProperty(() => SelectedServiceViewModel);
        }

        public void Add()
        {
            try
            {
                var dialog = new OpenFileDialog();

                if (dialog.ShowDialog() == true)
                {
                    var path = dialog.FileName;

                    if (!File.Exists(path))
                        return;

                    var processService = new ProcessService(new ProcessSetting(path));
                    ProcessManageService.Add(processService);
                    ProcessServiceViewModels.Add(new ProcessServiceViewModel(processService));
                }
            }
            catch (Exception) { }
        }

        public bool CanRemove()
        {
            return (SelectedServiceViewModel == null) ? (false) : (true);
        }

        public void Remove()
        {
            try
            {
                if (selectedServiceViewModel.ProcessService.IsActive)
                    selectedServiceViewModel.ProcessService.Stop();

                ProcessManageService.Remove(selectedServiceViewModel.ProcessService);
                ProcessServiceViewModels.Remove(selectedServiceViewModel);
            }
            catch (Exception) { }
        }
    }
}
