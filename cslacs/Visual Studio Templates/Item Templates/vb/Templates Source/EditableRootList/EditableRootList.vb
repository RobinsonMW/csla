Imports System
Imports System.Collections.Generic
Imports Csla
Imports Csla.Security

Namespace $rootnamespace$

  <Serializable()> _
  Public Class $safeitemname$
  Inherits BusinessListBase(Of $safeitemname$, $childitem$)

#Region "Authorization Rules"

    Private Shared Sub AddObjectAuthorizationRules()
      ' TODO: add authorization rules 
      AuthorizationRules.AllowGet(GetType($safeitemname$), "Role")
    End Sub

#End Region

#Region "Factory Methods"

    Public Shared Function New$safeitemname$() As $safeitemname$
      Return DataPortal.Create(Of $safeitemname$)()
    End Function

    Public Shared Function Get$safeitemname$(ByVal id As Integer) As $safeitemname$
      Return DataPortal.Fetch(Of $safeitemname$)(New SingleCriteria(Of $safeitemname$, Integer)(id))
    End Function

    ' Require use of factory methods 
    Private Sub New()
    End Sub

#End Region

#Region "Data Access"

    Private Overloads Sub DataPortal_Fetch(ByVal criteria As SingleCriteria(Of $safeitemname$, Integer))
      RaiseListChangedEvents = False
      ' TODO: load values into memory 
      Dim childData As Object = Nothing
      For Each item As Object In DirectCast(childData, List(Of Object))
      Me.Add($childitem$.Get$childitem$(childData))
      Next
      RaiseListChangedEvents = True
    End Sub

#End Region
  End Class



  <Serializable()> _
  Public Class $childitem$
    Inherits BusinessBase(Of $childitem$)

#Region " Business Methods "

    ' TODO: add your own fields, properties and methods
    'example with private backing field
    Private Shared IdProperty As PropertyInfo(Of Integer) = RegisterProperty(New PropertyInfo(Of Integer)("Id", "Id"))
    Private _id As Integer = IdProperty.DefaultValue
    ''' <Summary>
    ''' Gets and sets the Id value.
    ''' </Summary>
    Public Property Id() As Integer
      Get
        Return GetProperty(IdProperty, _id)
      End Get
      Set(ByVal value As Integer)
        SetProperty(IdProperty, _id, value)
      End Set
    End Property

    'example with managed backing field
    Private Shared NameProperty As PropertyInfo(Of String) = RegisterProperty(New PropertyInfo(Of String)("Name", "Name"))
    ''' <Summary>
    ''' Gets and sets the Name value.
    ''' </Summary>
    Public Property Name() As String
      Get
        Return GetProperty(NameProperty)
      End Get
      Set(ByVal value As String)
        SetProperty(NameProperty, value)
      End Set
    End Property


#End Region

#Region " Validation Rules "

    Protected Overrides Sub AddBusinessRules()
      ' TODO: add validation rules
      'ValidationRules.AddRule(Nothing, "")
    End Sub

#End Region

#Region " Authorization Rules "

    Protected Overrides Sub AddAuthorizationRules()

      ' TODO: add authorization rules
      AuthorizationRules.AllowWrite(NameProperty, "")

    End Sub

    Private Shared Sub AddObjectAuthorizationRules()
      'TODO: add authorization rules
      AuthorizationRules.AllowEdit(GetType(EditableChild), "Role")
    End Sub

#End Region

#Region " Factory Methods "

    Friend Shared Function New$childitem$() As $childitem$
      Return DataPortal.CreateChild(Of $safeitemname$)()
    End Function

    Friend Shared Function Get$childitem$(ByVal childData As Object) As $childitem$
      Return DataPortal.FetchChild(Of $safeitemname$)(childData)
    End Function

    Private Sub New()
      'Require use of factory methods
    End Sub

#End Region

#Region " Data Access "

    Protected Overrides Sub Child_Create()
      'TODO: load default values
      'omit this override if you have no defaults to set
      MyBase.Child_Create()
    End Sub

    Private Sub Child_Fetch(ByVal childData As Object)
      ' TODO: load values
    End Sub

    Private Sub Child_Insert(ByVal parent As Object)
      ' TODO: insert values
    End Sub

    Private Sub Child_Update(ByVal parent As Object)
      ' TODO: insert values
    End Sub

    Friend Sub Child_DeleteSelf(ByVal parent As Object)
      ' TODO: delete values
    End Sub

#End Region

  End Class

End Namespace