namespace GVUZ.Model.Entrants
{
	public enum EntranceTestResultSourceEnum
	{
		EgeDocument = 1,
		/// <summary>
        /// Документ о внутреннем испытании ОУ
		/// </summary>
        InstitutionEntranceTest = 2,
        /// <summary>
        /// Диплом обычной олимпиады школьников
        /// </summary>
		OlympicDocument = 3,
		GiaDocument = 4,
	}
}
