﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.8.1.0
//      SpecFlow Generator Version:1.8.0.0
//      Runtime Version:4.0.30319.269
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace FBS.Tests.FBSCheckRegNumber
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.8.1.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Запрос по регистрационному номеру и баллам по предметам")]
    public partial class ЗапросПоРегистрационномуНомеруИБалламПоПредметамFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "FBSCheckRegNumber.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("ru-RU"), "Запрос по регистрационному номеру и баллам по предметам", "", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 3
#line 4
 testRunner.Given("Я захожу под пользователем \"super\"");
#line 5
 testRunner.And("Я открываю страницу Запрос по регистрационному номеру и баллам по предметам");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("01. Проверка открытия страницы с Запрос по регистрационному номеру и баллам по пр" +
            "едметам")]
        public virtual void _01_ПроверкаОткрытияСтраницыСЗапросПоРегистрационномуНомеруИБалламПоПредметам()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("01. Проверка открытия страницы с Запрос по регистрационному номеру и баллам по пр" +
                    "едметам", ((string[])(null)));
#line 7
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 8
 testRunner.Then("нахожусь на странице Запрос по регистрационному номеру и баллам по предметам");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("02. Проверка присутствия хлебных крошек на странице Запрос по регистрационному но" +
            "меру и баллам по предметам")]
        public virtual void _02_ПроверкаПрисутствияХлебныхКрошекНаСтраницеЗапросПоРегистрационномуНомеруИБалламПоПредметам()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("02. Проверка присутствия хлебных крошек на странице Запрос по регистрационному но" +
                    "меру и баллам по предметам", ((string[])(null)));
#line 10
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Text"});
            table1.AddRow(new string[] {
                        "Свидетельства"});
            table1.AddRow(new string[] {
                        "Свидетельства ЕГЭ"});
            table1.AddRow(new string[] {
                        "Запрос по регистрационному номеру и баллам по предметам"});
#line 11
 testRunner.Then("на странице есть следующие хлебные крошки:", ((string)(null)), table1);
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("03. Проверка полей по умолчанию")]
        public virtual void _03_ПроверкаПолейПоУмолчанию()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("03. Проверка полей по умолчанию", ((string[])(null)));
#line 17
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Value"});
            table2.AddRow(new string[] {
                        "cNumber",
                        "Номер свидетельства"});
            table2.AddRow(new string[] {
                        "rpSubjects_ctl01_txtValue",
                        ""});
            table2.AddRow(new string[] {
                        "rpSubjects_ctl02_txtValue",
                        ""});
            table2.AddRow(new string[] {
                        "rpSubjects_ctl03_txtValue",
                        ""});
            table2.AddRow(new string[] {
                        "rpSubjects_ctl04_txtValue",
                        ""});
            table2.AddRow(new string[] {
                        "rpSubjects_ctl05_txtValue",
                        ""});
            table2.AddRow(new string[] {
                        "rpSubjects_ctl06_txtValue",
                        ""});
            table2.AddRow(new string[] {
                        "rpSubjects_ctl07_txtValue",
                        ""});
            table2.AddRow(new string[] {
                        "rpSubjects_ctl08_txtValue",
                        ""});
            table2.AddRow(new string[] {
                        "rpSubjects_ctl09_txtValue",
                        ""});
            table2.AddRow(new string[] {
                        "rpSubjects_ctl10_txtValue",
                        ""});
            table2.AddRow(new string[] {
                        "rpSubjects_ctl11_txtValue",
                        ""});
            table2.AddRow(new string[] {
                        "rpSubjects_ctl12_txtValue",
                        ""});
            table2.AddRow(new string[] {
                        "rpSubjects_ctl13_txtValue",
                        ""});
            table2.AddRow(new string[] {
                        "rpSubjects_ctl14_txtValue",
                        ""});
#line 18
 testRunner.Then("вижу в полях следующие данные:", ((string)(null)), table2);
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("04. Проверка обязательный полей")]
        public virtual void _04_ПроверкаОбязательныйПолей()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("04. Проверка обязательный полей", ((string[])(null)));
