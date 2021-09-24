namespace FbsUtility
{
	/// <summary>
	/// Предоставляет простой класс предназначенный для хранения 2-х типизированных связанных объектов.
	/// </summary>
	/// <typeparam name="TFirst">Тип первого связанного объекта.</typeparam>
	/// <typeparam name="TSecond">Тип второго связанного объекта.</typeparam>
	public class Pair<TFirst, TSecond>
	{
		#region Readonly & Static Fields

		private readonly TFirst first;
		private readonly TSecond second;

		#endregion

		#region Constructors

		/// <param name="first">Значение первого связанного объекта.</param>
		/// <param name="second">Значение сторого связанного объекта.</param>
		public Pair(TFirst first, TSecond second)
		{
			this.first = first;
			this.second = second;
		}

		#endregion

		#region Instance Properties

		/// <summary>
		/// Возвращает значение первого связанного объекта.
		/// </summary>
		public TFirst First
		{
			get { return this.first; }
		}

		/// <summary>
		/// Возвращает значение второго связанного объекта.
		/// </summary>
		public TSecond Second
		{
			get { return this.second; }
		}

		#endregion
	}
}