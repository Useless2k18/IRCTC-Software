// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginViewModel.cs" company="SDCWORLD">
//   Sourodeep Chatterjee
// </copyright>
// <summary>
//   Defines the LoginViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BusinessLogicWPF.ViewModel.LoginAndRegister
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Windows;

    using BusinessLogicWPF.GoogleCloudFireStoreLibrary;
    using BusinessLogicWPF.Helper;
    using BusinessLogicWPF.Model.Json.Creation;
    using BusinessLogicWPF.Properties;

    using Newtonsoft.Json;

    /// <summary>
    /// The login view model.
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// The files.
        /// </summary>
        [NotNull]
        private static readonly string Files = Directory.GetCurrentDirectory();

        /// <summary>
        /// The secret folder.
        /// </summary>
        [NotNull]
        private static readonly string SecretFolder = Path.Combine(Files, "Secret");
        
        public LoginViewModel()
        {
            #region Database with Secrets
            
            var di = new DirectoryInfo(SecretFolder);
            var jsonFiles = di.GetFiles("*.json");
            if (jsonFiles.Length == 0)
            {
                MessageBox.Show(
                    "Please copy the service-account-key.json file into the Secret Folder of the App Directory!\n" +
                    "Navigate to https://console.developers.google.com/apis/credentials to create a service account key\n" + 
                    "Note: Please Remember to give Owner permission to the service account key.\n" + 
                    "Don\'t forget to change your account to uselessgroup2k18@gmail.com first");
                DataHelper.ExitCode = -1;
                Application.Current.Shutdown(DataHelper.ExitCode);
                Process.Start(SecretFolder);
                Process.Start("https://console.developers.google.com/apis/credentials");
                return;
            }

            DataHelper.ExitCode = 0;

            // Google Cloud Platform project ID.
            const string ProjectId = "ticketchecker-d4f79";

            try
            {
                StaticDbContext.ConnectFireStore = new ConnectFireStore(ProjectId, Path.Combine(SecretFolder, jsonFiles[0].FullName));
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                DataHelper.ExitCode = -1;
                Application.Current.Shutdown(DataHelper.ExitCode);
            }

            #region Json Folder Path

            if (DataHelper.JsonFolderPath == null)
            {
                DataHelper.JsonFolderPath = Path.Combine(Files, "Json");
            }

            if (DataHelper.ZoneAndDivisionModel == null)
            {
                try
                {
                    DataHelper.ZoneAndDivisionModel =
                        JsonConvert.DeserializeObject<ZoneAndDivision>(
                            File.ReadAllText(Path.Combine(DataHelper.JsonFolderPath, Resources.ZoneAndDivisionJson)));
                }
                catch (JsonException e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            if (DataHelper.StationsList == null)
            {
                try
                {
                    DataHelper.StationsList = JsonConvert.DeserializeObject<StationsList>(
                        File.ReadAllText(Path.Combine(DataHelper.JsonFolderPath, Resources.StationsListJson)));
                }
                catch (JsonException e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            #endregion

            #endregion
        }
    }
}
