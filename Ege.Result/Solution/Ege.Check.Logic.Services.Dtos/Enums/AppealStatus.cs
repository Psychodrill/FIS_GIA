namespace Ege.Check.Logic.Services.Dtos.Enums
{
    using System.ComponentModel;

    public enum AppealStatus
    {
        [Description("Сформировано заявление участником")] CreatedByUser = 0,
        [Description("Сформировано заявление оператором")] CreatedByOperator = 1,
        [Description("Отменена участником")] CanceledByUser = 2,
        [Description("Отменена оператором")] CanceledByOperator = 3,
        [Description("Апелляция открыта участником заново")] RestoredByUser = 4,
        [Description("Апелляция открыта оператором заново")] RestoredByOperator = 5,
        [Description("Сформировано заявление в РЦОИ")] Create = 10,
        [Description("Распечатаны бланки")] PrintedBlanks = 11,
        [Description("Введены данные")] WriteData = 12,
        [Description("На обработке")] OnProcess = 20,
        [Description("Ожидание подтверждения")] WaitingCommit = 30,
        [Description("Введено подтверждение")] Committed = 32,
        [Description("Подтверждение на обработке")] CommitteInProccess = 40,
        [Description("Создано блокирование")] BlockCreate = 52,
        [Description("Блокирование на обработке")] BlockInProccess = 60,
        [Description("Удовлетворена")] Satisfied = 100,
        [Description("Отклонена конфликтной комиссией субъекта РФ")] Rejected = 101,
        [Description("Заблокирована")] Blocked = 103,
        [Description("Задержана")] Delayed = 1000
    }
}