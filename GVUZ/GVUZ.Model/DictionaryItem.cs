namespace GVUZ.Model
{
	public class DictionaryItem : DictionaryItem<int>
	{
	}

	public class DictionaryItem<T>
	{
		public T ID { get; set; }

		public string Name { get; set; }

		public bool HasChild { get; set; }
	}
}