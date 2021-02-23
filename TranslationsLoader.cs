using System;
using Oxide.Core.Plugins;

namespace Translations
{
    public class TranslationsLoader : PluginLoader
    {
        public override Type[] CorePlugins => new[]
        {
            typeof(TranslationsDownloader)
        };
    }
}