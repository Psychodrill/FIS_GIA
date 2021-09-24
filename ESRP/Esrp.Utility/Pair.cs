namespace FbsUtility
{
	/// <summary>
	/// ������������� ������� ����� ��������������� ��� �������� 2-� �������������� ��������� ��������.
	/// </summary>
	/// <typeparam name="TFirst">��� ������� ���������� �������.</typeparam>
	/// <typeparam name="TSecond">��� ������� ���������� �������.</typeparam>
	public class Pair<TFirst, TSecond>
	{
		#region Readonly & Static Fields

		private readonly TFirst first;
		private readonly TSecond second;

		#endregion

		#region Constructors

		/// <param name="first">�������� ������� ���������� �������.</param>
		/// <param name="second">�������� ������� ���������� �������.</param>
		public Pair(TFirst first, TSecond second)
		{
			this.first = first;
			this.second = second;
		}

		#endregion

		#region Instance Properties

		/// <summary>
		/// ���������� �������� ������� ���������� �������.
		/// </summary>
		public TFirst First
		{
			get { return this.first; }
		}

		/// <summary>
		/// ���������� �������� ������� ���������� �������.
		/// </summary>
		public TSecond Second
		{
			get { return this.second; }
		}

		#endregion
	}
}