using System.Reflection;
using Oxide.Core;
using Oxide.Core.Extensions;

namespace Translations
{
    public class TranslationsExtension : Extension
    {
        //internal static TranslationsExtension Instance;
        //private GameObject _downloaderObj;

        public TranslationsExtension(ExtensionManager manager)
            : base(manager)
        {
        }

        public override string Name => "TranslationsDownloader";
        public override string Author => "Arainrr";
        public override VersionNumber Version => new VersionNumber(1, 0, 1);

        public override bool SupportsReloading => true;

        //public override void LoadPluginWatchers(string pluginDirectory)
        //{
        //    Manager.RegisterPluginLoader(new TranslationsLoader());
        //}

        public override void OnModLoad()
        {
            //Instance = this;
            Manager.RegisterPluginLoader(new TranslationsLoader());
            Interface.Oxide.UnloadExtension(Assembly.GetExecutingAssembly().GetName().Name);
        }

        //public override void OnShutdown()
        //{
        //    Shutdown();
        //    Instance = null;
        //}

        //internal void Initialize()
        //{
        //    MainCamera.Instance.StartCoroutine(DownloadTranslations());
        //    _downloaderObj = new GameObject("TranslationsDownloader");
        //    _downloaderObj.AddComponent<TranslationsDownloader>();
        //    Interface.Oxide.LogError($"Initialize: {_downloaderObj}");
        //}

        //internal void Shutdown()
        //{
        //    Interface.Oxide.LogError($"Shutdown: {_downloaderObj}");
        //    if (_downloaderObj != null)
        //    {
        //        Object.Destroy(_downloaderObj);
        //        _downloaderObj = null;
        //    }
        //}

        //private class TranslationsDownloader : MonoBehaviour
        //{
        //    public void Awake()
        //    {
        //        Interface.Oxide.LogError($"Awake: {this}");
        //        StartCoroutine(DownloadTranslations());
        //    }

        //    private IEnumerator DownloadTranslations()
        //    {
        //        Interface.Oxide.LogWarning("Start downloading all the translation files.");
        //        using (var unityWebRequest = UnityWebRequest.Get(TranslationsUrl))
        //        {
        //            unityWebRequest.timeout = 60;
        //            yield return unityWebRequest.SendWebRequest();
        //            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
        //            {
        //                Interface.Oxide.LogError($"Failed to download translations. Code:{unityWebRequest.responseCode}. Error:{unityWebRequest.error}");
        //                DoDestroy();
        //                yield break;
        //            }

        //            var extractDirectory = Path.Combine(Interface.Oxide.DataFileSystem.Directory, "TranslationsDownloader");
        //            if (!Directory.Exists(extractDirectory))
        //            {
        //                Directory.CreateDirectory(extractDirectory);
        //            }

        //            var translationsZipPath = Path.Combine(Interface.Oxide.DataFileSystem.Directory, "Rust (translations).zip");
        //            File.WriteAllBytes(translationsZipPath, unityWebRequest.downloadHandler.data);
        //            ZipFile.ExtractToDirectory(translationsZipPath, extractDirectory);
        //            File.Delete(translationsZipPath);
        //        }
        //        Interface.Oxide.LogWarning("All translation files were successfully downloaded and extracted.");
        //        DoDestroy();
        //    }

        //    private void DoDestroy()
        //    {
        //        Destroy(this);
        //        Interface.Oxide.UnloadPlugin(nameof(Translations.TranslationsDownloader));
        //        Interface.Oxide.UnloadExtension("Oxide.Ext.TranslationsDownloader");
        //    }

        //    private void OnDestroy()
        //    {
        //        Interface.Oxide.LogError($"OnDestroy: {this}");
        //    }
        //}
    }
}