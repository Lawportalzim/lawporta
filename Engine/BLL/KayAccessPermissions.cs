using System.Configuration;

namespace Kay.BLL
{
    /// <summary>
    ///	Custom access permissions defined in public.config
    /// </summary>
    public class KayAccessPermissions : ConfigurationSection
    {
        [ConfigurationProperty("paths")]
        public KayAccessPermissionsPathCollection Paths { get { return this["paths"] as KayAccessPermissionsPathCollection; } }
    }

    /// <summary>
    ///	Path elements
    /// </summary>
    public class KayAccessPermissionsPath : ConfigurationElement
    {
        [ConfigurationProperty("path", IsRequired = true)]
        public string Path { get { return this["path"] as string; } }

        [ConfigurationProperty("groups", IsRequired = true)]
        public int Groups { get { return int.Parse(this["groups"].ToString()); } }

        [ConfigurationProperty("loginUrl", IsRequired = true)]
        public string LoginUrl { get { return this["loginUrl"] as string; } }
    }

    /// <summary>
    ///	Path elements collection
    /// </summary>
    public class KayAccessPermissionsPathCollection : ConfigurationElementCollection
    {
        public KayAccessPermissionsPath this[int index]
        {
            get { return base.BaseGet(index) as KayAccessPermissionsPath; }
            set { if (base.BaseGet(index) != null) base.BaseRemoveAt(index); this.BaseAdd(index, value); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new KayAccessPermissionsPath();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((KayAccessPermissionsPath)element).Path;
        }
    }
}