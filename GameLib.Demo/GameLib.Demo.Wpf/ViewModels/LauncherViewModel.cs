﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameLib.Core;
using GameLib.Demo.Wpf.Util;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GameLib.Demo.Wpf.ViewModels;

public partial class LauncherViewModel : ViewModelBase
{
    const string CrossImagePath = "/Resources/cross-color.png";
    const string CheckImagePath = "/Resources/check-color.png";

    [ObservableProperty]
    private ObservableCollection<ILauncher> _launchers = default!;

    [ObservableProperty]
    private ILauncher? _selectedLauncher;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string _isInstalledLogo = CrossImagePath;

    [ObservableProperty]
    private string _isRunningLogo = CrossImagePath;

    public LauncherViewModel(LauncherManager launcherManager)
    {
        LoadData(launcherManager);
    }

    private async void LoadData(LauncherManager launcherManager)
    {
        IsLoading = true;
        IEnumerable<ILauncher> launchers = Enumerable.Empty<ILauncher>();

        await Task.Run(() =>
        {
            try
            {
                launchers = launcherManager.GetLaunchers();
            }
            catch { /* ignore */ }
        });

        Launchers = new(launchers);
        SelectedLauncher = Launchers.FirstOrDefault();
        IsLoading = false;
    }

    partial void OnSelectedLauncherChanged(ILauncher? value)
    {
        switch (value)
        {
            case null:
                IsInstalledLogo = CrossImagePath;
                IsRunningLogo = CrossImagePath;
                break;
            default:
                IsInstalledLogo = value.IsInstalled ? CheckImagePath : CrossImagePath;
                IsRunningLogo = value.IsRunning ? CheckImagePath : CrossImagePath;
                break;
        }
    }

    [RelayCommand]
    public void RefreshLauncherIsRunning()
    {
        OnPropertyChanged(nameof(SelectedLauncher));
    }

    [RelayCommand]
    public static void RunLauncher(ILauncher? launcher)
    {
        if (launcher is null || !launcher.IsInstalled)
        {
            return;
        }

        if (launcher.IsRunning)
        {
            FocusUtil.FocusProcess(Path.GetFileNameWithoutExtension(launcher.Executable));
            return;
        }

        Process.Start(launcher.ExecutablePath);
    }

    [RelayCommand]
    public static void OpenPath(ILauncher? launcher)
    {
        switch (launcher)
        {
            case not null when !string.IsNullOrEmpty(launcher.ExecutablePath):
                Process.Start("explorer.exe", $"/select,\"{launcher.ExecutablePath}\"");
                break;

            case not null when !string.IsNullOrEmpty(launcher.InstallDir):
                Process.Start("explorer.exe", launcher.InstallDir);
                break;
        }
    }

    [RelayCommand]
    public static void CopyToClipboard(string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        Clipboard.SetText(text);
    }
}
