using System.Configuration;

namespace Kay.BLL
{
    /// <summary>
    ///	Gets SSL protected paths from public.config
    /// </summary>
    public class KaySslProtection : ConfigurationSection
    {
        [ConfigurationProperty("on", DefaultValue = "false", IsRequired = false)]
        public bool On { get { return (this["on"].ToString().ToLower() == "true"); } }

        [ConfigurationProperty("paths")]
        public KaySslProtectionPathCollection Paths { get { return this["paths"] as KaySslProtectionPathCollection; } }
    }

    /// <summary>
    ///	Path elements
    /// </summary>
    public class KaySslProtectionPath : ConfigurationElement
    {
        [ConfigurationProperty("path", IsRequired = true)]
        public string Path { get { return this["path"] as string; } }

        [ConfigurationProperty("force", IsRequired = false)]
        public bool ForceSsl { get { return this["force"].ToString().ToLower() != "false"; } }
    }

    /// <summary>
    ///	Path elements collection
    /// </summary>
    public class KaySslProtectionPathCollection : ConfigurationElementCollection
    {
        public KaySslProtectionPath this[int index]
        {
            get { return base.BaseGet(index) as KaySslProtectionPath; }
            set { if (base.BaseGet(index) != null) base.BaseRemoveAt(index); this.BaseAdd(index, value); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new KaySslProtectionPath();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((KaySslProtectionPath)element).Path;
        }
    }
}