#line 36
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Value"});
            table3.AddRow(new string[] {
                        "txtNumber",
                        ""});
            table3.AddRow(new string[] {
                        "rpSubjects_ctl01_txtValue",
                        ""});
            table3.AddRow(new string[] {
                        "rpSubjects_ctl02_txtValue",
                        ""});
            table3.AddRow(new string[] {
                        "rpSubjects_ctl03_txtValue",
                        ""});
            table3.AddRow(new string[] {
                        "rpSubjects_ctl04_txtValue",
                        ""});
            table3.AddRow(new string[] {
                        "rpSubjects_ctl05_txtValue",
                        ""});
            table3.AddRow(new string[] {
                        "rpSubjects_ctl06_txtValue",
                        ""});
            table3.AddRow(new string[] {
                        "rpSubjects_ctl07_txtValue",
                        ""});
            table3.AddRow(new string[] {
                        "rpSubjects_ctl08_txtValue",
                        ""});
            table3.AddRow(new string[] {
                        "rpSubjects_ctl09_txtValue",
                        ""});
            table3.AddRow(new string[] {
                        "rpSubjects_ctl10_txtValue",
                        ""});
            table3.AddRow(new string[] {
                        "rpSubjects_ctl11_txtValue",
                        ""});
            table3.AddRow(new string[] {
                        "rpSubjects_ctl12_txtValue",
                        ""});
            table3.AddRow(new string[] {
                        "rpSubjects_ctl13_txtValue",
                        ""});
            table3.AddRow(new string[] {
                        "rpSubjects_ctl14_txtValue",
                        ""});
#line 37
 testRunner.When("вношу в поля следующие данные:", ((string)(null)), table3);
#line 54
 testRunner.And("нажимаю кнопку \"Проверить\"");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Value"});
            table4.AddRow(new string[] {
                        "Произошли следующие ошибки:"});
            table4.AddRow(new string[] {
                        "Поле \"Номер свидетельства\" обязательно для заполнения"});
#line 55
 testRunner.Then("на экране есть:", ((string)(null)), table4);
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("05. Проверка обязательности заполнения полей с баллами")]
        public virtual void _05_ПроверкаОбязательностиЗаполненияПолейСБаллами()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("05. Проверка обязательности заполнения полей с баллами", ((string[])(null)));
#line 60
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Value"});
            table5.AddRow(new string[] {
                        "txtNumber",
                        "16-000027009-10"});
            table5.AddRow(new string[] {
                        "rpSubjects_ctl01_txtValue",
                        ""});
            table5.AddRow(new string[] {
                        "rpSubjects_ctl02_txtValue",
                        ""});
            table5.AddRow(new string[] {
                        "rpSubjects_ctl03_txtValue",
                        ""});
            table5.AddRow(new string[] {
                        "rpSubjects_ctl04_txtValue",
                        ""});
            table5.AddRow(new string[] {
                        "rpSubjects_ctl05_txtValue",
                        ""});
            table5.AddRow(new string[] {
                        "rpSubjects_ctl06_txtValue",
                        ""});
            table5.AddRow(new string[] {
                        "rpSubjects_ctl07_txtValue",
                        ""});
            table5.AddRow(new string[] {
                        "rpSubjects_ctl08_txtValue",
                        ""});
            table5.AddRow(new string[] {
                        "rpSubjects_ctl09_txtValue",
                        ""});
            table5.AddRow(new string[] {
                        "rpSubjects_ctl10_txtValue",
                        ""});
            table5.AddRow(new string[] {
                        "rpSubjects_ctl11_txtValue",
                        ""});
            table5.AddRow(new string[] {
                        "rpSubjects_ctl12_txtValue",
                        ""});
            table5.AddRow(new string[] {
                        "rpSubjects_ctl13_txtValue",
                        ""});
            table5.AddRow(new string[] {
                        "rpSubjects_ctl14_txtValue",
                        ""});
#line 61
 testRunner.When("вношу в поля следующие данные:", ((string)(null)), table5);
#line 78
 testRunner.And("нажимаю кнопку \"Проверить\"");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Value"});
            table6.AddRow(new string[] {
                        "Произошли следующие ошибки:"});
            table6.AddRow(new string[] {
                        "Укажите баллы хотя бы по двум предметам"});
#line 79
 testRunner.Then("на экране есть:", ((string)(null)), table6);
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("06. Проверка очистки полей с помощью кнопки \"Очистить\"")]
        public virtual void _06_ПроверкаОчисткиПолейСПомощьюКнопкиОчистить()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("06. Проверка очистки полей с помощью кнопки \"Очистить\"", ((string[])(null)));
