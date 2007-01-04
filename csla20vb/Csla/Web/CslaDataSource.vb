Imports System.Web.UI
Imports System.Web.UI.Design
Imports System.ComponentModel
Imports System.Reflection

Namespace Web

  ''' <summary>
  ''' A Web Forms data binding control designed to support
  ''' CSLA .NET business objects as data sources.
  ''' </summary>
  <Designer(GetType(Csla.Web.Design.CslaDataSourceDesigner))> _
  <DisplayName("Csla.DataSource")> _
  <Description("CSLA .NET Data Source Control")> _
  <ToolboxData("<{0}:CslaDataSource runat=""server""></{0}:CslaDataSource>")> _
  Public Class CslaDataSource
    Inherits DataSourceControl

    Private WithEvents mDefaultView As CslaDataSourceView

    ''' <summary>
    ''' Event raised when an object is to be created and
    ''' populated with data.
    ''' </summary>
    ''' <remarks>Handle this event in a page and set
    ''' e.BusinessObject to the populated business object.
    ''' </remarks>
    Public Event SelectObject As EventHandler(Of SelectObjectArgs)

    ''' <summary>
    ''' Event raised when an object is to be populated with data
    ''' and inserted.
    ''' </summary>
    ''' <remarks>Handle this event in a page to create an
    ''' instance of the object, load the object with data and
    ''' insert the object into the database.</remarks>
    Public Event InsertObject As EventHandler(Of InsertObjectArgs)

    ''' <summary>
    ''' Event raised when an object is to be updated.
    ''' </summary>
    ''' <remarks>Handle this event in a page to update an
    ''' existing instance of an object with new data and then
    ''' save the object into the database.</remarks>
    Public Event UpdateObject As EventHandler(Of UpdateObjectArgs)

    ''' <summary>
    ''' Event raised when an object is to be deleted.
    ''' </summary>
    ''' <remarks>Handle this event in a page to delete
    ''' an object from the database.</remarks>
    Public Event DeleteObject As EventHandler(Of DeleteObjectArgs)

    ''' <summary>
    ''' Returns the default view for this data control.
    ''' </summary>
    ''' <param name="viewName">Ignored.</param>
    ''' <returns></returns>
    ''' <remarks>This control only contains a "Default" view.</remarks>
    Protected Overrides Function GetView(ByVal viewName As String) As System.Web.UI.DataSourceView

      If mDefaultView Is Nothing Then
        mDefaultView = New CslaDataSourceView(Me, "Default")
      End If
      Return mDefaultView

    End Function

    ''' <summary>
    ''' Get or set the name of the assembly containing the 
    ''' business object class to be used as a data source.
    ''' </summary>
    ''' <value>Assembly name containing the business class.</value>
    Public Property TypeAssemblyName() As String
      Get
        Return CType(Me.GetView("Default"), CslaDataSourceView).TypeAssemblyName
      End Get
      Set(ByVal value As String)
        CType(Me.GetView("Default"), CslaDataSourceView).TypeAssemblyName = value
      End Set
    End Property

    ''' <summary>
    ''' Get or set the full type name of the business object
    ''' class to be used as a data source.
    ''' </summary>
    ''' <value>Full type name of the business class.</value>
    Public Property TypeName() As String
      Get
        Return CType(Me.GetView("Default"), CslaDataSourceView).TypeName
      End Get
      Set(ByVal value As String)
        CType(Me.GetView("Default"), CslaDataSourceView).TypeName = value
      End Set
    End Property

    ''' <summary>
    ''' Get or set a value indicating whether the
    ''' business object data source supports paging.
    ''' </summary>
    ''' <remarks>
    ''' To support paging, the business object
    ''' (collection) must implement 
    ''' <see cref="Csla.Core.IReportTotalRowCount"/>.
    ''' </remarks>
    Public Property TypeSupportsPaging() As Boolean
      Get
        Return CType(Me.GetView("Default"), CslaDataSourceView).TypeSupportsPaging
      End Get
      Set(ByVal value As Boolean)
        CType(Me.GetView("Default"), CslaDataSourceView).TypeSupportsPaging = value
      End Set
    End Property

    ''' <summary>
    ''' Get or set a value indicating whether the
    ''' business object data source supports sorting.
    ''' </summary>
    Public Property TypeSupportsSorting() As Boolean
      Get
        Return (CType(Me.GetView("Default"), CslaDataSourceView)).TypeSupportsSorting
      End Get
      Set(ByVal value As Boolean)
        CType(Me.GetView("Default"), CslaDataSourceView).TypeSupportsSorting = value
      End Set
    End Property

    ''' <summary>
    ''' Returns a <see cref="Type">Type</see> object based on the
    ''' assembly and type information provided.
    ''' </summary>
    ''' <param name="assemblyName">(Optional) Assembly name containing the type.</param>
    ''' <param name="typeName">Full type name of the class.</param>
    ''' <remarks></remarks>
    Friend Overloads Shared Function [GetType]( _
      ByVal assemblyName As String, ByVal typeName As String) As Type

      If Len(assemblyName) > 0 Then
        Dim asm As Assembly = Assembly.Load(assemblyName)
        Return asm.GetType(typeName, True, True)

      Else
        Return Type.GetType(typeName, True, True)
      End If

    End Function

    ''' <summary>
    ''' Returns a list of views available for this control.
    ''' </summary>
    ''' <remarks>This control only provides the "Default" view.</remarks>
    Protected Overrides Function GetViewNames() As System.Collections.ICollection

      Return New String() {"Default"}

    End Function

    ''' <summary>
    ''' Raises the SelectObject event.
    ''' </summary>
    Friend Sub OnSelectObject(ByVal e As SelectObjectArgs)

      RaiseEvent SelectObject(Me, e)

    End Sub

    ''' <summary>
    ''' Raises the InsertObject event.
    ''' </summary>
    Friend Sub OnInsertObject(ByVal e As InsertObjectArgs)

      RaiseEvent InsertObject(Me, e)

    End Sub

    ''' <summary>
    ''' Raises the UpdateObject event.
    ''' </summary>
    Friend Sub OnUpdateObject(ByVal e As UpdateObjectArgs)

      RaiseEvent UpdateObject(Me, e)

    End Sub

    ''' <summary>
    ''' Raises the DeleteObject event.
    ''' </summary>
    Friend Sub OnDeleteObject(ByVal e As DeleteObjectArgs)

      RaiseEvent DeleteObject(Me, e)

    End Sub

  End Class

  ''' <summary>
  ''' Argument object used in the SelectObject event.
  ''' </summary>
  <Serializable()> _
  Public Class SelectObjectArgs
    Inherits EventArgs

    Private mBusinessObject As Object
    Private mSortExpression As String
    Private mSortProperty As String
    Private mSortDirection As ListSortDirection
    Private mStartRowIndex As Integer
    Private mMaximumRows As Integer
    Private mRetrieveTotalRowCount As Boolean

    ''' <summary>
    ''' Get or set a reference to the business object
    ''' that is created and populated by the SelectObject
    ''' event handler in the web page.
    ''' </summary>
    ''' <value>A reference to a CSLA .NET business object.</value>
    Public Property BusinessObject() As Object
      Get
        Return mBusinessObject
      End Get
      Set(ByVal value As Object)
        mBusinessObject = value
      End Set
    End Property

    ''' <summary>
    ''' Gets the sort expression that should be used to
    ''' sort the data being returned to the data source
    ''' control.
    ''' </summary>
    Public ReadOnly Property SortExpression() As String
      Get
        Return mSortExpression
      End Get
    End Property

    ''' <summary>
    ''' Gets the property name for the sort if only one
    ''' property/column name is specified.
    ''' </summary>
    ''' <remarks>
    ''' If multiple properties/columns are specified
    ''' for the sort, you must parse the value from
    ''' <see cref="SortExpression"/> to find all the
    ''' property names and sort directions for the sort.
    ''' </remarks>
    Public ReadOnly Property SortProperty() As String
      Get
        Return mSortProperty
      End Get
    End Property

    ''' <summary>
    ''' Gets the sort direction for the sort if only
    ''' one property/column name is specified.
    ''' </summary>
    ''' <remarks>
    ''' If multiple properties/columns are specified
    ''' for the sort, you must parse the value from
    ''' <see cref="SortExpression"/> to find all the
    ''' property names and sort directions for the sort.
    ''' </remarks>
    Public ReadOnly Property SortDirection() As ListSortDirection
      Get
        Return mSortDirection
      End Get
    End Property

    ''' <summary>
    ''' Gets the index for the first row that will be
    ''' displayed. This should be the first row in
    ''' the resulting collection set into the
    ''' <see cref="BusinessObject"/> property.
    ''' </summary>
    Public ReadOnly Property StartRowIndex() As Integer
      Get
        Return mStartRowIndex
      End Get
    End Property

    ''' <summary>
    ''' Gets the maximum number of rows that
    ''' should be returned as a result of this
    ''' query. For paged collections, this is the
    ''' page size.
    ''' </summary>
    Public ReadOnly Property MaximumRows() As Integer
      Get
        Return mMaximumRows
      End Get
    End Property

    ''' <summary>
    ''' Gets a value indicating whether the
    ''' query should return the total row count
    ''' through the
    ''' <see cref="Csla.Core.IReportTotalRowCount"/>
    ''' interface.
    ''' </summary>
    Public ReadOnly Property RetrieveTotalRowCount() As Boolean
      Get
        Return mRetrieveTotalRowCount
      End Get
    End Property

    ''' <summary>
    ''' Creates an instance of the object, initializing
    ''' it with values from data binding.
    ''' </summary>
    ''' <param name="args">Values provided from data binding.</param>
    Public Sub New(ByVal args As System.Web.UI.DataSourceSelectArguments)

      mStartRowIndex = args.StartRowIndex
      mMaximumRows = args.MaximumRows
      mRetrieveTotalRowCount = args.RetrieveTotalRowCount

      mSortExpression = args.SortExpression
      If Not String.IsNullOrEmpty(mSortExpression) Then
        If Right(mSortExpression, 5) = " DESC" Then
          mSortProperty = Left(mSortExpression, mSortExpression.Length - 5)
          mSortDirection = ListSortDirection.Descending

        Else
          mSortProperty = args.SortExpression
          mSortDirection = ListSortDirection.Ascending
        End If
      End If

    End Sub

  End Class

  ''' <summary>
  ''' Argument object used in the InsertObject event.
  ''' </summary>
  Public Class InsertObjectArgs
    Inherits EventArgs

    Private mValues As System.Collections.IDictionary
    Private mRowsAffected As Integer

    ''' <summary>
    ''' Gets or sets the number of rows affected
    ''' while handling this event.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' The code handling the event should set this
    ''' value to indicate the number of rows affected
    ''' by the operation.
    ''' </remarks>
    Public Property RowsAffected() As Integer
      Get
        Return mRowsAffected
      End Get
      Set(ByVal value As Integer)
        mRowsAffected = value
      End Set
    End Property

    ''' <summary>
    ''' The list of data values entered by the user.
    ''' </summary>
    ''' <remarks>It is up to the event handler in the
    ''' web page to take the list of values, put them
    ''' into a business object and to save that object
    ''' into the database.</remarks>
    Public ReadOnly Property Values() As System.Collections.IDictionary
      Get
        Return mValues
      End Get
    End Property

    ''' <summary>
    ''' Create an instance of the object.
    ''' </summary>
    Public Sub New(ByVal values As System.Collections.IDictionary)

      mValues = values

    End Sub

  End Class

  ''' <summary>
  ''' Argument object used in the UpdateObject event.
  ''' </summary>
  Public Class UpdateObjectArgs
    Inherits EventArgs

    Private mKeys As System.Collections.IDictionary
    Private mValues As System.Collections.IDictionary
    Private mOldValues As System.Collections.IDictionary
    Private mRowsAffected As Integer

    ''' <summary>
    ''' Gets or sets the number of rows affected
    ''' while handling this event.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' The code handling the event should set this
    ''' value to indicate the number of rows affected
    ''' by the operation.
    ''' </remarks>
    Public Property RowsAffected() As Integer
      Get
        Return mRowsAffected
      End Get
      Set(ByVal value As Integer)
        mRowsAffected = value
      End Set
    End Property

    ''' <summary>
    ''' The list of key values entered by the user.
    ''' </summary>
    ''' <remarks>It is up to the event handler in the
    ''' web page to take the list of values, put them
    ''' into a business object and to save that object
    ''' into the database.</remarks>
    Public ReadOnly Property Keys() As System.Collections.IDictionary
      Get
        Return mKeys
      End Get
    End Property

    ''' <summary>
    ''' The list of data values entered by the user.
    ''' </summary>
    ''' <remarks>It is up to the event handler in the
    ''' web page to take the list of values, put them
    ''' into a business object and to save that object
    ''' into the database.</remarks>
    Public ReadOnly Property Values() As System.Collections.IDictionary
      Get
        Return mValues
      End Get
    End Property

    ''' <summary>
    ''' The list of old data values maintained by
    ''' data binding.
    ''' </summary>
    ''' <remarks>It is up to the event handler in the
    ''' web page to take the list of values, put them
    ''' into a business object and to save that object
    ''' into the database.</remarks>
    Public ReadOnly Property OldValues() As System.Collections.IDictionary
      Get
        Return mOldValues
      End Get
    End Property

    ''' <summary>
    ''' Creates an instance of the object.
    ''' </summary>
    Public Sub New(ByVal keys As System.Collections.IDictionary, ByVal values As System.Collections.IDictionary, ByVal oldValues As System.Collections.IDictionary)

      mKeys = keys
      mValues = values
      mOldValues = oldValues

    End Sub

  End Class

  ''' <summary>
  ''' Argument object used in the DeleteObject event.
  ''' </summary>
  Public Class DeleteObjectArgs
    Inherits EventArgs

    Private mKeys As System.Collections.IDictionary
    Private mOldValues As System.Collections.IDictionary
    Private mRowsAffected As Integer

    ''' <summary>
    ''' Gets or sets the number of rows affected
    ''' while handling this event.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' The code handling the event should set this
    ''' value to indicate the number of rows affected
    ''' by the operation.
    ''' </remarks>
    Public Property RowsAffected() As Integer
      Get
        Return mRowsAffected
      End Get
      Set(ByVal value As Integer)
        mRowsAffected = value
      End Set
    End Property

    ''' <summary>
    ''' The list of key values entered by the user.
    ''' </summary>
    ''' <remarks>It is up to the event handler in the
    ''' web page to use the values to identify the 
    ''' object to be deleted.</remarks>
    Public ReadOnly Property Keys() As System.Collections.IDictionary
      Get
        Return mKeys
      End Get
    End Property

    ''' <summary>
    ''' The list of old data values maintained by
    ''' data binding.
    ''' </summary>
    ''' <remarks>It is up to the event handler in the
    ''' web page to use the values to identify the 
    ''' object to be deleted.</remarks>
    Public ReadOnly Property OldValues() As System.Collections.IDictionary
      Get
        Return mOldValues
      End Get
    End Property

    ''' <summary>
    ''' Create an instance of the object.
    ''' </summary>
    Public Sub New(ByVal keys As System.Collections.IDictionary, ByVal oldValues As System.Collections.IDictionary)

      mKeys = keys
      mOldValues = oldValues

    End Sub

  End Class

End Namespace
