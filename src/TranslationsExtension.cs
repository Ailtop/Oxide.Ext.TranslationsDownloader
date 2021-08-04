using System.Reflection;
using Oxide.Core;
using Oxide.Core.Extensions;

namespace Oxide.Ext.TranslationsDownloader
{
    public class TranslationsExtension : Extension
    {
        public TranslationsExtension(ExtensionManager manager)
            : base(manager)
        {
        }

        public override string Name => "TranslationsDownloader";
        public override string Author => "Arainrr";
        public override VersionNumber Version => new VersionNumber(1, 1, 0);

        public override bool SupportsReloading => true;

        public override void OnModLoad()
        {
            Manager.RegisterPluginLoader(new TranslationsLoader());
            Interface.Oxide.UnloadExtension(Assembly.GetExecutingAssembly().GetName().Name);
        }
    }
}