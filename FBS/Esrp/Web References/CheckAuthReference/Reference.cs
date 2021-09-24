﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace Esrp.CheckAuthReference {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="CheckAuthSoap", Namespace="urn:ersp:v1")]
    public partial class CheckAuth : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback CheckUserAccessOperationCompleted;
        
        private System.Threading.SendOrPostCallback CheckUserTicketOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public CheckAuth() {
            this.Url = global::Esrp.Properties.Settings.Default.Esrp_CheckAuthReference_CheckAuth;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event CheckUserAccessCompletedEventHandler CheckUserAccessCompleted;
        
        /// <remarks/>
        public event CheckUserTicketCompletedEventHandler CheckUserTicketCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:ersp:v1/CheckUserAccess", RequestNamespace="urn:ersp:v1", ResponseNamespace="urn:ersp:v1", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public UserCheckResult CheckUserAccess(string userLogin, string userPassword, int systemID) {
            object[] results = this.Invoke("CheckUserAccess", new object[] {
                        userLogin,
                        userPassword,
                        systemID});
            return ((UserCheckResult)(results[0]));
        }
        
        /// <remarks/>
        public void CheckUserAccessAsync(string userLogin, string userPassword, int systemID) {
            this.CheckUserAccessAsync(userLogin, userPassword, systemID, null);
        }
        
        /// <remarks/>
        public void CheckUserAccessAsync(string userLogin, string userPassword, int systemID, object userState) {
            if ((this.CheckUserAccessOperationCompleted == null)) {
                this.CheckUserAccessOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCheckUserAccessOperationCompleted);
            }
            this.InvokeAsync("CheckUserAccess", new object[] {
                        userLogin,
                        userPassword,
                        systemID}, this.CheckUserAccessOperationCompleted, userState);
        }
        
        private void OnCheckUserAccessOperationCompleted(object arg) {
            if ((this.CheckUserAccessCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CheckUserAccessCompleted(this, new CheckUserAccessCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:ersp:v1/CheckUserTicket", RequestNamespace="urn:ersp:v1", ResponseNamespace="urn:ersp:v1", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public UserCheckResult CheckUserTicket(string login, System.Guid receivedUID, int systemID) {
            object[] results = this.Invoke("CheckUserTicket", new object[] {
                        login,
                        receivedUID,
                        systemID});
            return ((UserCheckResult)(results[0]));
        }
        
        /// <remarks/>
        public void CheckUserTicketAsync(string login, System.Guid receivedUID, int systemID) {
            this.CheckUserTicketAsync(login, receivedUID, systemID, null);
        }
        
        /// <remarks/>
        public void CheckUserTicketAsync(string login, System.Guid receivedUID, int systemID, object userState) {
            if ((this.CheckUserTicketOperationCompleted == null)) {
                this.CheckUserTicketOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCheckUserTicketOperationCompleted);
            }
            this.InvokeAsync("CheckUserTicket", new object[] {
                        login,
                        receivedUID,
                        systemID}, this.CheckUserTicketOperationCompleted, userState);
        }
        
        private void OnCheckUserTicketOperationCompleted(object arg) {
            if ((this.CheckUserTicketCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CheckUserTicketCompleted(this, new CheckUserTicketCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:ersp:v1")]
    public partial class UserCheckResult {
        
        private int statusIDField;
        
        private string loginField;
        
        /// <remarks/>
        public int StatusID {
            get {
                return this.statusIDField;
            }
            set {
                this.statusIDField = value;
            }
        }
        
        /// <remarks/>
        public string Login {
            get {
                return this.loginField;
            }
            set {
                this.loginField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    public delegate void CheckUserAccessCompletedEventHandler(object sender, CheckUserAccessCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CheckUserAccessCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CheckUserAccessCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public UserCheckResult Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((UserCheckResult)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    public delegate void CheckUserTicketCompletedEventHandler(object sender, CheckUserTicketCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CheckUserTicketCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CheckUserTicketCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public UserCheckResult Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((UserCheckResult)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591