#line 84
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Value"});
            table7.AddRow(new string[] {
                        "cNumber",
                        "16-000027009-10"});
            table7.AddRow(new string[] {
                        "rpSubjects_ctl01_txtValue",
                        "44"});
            table7.AddRow(new string[] {
                        "rpSubjects_ctl02_txtValue",
                        "44"});
            table7.AddRow(new string[] {
                        "rpSubjects_ctl03_txtValue",
                        "44"});
            table7.AddRow(new string[] {
                        "rpSubjects_ctl04_txtValue",
                        "55"});
            table7.AddRow(new string[] {
                        "rpSubjects_ctl05_txtValue",
                        "66"});
            table7.AddRow(new string[] {
                        "rpSubjects_ctl06_txtValue",
                        "77"});
            table7.AddRow(new string[] {
                        "rpSubjects_ctl07_txtValue",
                        "88"});
            table7.AddRow(new string[] {
                        "rpSubjects_ctl08_txtValue",
                        "99"});
            table7.AddRow(new string[] {
                        "rpSubjects_ctl09_txtValue",
                        "99"});
            table7.AddRow(new string[] {
                        "rpSubjects_ctl10_txtValue",
                        "10"});
            table7.AddRow(new string[] {
                        "rpSubjects_ctl11_txtValue",
                        "11"});
            table7.AddRow(new string[] {
                        "rpSubjects_ctl12_txtValue",
                        "10"});
            table7.AddRow(new string[] {
                        "rpSubjects_ctl13_txtValue",
                        "11"});
            table7.AddRow(new string[] {
                        "rpSubjects_ctl14_txtValue",
                        "10"});
#line 85
 testRunner.When("вношу в поля следующие данные:", ((string)(null)), table7);
#line 102
 testRunner.And("нажимаю кнопку \"Очистить\"");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Value"});
            table8.AddRow(new string[] {
                        "cNumber",
                        "Номер свидетельства"});
            table8.AddRow(new string[] {
                        "rpSubjects_ctl01_txtValue",
                        ""});
            table8.AddRow(new string[] {
                        "rpSubjects_ctl02_txtValue",
                        ""});
            table8.AddRow(new string[] {
                        "rpSubjects_ctl03_txtValue",
                        ""});
            table8.AddRow(new string[] {
                        "rpSubjects_ctl04_txtValue",
                        ""});
            table8.AddRow(new string[] {
                        "rpSubjects_ctl05_txtValue",
                        ""});
            table8.AddRow(new string[] {
                        "rpSubjects_ctl06_txtValue",
                        ""});
            table8.AddRow(new string[] {
                        "rpSubjects_ctl07_txtValue",
                        ""});
            table8.AddRow(new string[] {
                        "rpSubjects_ctl08_txtValue",
                        ""});
            table8.AddRow(new string[] {
                        "rpSubjects_ctl09_txtValue",
                        ""});
            table8.AddRow(new string[] {
                        "rpSubjects_ctl10_txtValue",
                        ""});
            table8.AddRow(new string[] {
                        "rpSubjects_ctl11_txtValue",
                        ""});
            table8.AddRow(new string[] {
                        "rpSubjects_ctl12_txtValue",
                        ""});
            table8.AddRow(new string[] {
                        "rpSubjects_ctl13_txtValue",
                        ""});
            table8.AddRow(new string[] {
                        "rpSubjects_ctl14_txtValue",
                        ""});
#line 103
 testRunner.Then("вижу в полях следующие данные:", ((string)(null)), table8);
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("07. Проверка результата, если внесены верные данные")]
        public virtual void _07_ПроверкаРезультатаЕслиВнесеныВерныеДанные()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("07. Проверка результата, если внесены верные данные", ((string[])(null)));
#line 121
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Value"});
            table9.AddRow(new string[] {
                        "txtNumber",
                        "16-000027009-10"});
            table9.AddRow(new string[] {
                        "rpSubjects_ctl03_txtValue",
                        "44"});
            table9.AddRow(new string[] {
                        "rpSubjects_ctl05_txtValue",
                        "70"});
#line 122
 testRunner.When("вношу в поля следующие данные:", ((string)(null)), table9);
