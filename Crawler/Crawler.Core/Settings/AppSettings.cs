﻿namespace Crawler.Core.Settings;

public sealed class AppSettings
{
    public DbSettings DbSettings { get; set; }

    public ExternalUrlsSettings ExternalUrlsSettings { get; set; }

    public string ConnectionString => 
        $"Host={DbSettings.Host};Port={DbSettings.Port};Database={DbSettings.Database};Username={DbSettings.Username};Password={DbSettings.Password}";
}

