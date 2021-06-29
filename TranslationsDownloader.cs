using System.Collections;
using System.IO;
using System.IO.Compression;
using Oxide.Core;
using Oxide.Core.Plugins;
using Oxide.Game.Rust.Libraries;
using UnityEngine;
using UnityEngine.Networking;

namespace Translations
{
    public class TranslationsDownloader : CSPlugin
    {
        private const string TranslationsDownloadUrl = "https://crowdin.com/backend/download/project/rust.zip";

        private bool _downloaded;
        private Coroutine _downloadCoroutine;
        private readonly Command _cmd = Interface.Oxide.GetLibrary<Command>();

        public TranslationsDownloader()
        {
            Author = "Arainrr";
            Name = "TranslationsDownloader";
            Title = "Rust Translations Downloader";
            Description = "Download Translations";
            Version = new VersionNumber(1, 0, 1);
            _cmd.AddConsoleCommand("translations", this, nameof(CCmdTranslations));
        }

        [HookMethod(nameof(OnTerrainInitialized))]
        private void OnTerrainInitialized()
        {
            if (!_downloaded)
            {
                StartDownload(true);
            }
        }

        [HookMethod(nameof(OnServerInitialized))]
        private void OnServerInitialized(bool initial)
        {
            if (!initial) return;
            //Interface.Oxide.LogError("OnTerrainInitialized");
            //foreach (var singletonComponent in UnityEngine.Object.FindObjectsOfType<SingletonComponent>())
            //{
            //    Interface.Oxide.LogError($"{singletonComponent?.name} : {singletonComponent?.GetType()}");
            //}
            //TranslationsExtension.Instance.Initialize();
            if (!_downloaded)
            {
                StartDownload(true);
            }
        }

        [HookMethod(nameof(CCmdTranslations))]
        private void CCmdTranslations(ConsoleSystem.Arg arg)
        {
            if (!arg.IsAdmin) return;
            if (arg.HasArgs())
            {
                if (_downloadCoroutine != null)
                {
                    MainCamera.Instance.StopCoroutine(_downloadCoroutine);
                    Interface.Oxide.LogWarning("Stop downloading the translation files.");
                    return;
                }
                Interface.Oxide.LogWarning("You haven't started downloading the translation files yet.");
                return;
            }
            StartDownload(false);
        }

        private void StartDownload(bool initial)
        {
            if (_downloadCoroutine != null)
            {
                MainCamera.Instance.StopCoroutine(_downloadCoroutine);//MainCamera
            }
            _downloadCoroutine = MainCamera.Instance.StartCoroutine(DownloadTranslations(initial));
        }

        private IEnumerator DownloadTranslations(bool initial)
        {
            Interface.Oxide.LogWarning("Start downloading the translation files.");
            using (var unityWebRequest = UnityWebRequest.Get(TranslationsDownloadUrl))
            {
                //unityWebRequest.timeout = 120;
                //yield return unityWebRequest.SendWebRequest();
                var asyncOperation = unityWebRequest.SendWebRequest();
                float timer = 0;
                while (!asyncOperation.isDone)
                {
                    timer += Time.deltaTime;
                    if (timer >= 5f)
                    {
                        timer = 0;
                        var downloaded = Mathf.FloorToInt(asyncOperation.progress * 100);
                        Interface.Oxide.LogInfo($"Downloading the translation files: {downloaded}%");
                    }
                    //var downloaded = Mathf.FloorToInt(asyncOperation.progress * 100);
                    //if (downloaded > 0 && downloaded % 10 == 0)
                    //{
                    //    Interface.Oxide.LogInfo($"Downloading the translation files: {downloaded}%");
                    //}
                    yield return null;
                }

                if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
                {
                    Interface.Oxide.LogError($"Failed to download translations. Code: {unityWebRequest.responseCode}. Error: {unityWebRequest.error}");
                    //Interface.Oxide.LogInfo("Translations will download again in 10 seconds");
                    //DoDestroy();
                    _downloadCoroutine = null;
                    yield break;
                }
                var extractDirectory = Path.Combine(Interface.Oxide.DataFileSystem.Directory, "Translations");
                if (!Directory.Exists(extractDirectory))
                {
                    Directory.CreateDirectory(extractDirectory);
                }

                var translationsZipPath = Path.Combine(Interface.Oxide.DataFileSystem.Directory, "Rust (translations).zip");
                File.WriteAllBytes(translationsZipPath, unityWebRequest.downloadHandler.data);
                ZipFile.ExtractToDirectory(translationsZipPath, extractDirectory, true);
                //File.Delete(translationsZipPath);
                Interface.Oxide.LogWarning($"Translation files were successfully downloaded({unityWebRequest.downloadedBytes / (1024f * 1024f):0.00}MB) and extracted to '{extractDirectory}'.");
                Interface.CallHook("OnTranslationsDownloaded", initial);
                //OnDownloaded();
                _downloaded = true;
            }

            _downloadCoroutine = null;
            //Interface.Oxide.LogError("Failed to download translations...");
        }

        //public static void DoDestroy()
        //{
        //    //Interface.Oxide.UnloadPlugin(nameof(TranslationsDownloader));
        //    //Interface.Oxide.UnloadExtension("Oxide.Ext.TranslationsDownloader");
        //}

        //public static void OnDownloaded()
        //{
        //}
    }
}