#line 127
 testRunner.And("нажимаю кнопку \"Проверить\"");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "Value"});
            table10.AddRow(new string[] {
                        "Номер свидетельства"});
            table10.AddRow(new string[] {
                        "16-000027009-10"});
            table10.AddRow(new string[] {
                        "Типографский номер"});
            table10.AddRow(new string[] {
                        "6856345"});
            table10.AddRow(new string[] {
                        "Документ, удостоверяющий личность"});
            table10.AddRow(new string[] {
                        "9205 527439"});
            table10.AddRow(new string[] {
                        "Регион"});
            table10.AddRow(new string[] {
                        "Республика Татарстан (Татарстан)"});
            table10.AddRow(new string[] {
                        "Год выдачи свидетельства"});
            table10.AddRow(new string[] {
                        "2010"});
            table10.AddRow(new string[] {
                        "Проверки"});
            table10.AddRow(new string[] {
                        "1"});
            table10.AddRow(new string[] {
                        "Статус свидетельства"});
            table10.AddRow(new string[] {
                        "Истек срок"});
#line 128
 testRunner.Then("на экране есть:", ((string)(null)), table10);
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "Value"});
            table11.AddRow(new string[] {
                        "Предмет"});
            table11.AddRow(new string[] {
                        "Заявлено"});
            table11.AddRow(new string[] {
                        "В базе"});
            table11.AddRow(new string[] {
                        "Апелляция"});
            table11.AddRow(new string[] {
                        "ФИЗИКА"});
            table11.AddRow(new string[] {
                        "!44,0"});
            table11.AddRow(new string[] {
                        "!44,0"});
            table11.AddRow(new string[] {
                        "Да"});
            table11.AddRow(new string[] {
                        "БИОЛОГИЯ"});
            table11.AddRow(new string[] {
                        "!70,0"});
            table11.AddRow(new string[] {
                        "!70,0"});
            table11.AddRow(new string[] {
                        "Нет"});
#line 144
 testRunner.And("на экране есть:", ((string)(null)), table11);
#line hidden
            TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                        "Value"});
            table12.AddRow(new string[] {
                        "Количество уникальных проверок свидетельства ВУЗами и их филиалами. Для пользоват" +
                            "елей ССУЗов данное поле носит справочный характер."});
#line 158
 testRunner.And("на экране есть:", ((string)(null)), table12);
#line hidden
            TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                        "Value"});
            table13.AddRow(new string[] {
                        "В 2011-2012 годах в свидетельства о результатах ЕГЭ баллы ниже установленного Рос" +
                            "обрнадзором минимального, не выставляются."});
            table13.AddRow(new string[] {
                        "С Приказами Рособрнадзора о применении шкалы минимальных баллов за соответствующи" +
                            "е годы можно ознакомиться в разделе «Документы»."});
            table13.AddRow(new string[] {
                        "Срок действия свидетельств ЕГЭ определяется в соответствии с действующими нормати" +
                            "вными документами."});
            table13.AddRow(new string[] {
                        "В 2012 году действительны свидетельства 2011 и 2012 годов."});
            table13.AddRow(new string[] {
                        "Свидетельства 2010 года действительны только для лиц, проходивших военную службу " +
                            "по призыву и уволенных с военной службы."});
            table13.AddRow(new string[] {
                        "С нормативными документами можно ознакомиться в разделе «Документы»."});
#line 161
 testRunner.And("на экране есть:", ((string)(null)), table13);
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("08. Проверка результата \"Не найдено\"")]
        public virtual void _08_ПроверкаРезультатаНеНайдено()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("08. Проверка результата \"Не найдено\"", ((string[])(null)));
#line 170
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table14 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Value"});
            table14.AddRow(new string[] {
                        "txtNumber",
                        "16-000027009-10"});
            table14.AddRow(new string[] {
                        "rpSubjects_ctl01_txtValue",
                        "1"});
            table14.AddRow(new string[] {
                        "rpSubjects_ctl02_txtValue",
                        "2"});
            table14.AddRow(new string[] {
                        "rpSubjects_ctl03_txtValue",
                        "3"});
#line 171
 testRunner.When("вношу в поля следующие данные:", ((string)(null)), table14);
#line 177
 testRunner.And("нажимаю кнопку \"Проверить\"");
#line hidden
            TechTalk.SpecFlow.Table table15 = new TechTalk.SpecFlow.Table(new string[] {
                        "Value"});
            table15.AddRow(new string[] {
                        "По Вашему запросу ничего не найдено"});
