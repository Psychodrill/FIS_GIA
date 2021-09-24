namespace Ege.Check.Logic.Services.Dtos.Enums
{
    using System.ComponentModel;

    public enum ProcessCondition
    {
        [Description("Результат в процессе формирования")] Processing = 1,
        [Description("Новый результат")] New = 2,
        [Description("Результат в процессе проверки")] ProcessingCheck = 3,
        [Description("Проверенный результат")] CheckDone = 4,
        [Description("Результат в процессе оценки")] ProcessingResult = 5,
        [Description("Оцененный результат")] ResultDone = 6,
        [Description("Апелляция по процедуре")] AppealsProcedure = 101,
        [Description("Пересдача по двойке")] ReExamBadMark = 111,
        [Description("Удаленный с экзамена")] GoHome = 201,
        [Description("Не закончивший по уважительной причине")] DontComplete = 202,
        [Description("Отменен по письму УККО Рособрнадзора")] Cancel = 203,
        [Description("--")] NotUsed = 204,
        [Description("Пересдача по двойке (ручная отмена)")] ReExamBadMark2 = 205,
        [Description("Решение ГЭК (другое)")] ResultGEK = 206,
        [Description("Апелляция по процедуре (в виде письма)")] AppealsProcedure2 = 207
    }
}