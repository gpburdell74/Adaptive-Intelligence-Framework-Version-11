# <span style="margin-bottom:0.25px"> Adaptive Intelligence .NET Shared Framework Version 11</span>

<hr style="border: none; height: 1px; background: linear-gradient(to right, limegreen, blue);">

This is a general purpose .NET Class Library for application development specifically for .NET 11.  
There are no guarantees provided, so use at your own discretion.

Code from the original Adaptive Intelligence Framework is re-used here with improvements and optimizations,
but instead of being split into multiple libraries, the code is now combined into a single library 
with specific namespaces for easier use and deployment.

### <span style="margin-bottom:0.25px">The **Common** Namespace</span>

<hr style="border: none; height: 1px; background: linear-gradient(to right, limegreen, blue);">

#### Former Namespace:	`Adaptive.Intelligence.Shared`
#### Current Namespace:	`Adaptive.Intelligence.Common`

<hr style="border: none; height: 1px; background: linear-gradient(to right, limegreen, blue);">

This namespace contains the core classes and methods that are used across the entire framework. 
It includes a variety of abstract base classes, utility functions, and common data structures that facilitate 
application development.  There are several sub-namespaces and folders that contain specific types of classes,
some of which (but not all) are listed below for the `Adaptive.Intelligence.Common` namespace.

<hr style="border: none; height: 1px; background: linear-gradient(to right, limegreen, blue);">

**Notable Classes and Interfaces In The `Adaptive.Intelligence.Common.Abstractions` namespace:

<hr style="border: none; height: 1px; background: linear-gradient(to right, limegreen, blue);">

Defines The Contract For:

`IExceptionTracking`	- Classes that require exception tracking during operation.
`IOperationResult`		- Classes that represent the result of an operation, including success/failure status and any associated errors or messages.
`IOperationResult<T>`	- Any `IOperationResult` implementations that also contain or return data.

Provides the Base Implementation For:

`DisposableObjectBase`	- the IDisposable pattern.
`PropertyAwareBase`		- the INotifyPropertyChanged pattern.
`ExceptionTrackingBase`	- Classes whose operations require catching and tracking of exceptions during operation.
`LoggableBase`			- Classes whose operations require logging and dependency-injection of the `ILogger` interface.
`StaticLoggableBase`	- Classes whose operations require logging, but utilize a static instance for writing to a single log file.
`BusinessBase`			- General purpose business objects.
`BusinessBase<T>`		- General purpose business objects with a specific data type.

