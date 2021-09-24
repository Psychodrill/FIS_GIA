using System;

namespace GVUZ.Helper.MVC
{
    /// <summary>
    ///     �������, ������� ����� �������� ����������, ����� �� ����������� ���� ��� ��������.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class MenuSectionAttribute : Attribute
    {
        public MenuSectionAttribute(string section)
        {
            if (section == null) throw new ArgumentNullException("section");
            Section = section;
        }

        public string Section { get; private set; }
    }
}