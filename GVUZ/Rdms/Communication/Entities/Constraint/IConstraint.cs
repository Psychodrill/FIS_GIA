namespace Rdms.Communication.Entities.Constraint
{
	public interface IConstraint
	{
		/// <summary>
		/// 	Сериализация в строку.
		/// </summary>
		/// <returns></returns>
		string ToString();

		/// <summary>
		/// 	Инициализация значениями из строки.
		/// </summary>
		/// <param name = "s"></param>
		void ParseString(string s);

		/// <summary>
		/// 	Проверяет значение на соответствие ограничению.
		/// </summary>
		/// <param name = "value"></param>
		/// <returns></returns>
		bool CheckValue(object value);
	}
}