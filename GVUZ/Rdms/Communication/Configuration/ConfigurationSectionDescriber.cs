using System.Configuration;

namespace Rdms.Communication.BaseConfigurationService
{
	public class ConfigurationSectionDescriber<TElementType> : ConfigurationSection
		where TElementType : ConfigurationElementDescriber, new()
	{
		[ConfigurationProperty("items", IsDefaultCollection = true)]
		//[ConfigurationCollection(Type.  ConfigurationCollectionDescriber<TElementType>,
			//   AddItemName = "module",
			//   ClearItemsName = "clear",
			//   RemoveItemName = "remove")]
			public ConfigurationCollectionDescriber<TElementType> Items
		{
			get { return (ConfigurationCollectionDescriber<TElementType>) base["items"]; }
		}
	}
}