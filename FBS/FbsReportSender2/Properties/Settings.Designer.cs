//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3074
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FbsReportSender.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("smtp.gmail.com")]
        public string SMTPServer {
            get {
                return ((string)(this["SMTPServer"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool SMTPServer_EnableSSL {
            get {
                return ((bool)(this["SMTPServer_EnableSSL"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("info@fbsege.ru")]
        public string SMTPServer_Login {
            get {
                return ((string)(this["SMTPServer_Login"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("4422u4")]
        public string SMTPServer_Password {
            get {
                return ((string)(this["SMTPServer_Password"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("info@fbsege.ru")]
        public string EmailAddress_From {
            get {
                return ((string)(this["EmailAddress_From"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("587")]
        public int SMTPServer_Port {
            get {
                return ((int)(this["SMTPServer_Port"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Отчеты из Подсистемы ФИС Результаты ЕГЭ")]
        public string ReportName {
            get {
                return ((string)(this["ReportName"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("denis.valeev@gmail.com,dvaleev@armd.ru")]
        public string EmailAddress_To {
            get {
                return ((string)(this["EmailAddress_To"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=zubrus;Initial Catalog=fbs_db;Persist Security Info=True;User ID=fbs;" +
            "Password=fbs;Connection Timeout=3000")]
        public string DBConnection {
            get {
                return ((string)(this["DBConnection"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\r\n\t\t\t\t\t[Отчет о регистрации пользователей за 24 часа],\r\n\t\t\t\t\t[Отчет о загрузках с" +
            "видетельств за 24 часа],\r\n\t\t\t\t\t[Отчет об уникальных проверках свидетельств за 24" +
            " часа],\r\n\t\t\t\t\t[Отчет о наиболее активных организациях за 24 часа]\r\n\t\t\t\t")]
        public string ReportViewListDaily {
            get {
                return ((string)(this["ReportViewListDaily"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\r\n\t\t\t\t\t[Отчет о регистрации пользователей за неделю],\r\n\t\t\t\t\t[Отчет о загрузках св" +
            "идетельств за неделю],\r\n\t\t\t\t\t[Отчет об уникальных проверках свидетельств за неде" +
            "лю],\r\n\t\t\t\t\t[Отчет о наиболее активных организациях за неделю]\r\n\t\t\t\t")]
        public string ReportViewListWeekly {
            get {
                return ((string)(this["ReportViewListWeekly"]));
            }
        }
    }
}
