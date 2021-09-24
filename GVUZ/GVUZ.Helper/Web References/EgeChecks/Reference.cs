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

namespace GVUZ.Helper.EgeChecks {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="WSChecksSoap", Namespace="urn:fbs:v2")]
    public partial class WSChecks : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private UserCredentials userCredentialsValueField;
        
        private System.Threading.SendOrPostCallback SingleCheckOperationCompleted;
        
        private System.Threading.SendOrPostCallback BatchCheckOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetBatchCheckResultOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetSingleCheckQuerySampleOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetBatchCheckQuerySampleOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public WSChecks() {
            this.Url = global::GVUZ.Helper.Properties.Settings.Default.GVUZ_Helper_EgeChecks_WSChecks;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public UserCredentials UserCredentialsValue {
            get {
                return this.userCredentialsValueField;
            }
            set {
                this.userCredentialsValueField = value;
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
        public event SingleCheckCompletedEventHandler SingleCheckCompleted;
        
        /// <remarks/>
        public event BatchCheckCompletedEventHandler BatchCheckCompleted;
        
        /// <remarks/>
        public event GetBatchCheckResultCompletedEventHandler GetBatchCheckResultCompleted;
        
        /// <remarks/>
        public event GetSingleCheckQuerySampleCompletedEventHandler GetSingleCheckQuerySampleCompleted;
        
        /// <remarks/>
        public event GetBatchCheckQuerySampleCompletedEventHandler GetBatchCheckQuerySampleCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("UserCredentialsValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:fbs:v2/SingleCheck", RequestNamespace="urn:fbs:v2", ResponseNamespace="urn:fbs:v2", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string SingleCheck(string queryXML) {
            object[] results = this.Invoke("SingleCheck", new object[] {
                        queryXML});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void SingleCheckAsync(string queryXML) {
            this.SingleCheckAsync(queryXML, null);
        }
        
        /// <remarks/>
        public void SingleCheckAsync(string queryXML, object userState) {
            if ((this.SingleCheckOperationCompleted == null)) {
                this.SingleCheckOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSingleCheckOperationCompleted);
            }
            this.InvokeAsync("SingleCheck", new object[] {
                        queryXML}, this.SingleCheckOperationCompleted, userState);
        }
        
        private void OnSingleCheckOperationCompleted(object arg) {
            if ((this.SingleCheckCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SingleCheckCompleted(this, new SingleCheckCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("UserCredentialsValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:fbs:v2/BatchCheck", RequestNamespace="urn:fbs:v2", ResponseNamespace="urn:fbs:v2", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string BatchCheck(string queryXML) {
            object[] results = this.Invoke("BatchCheck", new object[] {
                        queryXML});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void BatchCheckAsync(string queryXML) {
            this.BatchCheckAsync(queryXML, null);
        }
        
        /// <remarks/>
        public void BatchCheckAsync(string queryXML, object userState) {
            if ((this.BatchCheckOperationCompleted == null)) {
                this.BatchCheckOperationCompleted = new System.Threading.SendOrPostCallback(this.OnBatchCheckOperationCompleted);
            }
            this.InvokeAsync("BatchCheck", new object[] {
                        queryXML}, this.BatchCheckOperationCompleted, userState);
        }
        
        private void OnBatchCheckOperationCompleted(object arg) {
            if ((this.BatchCheckCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.BatchCheckCompleted(this, new BatchCheckCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("UserCredentialsValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:fbs:v2/GetBatchCheckResult", RequestNamespace="urn:fbs:v2", ResponseNamespace="urn:fbs:v2", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetBatchCheckResult(string queryXML) {
            object[] results = this.Invoke("GetBatchCheckResult", new object[] {
                        queryXML});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetBatchCheckResultAsync(string queryXML) {
            this.GetBatchCheckResultAsync(queryXML, null);
        }
        
        /// <remarks/>
        public void GetBatchCheckResultAsync(string queryXML, object userState) {
            if ((this.GetBatchCheckResultOperationCompleted == null)) {
                this.GetBatchCheckResultOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetBatchCheckResultOperationCompleted);
            }
            this.InvokeAsync("GetBatchCheckResult", new object[] {
                        queryXML}, this.GetBatchCheckResultOperationCompleted, userState);
        }
        
        private void OnGetBatchCheckResultOperationCompleted(object arg) {
            if ((this.GetBatchCheckResultCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetBatchCheckResultCompleted(this, new GetBatchCheckResultCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:fbs:v2/GetSingleCheckQuerySample", RequestNamespace="urn:fbs:v2", ResponseNamespace="urn:fbs:v2", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetSingleCheckQuerySample() {
            object[] results = this.Invoke("GetSingleCheckQuerySample", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetSingleCheckQuerySampleAsync() {
            this.GetSingleCheckQuerySampleAsync(null);
        }
        
        /// <remarks/>
        public void GetSingleCheckQuerySampleAsync(object userState) {
            if ((this.GetSingleCheckQuerySampleOperationCompleted == null)) {
                this.GetSingleCheckQuerySampleOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetSingleCheckQuerySampleOperationCompleted);
            }
            this.InvokeAsync("GetSingleCheckQuerySample", new object[0], this.GetSingleCheckQuerySampleOperationCompleted, userState);
        }
        
        private void OnGetSingleCheckQuerySampleOperationCompleted(object arg) {
            if ((this.GetSingleCheckQuerySampleCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetSingleCheckQuerySampleCompleted(this, new GetSingleCheckQuerySampleCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:fbs:v2/GetBatchCheckQuerySample", RequestNamespace="urn:fbs:v2", ResponseNamespace="urn:fbs:v2", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetBatchCheckQuerySample() {
            object[] results = this.Invoke("GetBatchCheckQuerySample", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetBatchCheckQuerySampleAsync() {
            this.GetBatchCheckQuerySampleAsync(null);
        }
        
        /// <remarks/>
        public void GetBatchCheckQuerySampleAsync(object userState) {
            if ((this.GetBatchCheckQuerySampleOperationCompleted == null)) {
                this.GetBatchCheckQuerySampleOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetBatchCheckQuerySampleOperationCompleted);
            }
            this.InvokeAsync("GetBatchCheckQuerySample", new object[0], this.GetBatchCheckQuerySampleOperationCompleted, userState);
        }
        
        private void OnGetBatchCheckQuerySampleOperationCompleted(object arg) {
            if ((this.GetBatchCheckQuerySampleCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetBatchCheckQuerySampleCompleted(this, new GetBatchCheckQuerySampleCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:fbs:v2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="urn:fbs:v2", IsNullable=false)]
    public partial class UserCredentials : System.Web.Services.Protocols.SoapHeader {
        
        private string loginField;
        
        private string passwordField;
        
        private string clientField;
        
        private System.Xml.XmlAttribute[] anyAttrField;
        
        /// <remarks/>
        public string Login {
            get {
                return this.loginField;
            }
            set {
                this.loginField = value;
            }
        }
        
        /// <remarks/>
        public string Password {
            get {
                return this.passwordField;
            }
            set {
                this.passwordField = value;
            }
        }
        
        /// <remarks/>
        public string Client {
            get {
                return this.clientField;
            }
            set {
                this.clientField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr {
            get {
                return this.anyAttrField;
            }
            set {
                this.anyAttrField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void SingleCheckCompletedEventHandler(object sender, SingleCheckCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SingleCheckCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SingleCheckCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void BatchCheckCompletedEventHandler(object sender, BatchCheckCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class BatchCheckCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal BatchCheckCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void GetBatchCheckResultCompletedEventHandler(object sender, GetBatchCheckResultCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetBatchCheckResultCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetBatchCheckResultCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void GetSingleCheckQuerySampleCompletedEventHandler(object sender, GetSingleCheckQuerySampleCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetSingleCheckQuerySampleCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetSingleCheckQuerySampleCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void GetBatchCheckQuerySampleCompletedEventHandler(object sender, GetBatchCheckQuerySampleCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetBatchCheckQuerySampleCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetBatchCheckQuerySampleCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591