// --------------------------------------------------------------------------
// <copyright file="ReportLinkCollection.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Razorbill.Module.Common.Configurations
{
    using System.Configuration;

    /// <summary>
    /// Report Link Collection.
    /// </summary>
    public class ReportLinkCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public ReportLinkElement this[int index]
        {
            get
            {
                return (ReportLinkElement)BaseGet(index);
            }

            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }

        public new ReportLinkElement this[string name]
        {
            get
            {
                return (ReportLinkElement)BaseGet(name);
            }
        }

        public void Add(ReportLinkElement element)
        {
            BaseAdd(element);
        }

        public void Remove(ReportLinkElement element)
        {
            if (BaseIndexOf(element) >= 0)
            {
                BaseRemove(element.Name);
            }
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string alias)
        {
            BaseRemove(alias);
        }

        public void Clear()
        {
            BaseClear();
        }

        public int IndexOf(ReportLinkElement element)
        {
            return BaseIndexOf(element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        protected override sealed ConfigurationElement CreateNewElement()
        {
            return new ReportLinkElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ReportLinkElement)element).Name;
        }
    }
}