#line 178
 testRunner.Then("на экране есть:", ((string)(null)), table15);
#line hidden
            TechTalk.SpecFlow.Table table16 = new TechTalk.SpecFlow.Table(new string[] {
                        "Value"});
            table16.AddRow(new string[] {
                        "Количество уникальных проверок свидетельства ВУЗами и их филиалами. Для пользоват" +
                            "елей ССУЗов данное поле носит справочный характер."});
#line 181
 testRunner.And("на экране есть:", ((string)(null)), table16);
#line hidden
            TechTalk.SpecFlow.Table table17 = new TechTalk.SpecFlow.Table(new string[] {
                        "Value"});
            table17.AddRow(new string[] {
                        "В 2011-2012 годах в свидетельства о результатах ЕГЭ баллы ниже установленного Рос" +
                            "обрнадзором минимального, не выставляются."});
            table17.AddRow(new string[] {
                        "С Приказами Рособрнадзора о применении шкалы минимальных баллов за соответствующи" +
                            "е годы можно ознакомиться в разделе «Документы»."});
            table17.AddRow(new string[] {
                        "Срок действия свидетельств ЕГЭ определяется в соответствии с действующими нормати" +
                            "вными документами."});
            table17.AddRow(new string[] {
                        "В 2012 году действительны свидетельства 2011 и 2012 годов."});
            table17.AddRow(new string[] {
                        "Свидетельства 2010 года действительны только для лиц, проходивших военную службу " +
                            "по призыву и уволенных с военной службы."});
            table17.AddRow(new string[] {
                        "С нормативными документами можно ознакомиться в разделе «Документы»."});
#line 184
 testRunner.And("на экране есть:", ((string)(null)), table17);
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("09. Проверка отображения раздела \"Другие свидетельства\"")]
        public virtual void _09_ПроверкаОтображенияРазделаДругиеСвидетельства()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("09. Проверка отображения раздела \"Другие свидетельства\"", ((string[])(null)));
#line 193
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table18 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Value"});
            table18.AddRow(new string[] {
                        "txtNumber",
                        "77-000057923-11"});
            table18.AddRow(new string[] {
                        "rpSubjects_ctl06_txtValue",
                        "38"});
            table18.AddRow(new string[] {
                        "rpSubjects_ctl11_txtValue",
                        "56"});
#line 194
 testRunner.When("вношу в поля следующие данные:", ((string)(null)), table18);
#line 199
 testRunner.And("нажимаю кнопку \"Проверить\"");
#line hidden
            TechTalk.SpecFlow.Table table19 = new TechTalk.SpecFlow.Table(new string[] {
                        "Value"});
            table19.AddRow(new string[] {
                        "Другие свидетельства"});
#line 200
 testRunner.Then("на экране есть:", ((string)(null)), table19);
#line hidden
            TechTalk.SpecFlow.Table table20 = new TechTalk.SpecFlow.Table(new string[] {
                        "Value"});
            table20.AddRow(new string[] {
                        "Свидетельство"});
            table20.AddRow(new string[] {
                        "Год"});
            table20.AddRow(new string[] {
                        "Статус"});
            table20.AddRow(new string[] {
                        "77-000045432-11"});
            table20.AddRow(new string[] {
                        "2011"});
            table20.AddRow(new string[] {
                        "Аннулировано"});
            table20.AddRow(new string[] {
                        "77-000057880-11"});
            table20.AddRow(new string[] {
                        "2011"});
            table20.AddRow(new string[] {
                        "Аннулировано"});
#line 203
 testRunner.And("на экране есть таблица \"gvCertificateList\" со значениями:", ((string)(null)), table20);
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("10.  Проверка правильности ввода данных в поле Номер свидетельства")]
        public virtual void _10_ПроверкаПравильностиВводаДанныхВПолеНомерСвидетельства()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("10.  Проверка правильности ввода данных в поле Номер свидетельства", ((string[])(null)));
