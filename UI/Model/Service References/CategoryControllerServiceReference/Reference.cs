﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TodoSystem.UI.Model.CategoryControllerServiceReference {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Category", Namespace="http://schemas.datacontract.org/2004/07/TodoSystem.Service.Model.Interface")]
    [System.SerializableAttribute()]
    public partial class Category : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Drawing.Color ColorField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int OrderField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Drawing.Color Color {
            get {
                return this.ColorField;
            }
            set {
                if ((this.ColorField.Equals(value) != true)) {
                    this.ColorField = value;
                    this.RaisePropertyChanged("Color");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Order {
            get {
                return this.OrderField;
            }
            set {
                if ((this.OrderField.Equals(value) != true)) {
                    this.OrderField = value;
                    this.RaisePropertyChanged("Order");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="CategoryControllerServiceReference.ICategoryController")]
    public interface ICategoryController {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICategoryController/SelectAll", ReplyAction="http://tempuri.org/ICategoryController/SelectAllResponse")]
        TodoSystem.UI.Model.CategoryControllerServiceReference.Category[] SelectAll();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICategoryController/SelectAll", ReplyAction="http://tempuri.org/ICategoryController/SelectAllResponse")]
        System.Threading.Tasks.Task<TodoSystem.UI.Model.CategoryControllerServiceReference.Category[]> SelectAllAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICategoryController/SelectById", ReplyAction="http://tempuri.org/ICategoryController/SelectByIdResponse")]
        TodoSystem.UI.Model.CategoryControllerServiceReference.Category SelectById(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICategoryController/SelectById", ReplyAction="http://tempuri.org/ICategoryController/SelectByIdResponse")]
        System.Threading.Tasks.Task<TodoSystem.UI.Model.CategoryControllerServiceReference.Category> SelectByIdAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICategoryController/SelectByName", ReplyAction="http://tempuri.org/ICategoryController/SelectByNameResponse")]
        TodoSystem.UI.Model.CategoryControllerServiceReference.Category[] SelectByName(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICategoryController/SelectByName", ReplyAction="http://tempuri.org/ICategoryController/SelectByNameResponse")]
        System.Threading.Tasks.Task<TodoSystem.UI.Model.CategoryControllerServiceReference.Category[]> SelectByNameAsync(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICategoryController/Create", ReplyAction="http://tempuri.org/ICategoryController/CreateResponse")]
        TodoSystem.UI.Model.CategoryControllerServiceReference.Category Create(string name, System.Drawing.Color color, int order);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICategoryController/Create", ReplyAction="http://tempuri.org/ICategoryController/CreateResponse")]
        System.Threading.Tasks.Task<TodoSystem.UI.Model.CategoryControllerServiceReference.Category> CreateAsync(string name, System.Drawing.Color color, int order);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICategoryController/Update", ReplyAction="http://tempuri.org/ICategoryController/UpdateResponse")]
        void Update(TodoSystem.UI.Model.CategoryControllerServiceReference.Category category);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICategoryController/Update", ReplyAction="http://tempuri.org/ICategoryController/UpdateResponse")]
        System.Threading.Tasks.Task UpdateAsync(TodoSystem.UI.Model.CategoryControllerServiceReference.Category category);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICategoryController/Delete", ReplyAction="http://tempuri.org/ICategoryController/DeleteResponse")]
        void Delete(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICategoryController/Delete", ReplyAction="http://tempuri.org/ICategoryController/DeleteResponse")]
        System.Threading.Tasks.Task DeleteAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICategoryController/ChangeOrder", ReplyAction="http://tempuri.org/ICategoryController/ChangeOrderResponse")]
        void ChangeOrder(int id, int order);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICategoryController/ChangeOrder", ReplyAction="http://tempuri.org/ICategoryController/ChangeOrderResponse")]
        System.Threading.Tasks.Task ChangeOrderAsync(int id, int order);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ICategoryControllerChannel : TodoSystem.UI.Model.CategoryControllerServiceReference.ICategoryController, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CategoryControllerClient : System.ServiceModel.ClientBase<TodoSystem.UI.Model.CategoryControllerServiceReference.ICategoryController>, TodoSystem.UI.Model.CategoryControllerServiceReference.ICategoryController {
        
        public CategoryControllerClient() {
        }
        
        public CategoryControllerClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CategoryControllerClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CategoryControllerClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CategoryControllerClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public TodoSystem.UI.Model.CategoryControllerServiceReference.Category[] SelectAll() {
            return base.Channel.SelectAll();
        }
        
        public System.Threading.Tasks.Task<TodoSystem.UI.Model.CategoryControllerServiceReference.Category[]> SelectAllAsync() {
            return base.Channel.SelectAllAsync();
        }
        
        public TodoSystem.UI.Model.CategoryControllerServiceReference.Category SelectById(int id) {
            return base.Channel.SelectById(id);
        }
        
        public System.Threading.Tasks.Task<TodoSystem.UI.Model.CategoryControllerServiceReference.Category> SelectByIdAsync(int id) {
            return base.Channel.SelectByIdAsync(id);
        }
        
        public TodoSystem.UI.Model.CategoryControllerServiceReference.Category[] SelectByName(string name) {
            return base.Channel.SelectByName(name);
        }
        
        public System.Threading.Tasks.Task<TodoSystem.UI.Model.CategoryControllerServiceReference.Category[]> SelectByNameAsync(string name) {
            return base.Channel.SelectByNameAsync(name);
        }
        
        public TodoSystem.UI.Model.CategoryControllerServiceReference.Category Create(string name, System.Drawing.Color color, int order) {
            return base.Channel.Create(name, color, order);
        }
        
        public System.Threading.Tasks.Task<TodoSystem.UI.Model.CategoryControllerServiceReference.Category> CreateAsync(string name, System.Drawing.Color color, int order) {
            return base.Channel.CreateAsync(name, color, order);
        }
        
        public void Update(TodoSystem.UI.Model.CategoryControllerServiceReference.Category category) {
            base.Channel.Update(category);
        }
        
        public System.Threading.Tasks.Task UpdateAsync(TodoSystem.UI.Model.CategoryControllerServiceReference.Category category) {
            return base.Channel.UpdateAsync(category);
        }
        
        public void Delete(int id) {
            base.Channel.Delete(id);
        }
        
        public System.Threading.Tasks.Task DeleteAsync(int id) {
            return base.Channel.DeleteAsync(id);
        }
        
        public void ChangeOrder(int id, int order) {
            base.Channel.ChangeOrder(id, order);
        }
        
        public System.Threading.Tasks.Task ChangeOrderAsync(int id, int order) {
            return base.Channel.ChangeOrderAsync(id, order);
        }
    }
}