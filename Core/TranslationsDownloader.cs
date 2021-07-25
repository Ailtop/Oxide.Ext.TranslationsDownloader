using System.Collections;
using System.IO;
using System.IO.Compression;
using Oxide.Core;
using Oxide.Core.Plugins;
using Oxide.Game.Rust.Libraries;
using UnityEngine;
using UnityEngine.Networking;

namespace Oxide.Ext.TranslationsDownloader
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
            Title = "Translations Downloader";
            Description = "Download Translation Files for Rust";
            Version = new VersionNumber(1, 1, 0);
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
                MainCamera.Instance.StopCoroutine(_downloadCoroutine);
            }
            _downloadCoroutine = MainCamera.Instance.StartCoroutine(DownloadTranslations(initial));
        }

        private IEnumerator DownloadTranslations(bool initial)
        {
            Interface.Oxide.LogWarning("Start downloading the translation files.");
            using (var unityWebRequest = UnityWebRequest.Get(TranslationsDownloadUrl))
            {
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
                    yield return null;
                }

                if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
                {
                    Interface.Oxide.LogError($"Failed to download translations. Code: {unityWebRequest.responseCode}. Error: {unityWebRequest.error}");
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
                _downloaded = true;
            }

            _downloadCoroutine = null;
        }
    }
}