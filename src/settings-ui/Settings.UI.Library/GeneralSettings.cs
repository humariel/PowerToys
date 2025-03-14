﻿// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.PowerToys.Settings.UI.Library.Interfaces;
using Microsoft.PowerToys.Settings.UI.Library.Utilities;

namespace Microsoft.PowerToys.Settings.UI.Library
{
    public class GeneralSettings : ISettingsConfig
    {
        // Gets or sets a value indicating whether run powertoys on start-up.
        [JsonPropertyName("startup")]
        public bool Startup { get; set; }

        // Gets or sets a value indicating whether the powertoy elevated.
        [JsonPropertyName("is_elevated")]
        public bool IsElevated { get; set; }

        // Gets or sets a value indicating whether powertoys should run elevated.
        [JsonPropertyName("run_elevated")]
        public bool RunElevated { get; set; }

        // Gets or sets a value indicating whether is admin.
        [JsonPropertyName("is_admin")]
        public bool IsAdmin { get; set; }

        // Gets or sets theme Name.
        [JsonPropertyName("theme")]
        public string Theme { get; set; }

        // Gets or sets system theme name.
        [JsonPropertyName("system_theme")]
        public string SystemTheme { get; set; }

        // Gets or sets powertoys version number.
        [JsonPropertyName("powertoys_version")]
        public string PowertoysVersion { get; set; }

        [JsonPropertyName("action_name")]
        public string CustomActionName { get; set; }

        [JsonPropertyName("enabled")]
        public EnabledModules Enabled { get; set; }

        [JsonPropertyName("download_updates_automatically")]
        public bool AutoDownloadUpdates { get; set; }

        [JsonPropertyName("enable_experimentation")]
        public bool EnableExperimentation { get; set; }

        public GeneralSettings()
        {
            Startup = false;
            IsAdmin = false;
            IsElevated = false;
            AutoDownloadUpdates = false;
            EnableExperimentation = true;
            Theme = "system";
            SystemTheme = "light";
            try
            {
                PowertoysVersion = DefaultPowertoysVersion();
            }
            catch (Exception e)
            {
                Logger.LogError("Exception encountered when getting PowerToys version", e);
                PowertoysVersion = "v0.0.0";
            }

            Enabled = new EnabledModules();
            CustomActionName = string.Empty;
        }

        // converts the current to a json string.
        public string ToJsonString()
        {
            return JsonSerializer.Serialize(this);
        }

        private static string DefaultPowertoysVersion()
        {
            return interop.CommonManaged.GetProductVersion();
        }

        // This function is to implement the ISettingsConfig interface.
        // This interface helps in getting the settings configurations.
        public string GetModuleName()
        {
            // The SettingsUtils functions access general settings when the module name is an empty string.
            return string.Empty;
        }

        public bool UpgradeSettingsConfiguration()
        {
            try
            {
                if (Helper.CompareVersions(PowertoysVersion, Helper.GetProductVersion()) != 0)
                {
                    // Update settings
                    PowertoysVersion = Helper.GetProductVersion();
                    return true;
                }
            }
            catch (FormatException)
            {
                // If there is an issue with the version number format, don't migrate settings.
            }

            return false;
        }

        public void AddEnabledModuleChangeNotification(Action callBack)
        {
            Enabled.AddEnabledModuleChangeNotification(callBack);
        }
    }
}
