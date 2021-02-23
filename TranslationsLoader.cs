using System;
using Oxide.Core.Plugins;

namespace TranslationsDownloader
{
    internal class TranslationsLoader : PluginLoader
    {
        public override Type[] CorePlugins => new[]
        {
            typeof(TranslationsDownloader)
        };
    }
}