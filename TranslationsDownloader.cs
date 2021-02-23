using Oxide.Core;
using Oxide.Core.Plugins;

namespace TranslationsDownloader
{
    public class TranslationsDownloader : CSPlugin
    {
        public TranslationsDownloader()
        {
            Title = "TranslationsDownloader";
            Author = "Arainrr";
            Version = new VersionNumber(1, 0, 0);
            Description = "Download TranslationsDownloader";
        }

        [HookMethod("OnTerrainInitialized")]
        private void OnTerrainInitialized()
        {
            Interface.Oxide.LogError("OnTerrainInitialized");
            //foreach (var singletonComponent in UnityEngine.Object.FindObjectsOfType<SingletonComponent>())
            //{
            //    Interface.Oxide.LogError($"{singletonComponent?.name} : {singletonComponent?.GetType()}");
            //}
            TranslationsExtension.Instance.Initialize();
        }
    }
}