#line 215
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table21 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Value"});
            table21.AddRow(new string[] {
                        "txtNumber",
                        "Номер свидетельства"});
            table21.AddRow(new string[] {
                        "rpSubjects_ctl01_txtValue",
                        ""});
            table21.AddRow(new string[] {
                        "rpSubjects_ctl02_txtValue",
                        ""});
            table21.AddRow(new string[] {
                        "rpSubjects_ctl03_txtValue",
                        ""});
            table21.AddRow(new string[] {
                        "rpSubjects_ctl04_txtValue",
                        ""});
            table21.AddRow(new string[] {
                        "rpSubjects_ctl05_txtValue",
                        ""});
            table21.AddRow(new string[] {
                        "rpSubjects_ctl06_txtValue",
                        ""});
            table21.AddRow(new string[] {
                        "rpSubjects_ctl07_txtValue",
                        ""});
            table21.AddRow(new string[] {
                        "rpSubjects_ctl08_txtValue",
                        ""});
            table21.AddRow(new string[] {
                        "rpSubjects_ctl09_txtValue",
                        ""});
            table21.AddRow(new string[] {
                        "rpSubjects_ctl10_txtValue",
                        ""});
            table21.AddRow(new string[] {
                        "rpSubjects_ctl11_txtValue",
                        ""});
            table21.AddRow(new string[] {
                        "rpSubjects_ctl12_txtValue",
                        ""});
            table21.AddRow(new string[] {
                        "rpSubjects_ctl13_txtValue",
                        ""});
            table21.AddRow(new string[] {
                        "rpSubjects_ctl14_txtValue",
                        ""});
#line 216
 testRunner.When("вношу в поля следующие данные:", ((string)(null)), table21);
#line 233
 testRunner.And("нажимаю кнопку \"Проверить\"");
#line hidden
            TechTalk.SpecFlow.Table table22 = new TechTalk.SpecFlow.Table(new string[] {
                        "Value"});
            table22.AddRow(new string[] {
                        "Произошли следующие ошибки:"});
            table22.AddRow(new string[] {
                        "Номер свидетельства должен быть в формате XX-XXXXXXXXX-XX"});
#line 234
 testRunner.Then("на экране есть:", ((string)(null)), table22);
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("11. Проверка правильности заполнения поля Баллы")]
        public virtual void _11_ПроверкаПравильностиЗаполненияПоляБаллы()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("11. Проверка правильности заполнения поля Баллы", ((string[])(null)));
#line 239
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table23 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name",
                        "Value"});
            table23.AddRow(new string[] {
                        "txtNumber",
                        "16-000027009-10"});
            table23.AddRow(new string[] {
                        "rpSubjects_ctl01_txtValue",
                        "test"});
            table23.AddRow(new string[] {
                        "rpSubjects_ctl02_txtValue",
                        ""});
            table23.AddRow(new string[] {
                        "rpSubjects_ctl03_txtValue",
                        ""});
            table23.AddRow(new string[] {
                        "rpSubjects_ctl04_txtValue",
                        ""});
            table23.AddRow(new string[] {
                        "rpSubjects_ctl05_txtValue",
                        ""});
            table23.AddRow(new string[] {
                        "rpSubjects_ctl06_txtValue",
                        ""});
            table23.AddRow(new string[] {
                        "rpSubjects_ctl07_txtValue",
                        ""});
            table23.AddRow(new string[] {
                        "rpSubjects_ctl08_txtValue",
                        ""});
            table23.AddRow(new string[] {
                        "rpSubjects_ctl09_txtValue",
                        ""});
            table23.AddRow(new string[] {
                        "rpSubjects_ctl10_txtValue",
                        ""});
            table23.AddRow(new string[] {
                        "rpSubjects_ctl11_txtValue",
                        ""});
            table23.AddRow(new string[] {
                        "rpSubjects_ctl12_txtValue",
                        ""});
            table23.AddRow(new string[] {
                        "rpSubjects_ctl13_txtValue",
                        ""});
            table23.AddRow(new string[] {
                        "rpSubjects_ctl14_txtValue",
                        ""});
#line 240
 testRunner.When("вношу в поля следующие данные:", ((string)(null)), table23);
#line 257
 testRunner.And("нажимаю кнопку \"Проверить\"");
#line hidden
            TechTalk.SpecFlow.Table table24 = new TechTalk.SpecFlow.Table(new string[] {
                        "Value"});
            table24.AddRow(new string[] {
                        "Произошли следующие ошибки:"});
            table24.AddRow(new string[] {
                        "Проверьте правильность заполнения баллов"});
#line 258
 testRunner.Then("на экране есть:", ((string)(null)), table24);
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
