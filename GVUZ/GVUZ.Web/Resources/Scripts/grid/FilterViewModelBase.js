;var DataBoundGrid = (function (__module) {

    /* Общий класс для фильтров (типа, скрыть/показать, применен, найти, очистить) */
    function FilterViewModelBase(data, customDataMapping) {
        /* текущие ПРИМЕНЕННЫЕ значения фильтра, которые сохраняются по кнопке Найти (применить фильтр) 
        изначально null, т.к. предполагается, что аргумент data содержит значения по-умолчанию, 
        т.е. изначально фильтр не применен (пустой)
        */
        this.appliedValues = ko.observable(null);

        /* признак того, что примененный фильтр в this.appliedValues() заполнен значениями по-умолчанию 
        изначально true, т.к. предполагается что аргумент data при вызове конструктора FilterViewModelBase 
        содержит значения по-умолчанию (пустой фильтр)
        */
        this.isDefault = ko.observable(true);

        /* признак видимости полей фильтра (скрыть/показать) */
        this.isVisible = ko.observable(false);

        /* текст, отражающий состояние видимости фильтра - значение, вычисляемое через ko.computed(...) */
        this.visibleText = ko.computed(function () {
            return this.isVisible() ? 'Скрыть фильтр' : 'Отобразить фильтр';
        }, this); /* для ko.computed не забываем передавать scope (this) чтобы можно было использовать this в области видимости самой функции (которая return this.isVisible()) */


        /* установка состояния значений фильтра в соответствие с данными, приехавшими с сервера (аргумент data в функции FilterViewModelBase)
        процедура маппинга создает свойства, соответствующие серверной view-модели фильтра в текущем инстансе javascript-класса (this)
        например если серверная C# view-модель имеет вид 
        public class UncheckedApplicationsFilterViewModel
        {
        public string ApplicationNumber {get;set;}
        }
        то после маппинга в текущем инстансе добавится свойство ApplicationNumber (в виде ko.observable), 
        изначально содержащее значение свойства ApplicationNumber, которое было в серверной view-модели
                 
        это и есть те самые свойства, названия которых прописываются в атрибутах data-bind=... 
        и именно их значения пользователь меняет через контролы
        */
        ko.mapping.fromJS(data, customDataMapping || {}, this);
        this.applyFilter();
    }

    FilterViewModelBase.prototype = {
        /* метод, изменяющий состояние видимости полей фильтра, можно привязывать к click-биндингу для div c текстом скрыть/показать */
        toggleVisible: function () {
            this.isVisible(!this.isVisible());
        },

        /* применяем фильтр: копируем текущие значения, заданные пользователем в контролах в appliedValues (через авто-mapping) */
        applyFilter: function () {
            this.appliedValues(ko.mapping.toJS(this, this.defaultValuesMap));
            this.isDefault(!this.checkDirty());
        },

        /* сбрасываем фильтр: устанавливаем текущие значения (те, что привязаны к контролам) в дефолтные,
        зануляем примененные ранее значения (по кнопке Найти)
        устанавливаем признак что фильтр содержит все значения по-умолчанию
        */
        resetFilter: function () {
            ko.mapping.fromJS(this.defaultValues, this.defaultValuesMap, this);
            this.appliedValues(null);
            this.isDefault(true);
        },
        /* возвращает true если хотя бы одно из текущих значений, введенных пользователем в контролах отличается от дефолтного
        возвращает false все текущие значения совпадают с дефолтными */
        checkDirty: function () {
            var applied = this.appliedValues();
            if (applied != null && this.defaultValues != null) {
                for (var p in this.defaultValues) {
                    if (applied[p] && ko.utils.unwrapObservable(applied[p]) !== ko.utils.unwrapObservable(this.defaultValues[p])) {
                        return true;
                    }
                }
            }

            return false;
        }
    };

    WebUtils.extend(__module, { FilterViewModelBase: FilterViewModelBase });
    return __module;

})(DataBoundGrid || {});