using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Fbs.Core.CNEChecks
{
    public enum EducationForm
    { 
        [Description("Очная")]
        Intramural=1,
        [Description("Заочная")]
        Extramural=2,
        [Description("Очно-заочная")]
        Parttime=3

    }
    public enum PaymentSource
    {
        [Description("Из средств бюджета")]
        Budget=1,
        [Description("С полным возмещением затрат")]
        Personal=2,
        [Description("Целевой набор")]
        Targetting=3

    }

    public enum CheckByMarkSumResult
    {
        [Description("Участник ЕГЭ найден")]
        Valid=1,
        [Description("Участник ЕГЭ найден, но сумма баллов неверна")]
        ValidButNotCorrect=2,
        [Description("Участник ЕГЭ найден, но баллы по заданным предметам отсутствуют")]
        ValidButNoResults=3,
        [Description("Участник ЕГЭ не найден")]
        Invalid=4

    }
}
