using System;
using System.Configuration;

namespace Rdms.Communication.BaseConfigurationService
{
	public class ConfigurationCollectionDescriber<TElementType> : ConfigurationElementCollection
		where TElementType : ConfigurationElementDescriber, new()
	{
		public override ConfigurationElementCollectionType CollectionType
		{
			get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new TElementType();
		}

		protected override Object GetElementKey(ConfigurationElement element)
		{
			return ((TElementType) element).ID;
		}

		public ConfigurationElementDescriber this[int index]
		{
			get { return (TElementType) BaseGet(index); }
			set
			{
				if (BaseGet(index) != null)
				{
					BaseRemoveAt(index);
				}
				BaseAdd(index, value);
			}
		}

		public ConfigurationElementDescriber this[short id]
		{
			get { return (TElementType) BaseGet(id); }
		}

		public int IndexOf(TElementType module)
		{
			return BaseIndexOf(module);
		}

		public void Add(TElementType module)
		{
			BaseAdd(module);
		}

		protected override void BaseAdd(ConfigurationElement element)
		{
			base.BaseAdd(element, false);
		}


		public void Remove(TElementType module)
		{
			if (BaseIndexOf(module) >= 0)
				BaseRemove(module.ID);
		}

		public void RemoveAt(int index)
		{
			BaseRemoveAt(index);
		}

		public void Remove(short id)
		{
			BaseRemove(id);
		}

		public void Clear()
		{
			BaseClear();
		}

		public short GetNewId()
		{
			short max = short.MinValue;
			for (int i = 0; i < Count; i++)
			{
				if (max < (this[i]).ID)
				{
					max = (this[i]).ID;
				}
			}
			return (short) (max + 1);
		}
	}
}