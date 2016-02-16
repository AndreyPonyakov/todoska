# todoska
Sample application for students

# Table of contents:

* [Preview](#preview)
* [Server](#server)
* [Client](#client)
* [Transfer layer](#transfer-layer)
* [Storage layer](#storage-layer)
* [Data transfer objects](#data-transfer-objects)
* [Service contracts](#service-contracts)

# Preview

![Preview](/Preview.png)

# Server

WCF-server with the console host.
To the storage data it's used SQL Compact Edition 4.0.

|Project name|Description|
|:--|:--|
|Host|Service bootstapper|
|Model.Interface|Service contract|
|Model.Implementation|Implementation of the service contract|
|Model.SqlCE|Data storage layer|
|Model.Implementation.Test|Unit tests to the service implementation|
|Model.SqlCE.Test|Unit test to the data storage layer|


# Client

WCF-client with WPF GUI.

|Project name|Description|
|:--|:--|
|Runner|Client bootstapper|
|Model|WCF-client|
|ViewModel|Mediator between model and view|
|View|User interface|

# Transfer layer

## CategoryController

|Property|Value|
|:--|:--|
|Service name|CategoryController|
|Contract|ICategoryController|
|Binding|basicHttpBinding|
|Base address|localhost:8733/Design_Time_Addresses/Host/CategoryController|

## TodoController

|Property|Value|
|:--|:--|
|Service name|TodoController|
|Contract|ITodoController|
|Binding|basicHttpBinding|
|Base address|localhost:8733/Design_Time_Addresses/Host/TodoController|

# Storage layer

## Category

|Name|DataType|Nullable|Description|
|:--|:-:|:-:|:--|
|Id|INT|False|Autoincrement, primary key|
|Name|NVARCHAR(100)|False||
|Color|INT|True||
|Order|INT|False||

## Todo

|Name|DataType|Nullable|Description|
|:--|:-:|:-:|:--|
|Id|INT|False|Autoincrement, primary key|
|Title|NVARCHAR(100)|False||
|Desc|NVARCHAR(200)|True||
|Deadline|DATETIME|True||
|CategoryId|INT|True|Foreign key|
|Checked|BIT|False|Default - 0|
|Order|INT|False||

* Foreign key Category.Id = Todo.CategoryId (0..1 - *), no action on delete.
* To convert from Drawing.Color to INT it's used ```Color.ToArgb``` method.
* Order uses INT as orderable type. It's admited to have identical values and client reordering.

# Data transfer objects
## Category
A category which allows to group the different todoes.

Members:

|Property name|Type|Nallable|Required|Description|
|:--|:--|:-:|:-:|:--|
|Id|```int```|False|True|Primary key|
|Name|```string```|False|True|Category name|
|Color|```Color?```|True|False|Preferable color of category|
|Order|```int```|False|False|Priority of category in list|

## Todo
A todo item.

Members:

|Property name|Type|Nallable|Required|Description|
|:--|:--|:-:|:-:|:--|
|Id|```int```|False|True|Primary key|
|Title|```string```|False|True|Short text title|
|Desc|```string```|True|False|Long text body|
|Deadline|```DateTime?```|True|False|Deadline time|
|Checked|```bool```|False|False|Checked status of todo (default - false)|
|CategoryId|```int?```|True|False|Primary key of attached category (foreign key)|
|Order|```int```|False|False|Priority of todo in list|

# Service contracts

## ICategoryController
Category controller contract

Methods:
* [SelectAll](#icategorycontroller-selectall)
* [SelectById](#icategorycontroller-selectbyid)
* [SelectByName](#icategorycontroller-selectbyname)
* [Create](#icategorycontroller-create)
* [Delete](#icategorycontroller-delete)
* [ChangeText](#icategorycontroller-changetext)
* [ChangeOrder](#icategorycontroller-changeorder)
* [ChangeColor](#icategorycontroller-changecolor)

## ITodoController
Todo controller contract

Methods:
* [SelectAll](#itodocontroller-selectall)
* [SelectById](#itodocontroller-selectbyid)
* [SelectByTitle](#itodocontroller-selectbytitle)
* [SelectByCategory](#itodocontroller-selectbycategory)
* [Create](#itodocontroller-create)
* [Delete](#itodocontroller-delete)
* [ChangeText](#itodocontroller-changetext)
* [ChangeOrder](#itodocontroller-changeorder)
* [SetDeadline](#itodocontroller-setdeadline)
* [Check](#itodocontroller-check)
* [SetCategory](#itodocontroller-setcategory)

# Controllers description
## ICategoryController SelectAll
Gets full list of categories.

Declaration:
```
[OperationContract]
IEnumerable<Category> SelectAll();
```

Output type: ```IEnumerable<Category>```
* Returns empty list if there no categories.


## ICategoryController SelectById
Fetches single category by primary key.

Declaration:
```
[OperationContract]
Category SelectById(int id);
```
Arguments:

|Name|Type|Nallable|Description|
|:--|:--|:-:|:-:|:--|
|id|```int```|False|Primary key of category|

Output type: ```Category```
* Returns ```null``` if category with the target primary key doesn't exist

## ICategoryController SelectByName
Fetches category list having the target name.

Declaration:
```
[OperationContract]
IEnumerable<Category> SelectByName(string name);
```
Arguments:

|Name|Type|Nallable|Description|
|:--|:--|:-:|:-:|:--|
|name|```string```|True|Target name|

Output type: ```IEnumerable<Category>```
* Returns an empty list if there no categories.

## ICategoryController Create
Creates a new category with target attributes.

Declaration:
```
[OperationContract]
[FaultContract(typeof(DataValidationFault))]
Category Create(string name);
```
Arguments:

|Name|Type|Nallable|Description|
|:--|:--|:-:|:-:|:--|
|name|```string```|False|Category's name|

Output type: ```Category```
* Returns a category instance with the primary key and last order value.

Faults:

|Name|Condition|Description|
|:-:|:--|:--|
|DataValidationFault|Name is null or isn't validated by the data base ||

## ICategoryController Delete
Deletes category by primary key.

Declaration:
```
[OperationContract]
[FaultContract(typeof(ForeignKeyConstraintFault))]
void Delete(int id);
```
Arguments:

|Name|Type|Nallable|Description|
|:--|:--|:--|:-:|:--|
|id|```int```|False|Primary key of category|

* There no exception if category with the target primary key doesn't exist.

Faults:

|Name|Condition|Description|
|:-:|:--|:--|
|ForeignKeyConstraintFault| The category compounds some todoes | Foreign key constraint with the Todo table|

## ICategoryController ChangeText
Changes name of item by primary key.

Declaration:
```
[OperationContract]
[FaultContract(typeof(DataValidationFault))]
[FaultContract(typeof(ItemNotFoundFault))]
void ChangeText(int id, string name);
```
Arguments:

|Name|Type|Nallable|Description|
|:--|:--|:-:|:--|:--|
|id|```int```|False|Primary key of category|
|name|```string```|False|Category's name|

Faults:

|Name|Condition|Description|
|:-:|:--|:--|
|DataValidationFault|Name is null or isn't validated by the data base||
|ItemNotFoundFault|Category with the target primary key doesn't exist||

## ICategoryController ChangeOrder
Changes priority in the list.

Declaration:
```
[OperationContract]
[FaultContract(typeof(ItemNotFoundFault))]
void ChangeOrder(int id, int order);
```
Arguments:

|Name|Type|Nallable|Description|
|:--|:--|:-:|:--|:--|
|id|```int```|False|Primary key of category|
|order|```int```|False|Target priority in list|

Faults:

|Name|Condition|Description|
|:-:|:--|:--|
|ItemNotFoundFault|Category with the target primary key doesn't exist||

## ICategoryController ChangeColor
Changes color of item by primary key.

Declaration:
```
[OperationContract]
[FaultContract(typeof(ItemNotFoundFault))]
void ChangeColor(int id, Color? color);
```
Arguments:

|Name|Type|Nallable|Description|
|:--|:--|:-:|:--|:--|
|id|```int```|False|Primary key of category|
|color|```Color?```|True|Target color|

Faults:

|Name|Condition|Description|
|:-:|:--|:--|
|ItemNotFoundFault|Category with the target primary key doesn't exist||

## ITodoController SelectAll
Selects a full list of todoes.

Declaration:
```
[OperationContract]
IEnumerable<Todo> SelectAll();
```

Output type: ```IEnumerable<Todo>```
* Returns empty list if there no todoes.


## ITodoController SelectById
Fetches single todo by primary key.

Declaration:
```
[OperationContract]
Todo SelectById(int id);
```
Arguments:

|Name|Type|Nallable|Description|
|:--|:--|:-:|:-:|:--|
|id|```int```|False|Primary key of todo|

Output type: ```Todo```
* Returns ```null``` if todo with the target primary key doesn't exist

## ITodoController SelectByTitle
Selects a list of the todoes with the target title.

Declaration:
```
[OperationContract]
IEnumerable<Todo> SelectByTitle(string title);
```
Arguments:

|Name|Type|Nallable|Description|
|:--|:--|:-:|:-:|:--|
|title|```string```|True|Target title|

Output type: ```IEnumerable<Todo>```
* Return empty list if there no todoes.

## ITodoController SelectByCategoty
Selects a list of the todoes with the target category.

Declaration:
```
[OperationContract]
IEnumerable<Todo> SelectByCategory(int categoryId);
```
Arguments:

|Name|Type|Nallable|Description|
|:--|:--|:-:|:-:|:--|
|categoryId|```int```|False|Primary key of target category|

Output type: ```IEnumerable<Todo>```
* Return empty list if there no todoes.

## ITodoController Create
Creates a new Todo and appends in the controller.

Declaration:
```
[OperationContract]
[FaultContract(typeof(DataValidationFault))]
Todo Create(string title);
```
Arguments:

|Name|Type|Nallable|Description|
|:--|:--|:-:|:-:|:--|
|title|```string```|False|Target title|

Output type: ```Todo```
* Return a category instance with the primary key and last order value.

Faults:

|Name|Condition|Description|
|:-:|:--|:--|
|DataValidationFault|Name is null or isn't validated by the data base ||

## ITodoController Delete
Deletes todo by the primary key.

Declaration:
```
[OperationContract]
void Delete(int id);
```
Arguments:

|Name|Type|Nallable|Description|
|:--|:--|:--|:-:|:--|
|id|```int```|False|Primary key of the todo|

* There no exception if todo with the target primary key doesn't exist.

## ITodoController ChangeText
Sets new title and description by primary key.

Declaration:
```
[OperationContract]
[FaultContract(typeof(ItemNotFoundFault))]
[FaultContract(typeof(DataValidationFault))]
void ChangeText(int id, string title, string desc);
```
Arguments:

|Name|Type|Nallable|Description|
|:--|:--|:-:|:--|:--|
|id|```int```|False|Primary key of category|
|title|```string```|False|New title|
|desc|```string```|True|New description|

Faults:

|Name|Condition|Description|
|:-:|:--|:--|
|DataValidationFault|title is null or isn't validated by the data base||
|ItemNotFoundFault|Todo with the target primary key doesn't exist||

## ITodoController ChangeOrder
Changes priority in the list.

Declaration:
```
[OperationContract]
[FaultContract(typeof(ItemNotFoundFault))]
void ChangeOrder(int id, int order);
```
Arguments:

|Name|Type|Nallable|Description|
|:--|:--|:-:|:--|:--|
|id|```int```|False|Primary key of the todo|
|order|```int```|False|Target priority in list|

Faults:

|Name|Condition|Description|
|:-:|:--|:--|
|ItemNotFoundFault|Todo with the target primary key doesn't exist||

## ITodoController SetDeadline
Sets deadline time.

Declaration:
```
[OperationContract]
[FaultContract(typeof(ItemNotFoundFault))]
void SetDeadline(int id, DateTime? deadline);
```
Arguments:

|Name|Type|Nallable|Description|
|:--|:--|:-:|:--|:--|
|id|```int```|False|Primary key of the todo|
|deadline|```DateTime?```|False|New deadline time|

Faults:

|Name|Condition|Description|
|:-:|:--|:--|
|ItemNotFoundFault|Todo with the target primary key doesn't exist||

## ITodoController Check
Makes todo as checked.

Declaration:
```
[OperationContract]
[FaultContract(typeof(ItemNotFoundFault))]
void Check(int id, bool isChecked);
```
Arguments:

|Name|Type|Nallable|Description|
|:--|:--|:-:|:--|:--|
|id|```int```|False|Primary key of the todo|
|isChecked|```bool```|False|True if the todo is checked|

Faults:

|Name|Condition|Description|
|:-:|:--|:--|
|ItemNotFoundFault|Todo with the target primary key doesn't exist||

## ITodoController SetCategory
Changes category.

Declaration:
```
[OperationContract]
[FaultContract(typeof(ItemNotFoundFault))]
[FaultContract(typeof(ForeignKeyConstraintFault))]
void SetCategory(int id, int? categoryId);
```
Arguments:

|Name|Type|Nallable|Description|
|:--|:--|:-:|:--|:--|
|id|```int```|False|Primary key of the todo|
|categoryId|```int?```|True|Primary key of a new category|

Faults:

|Name|Condition|Description|
|:-:|:--|:--|
|ItemNotFoundFault|Todo with the target primary key doesn't exist||
|ForeignKeyConstraintFault| The category with the target primary key doesn't exist | Foreign key constraint with the Category table|
