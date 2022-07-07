﻿using System.Diagnostics;

namespace GameLib.Plugin.Gog.Model;

public class GogGame : IGame
{
    #region Interface implementations
    public string GameId { get; internal set; } = string.Empty;
    public string GameName { get; internal set; } = string.Empty;
    public string InstallDir { get; internal set; } = string.Empty;
    public string ExecutablePath { get; internal set; } = string.Empty;
    public string Executable { get; internal set; } = string.Empty;
    public string WorkingDir { get; internal set; } = string.Empty;
    public string LaunchString { get; internal set; } = string.Empty;
    public DateTime InstallDate { get; internal set; } = DateTime.MinValue;
    public bool IsRunning =>
        !string.IsNullOrEmpty(Executable) &&
        Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(Executable)).Where(p => !p.HasExited).Any();
    #endregion

    public string BuildId { get; internal set; } = string.Empty;
    public string DependsOn { get; internal set; } = string.Empty;
    public string Dlc { get; internal set; } = string.Empty;
    public string InstallerLanguage { get; internal set; } = string.Empty;
    public string LangCode { get; internal set; } = string.Empty;
    public string Language { get; internal set; } = string.Empty;
    public string LaunchCommand { get; internal set; } = string.Empty;
    public string LaunchParam { get; internal set; } = string.Empty;
    public string Path { get; internal set; } = string.Empty;
    public string ProductId { get; internal set; } = string.Empty;
    public string StartMenu { get; internal set; } = string.Empty;
    public string StartMenuLink { get; internal set; } = string.Empty;
    public string SupportLink { get; internal set; } = string.Empty;
    public string UninstallCommand { get; internal set; } = string.Empty;
    public string Version { get; internal set; } = string.Empty;
}
