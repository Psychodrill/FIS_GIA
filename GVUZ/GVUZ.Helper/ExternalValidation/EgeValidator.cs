using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services.Protocols;
using GVUZ.Helper.EgeChecks;
using Microsoft.Practices.ServiceLocation;

namespace GVUZ.Helper.ExternalValidation
{
    /// <summary>
    ///     ���������� ���������� ��� ����� ������ ���.
    /// </summary>
    /// <remarks>
    ///     ��� �������� (�������, ���, ��������, ����� ��-��, ����� ��������, ����� ��������):
    ///     "����������", "�����", "����������", "21-234567008-10", "4619", "513316"
    ///     "������", "�����", "����������", "21-234567007-10", "5751", "29581"
    ///     "���������", "������", "�������������", "21-234567009-10", "8537", "156651"
    /// </remarks>
    public class EgeValidator
    {
        public const int ServiceTimeout = 100000;

        /// <summary>
        ///     ��������� ���� ������������� � ���. ������������ � ������.
        ///     ���������� ������ ������, ���� ��������� ��������� ������������� ������������� �� ��� ��� ������ � ��������.
        /// </summary>
        //[Obsolete]
        public IEnumerable<string> Validate(EgePacket packet, EgeResult expectedResult)
        {
            if (packet == null) throw new ArgumentNullException("packet");
            if (expectedResult == null) throw new ArgumentNullException("expectedResult");
            EgeResult result;
            using (var wsChecks = new WSChecks {Timeout = ServiceTimeout})
            {
                try
                {
                    result = EgeInformationProvider.GetResultWithAuthentication(wsChecks, packet);
                }
                catch (SoapException)
                {
                    return new[] {Messages.EgeValidator_FbsCallFailed};
                }
                if (result.Errors != null && result.Errors.Count > 0)
                    return result.Errors;
            }

            if (result.Certificates.Count == 0)
                return new[] {"������������� � ��� �� ������� � ��� ���."};

            bool isStatusError;
            return
                expectedResult.Certificates[0].Validate(result.Certificates[0], out isStatusError).Select(x => x.Error);
        }

        /// <summary>
        ///     ���������� ����� �� ������� �������� ���.
        /// </summary>
        public EgeResultAndStatus GetEgeInformation(EgePacket packet)
        {
            return ServiceLocator.Current.GetInstance<IEgeInformationProvider>().GetEgeInformation(packet);
        }
    }
}