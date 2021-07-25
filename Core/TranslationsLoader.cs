using System;
using Oxide.Core.Plugins;

namespace Oxide.Ext.TranslationsDownloader
{
    public class TranslationsLoader : PluginLoader
    {
        public override Type[] CorePlugins => new[]
        {
            typeof(TranslationsDownloader)
        };
    }
}