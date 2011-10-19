using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Csla;
using Csla.Security;
using Csla.Data;
using Csla.Serialization;
using System.ComponentModel;

namespace ProjectTracker.Library
{
  [Serializable()]
  public class ResourceEdit : BusinessBase<ResourceEdit>
  {
    public static readonly PropertyInfo<byte[]> TimeStampProperty = RegisterProperty<byte[]>(c => c.TimeStamp);
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public byte[] TimeStamp
    {
      get { return GetProperty(TimeStampProperty); }
      set { SetProperty(TimeStampProperty, value); }
    }

    public static readonly PropertyInfo<int> IdProperty = RegisterProperty<int>(c => c.Id);
    public int Id
    {
      get { return GetProperty(IdProperty); }
      set { SetProperty(IdProperty, value); }
    }

    public static readonly PropertyInfo<string> LastNameProperty = 
      RegisterProperty<string>(c => c.LastName);
    [Display(Name = "Last name")]
    [Required]
    [StringLength(50)]
    public string LastName
    {
      get { return GetProperty(LastNameProperty); }
      set { SetProperty(LastNameProperty, value); }
    }

    public static readonly PropertyInfo<string> FirstNameProperty = 
      RegisterProperty<string>(c => c.FirstName);
    [Display(Name = "First name")]
    [Required]
    [StringLength(50)]
    public string FirstName
    {
      get { return GetProperty(FirstNameProperty); }
      set { SetProperty(FirstNameProperty, value); }
    }

    [Display(Name = "Full name")]
    public string FullName
    {
      get { return LastName + ", " + FirstName; }
    }

    public static readonly PropertyInfo<ResourceAssignments> AssignmentsProperty =
      RegisterProperty<ResourceAssignments>(c => c.Assignments);
    public ResourceAssignments Assignments
    {
      get
      {
        if (!(FieldManager.FieldExists(AssignmentsProperty)))
          LoadProperty(AssignmentsProperty, DataPortal.CreateChild<ResourceAssignments>());
        return GetProperty(AssignmentsProperty);
      }
      private set { LoadProperty(AssignmentsProperty, value); }
    }

    public override string ToString()
    {
      return Id.ToString();
    }

    protected override void AddBusinessRules()
    {
      base.AddBusinessRules();
      BusinessRules.AddRule(new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.WriteProperty, LastNameProperty, "ProjectManager"));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.WriteProperty, FirstNameProperty, "ProjectManager"));
      BusinessRules.AddRule(new NoDuplicateProject { PrimaryProperty = AssignmentsProperty });
    }

    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static void AddObjectAuthorizationRules()
    {
      Csla.Rules.BusinessRules.AddRule(typeof(ResourceEdit), new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.CreateObject, "ProjectManager"));
      Csla.Rules.BusinessRules.AddRule(typeof(ResourceEdit), new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.EditObject, "ProjectManager"));
      Csla.Rules.BusinessRules.AddRule(typeof(ResourceEdit), new Csla.Rules.CommonRules.IsInRole(Csla.Rules.AuthorizationActions.DeleteObject, "ProjectManager", "Administrator"));
    }

    protected override void OnChildChanged(Csla.Core.ChildChangedEventArgs e)
    {
      if (e.ChildObject is ResourceAssignments)
      {
        BusinessRules.CheckRules(AssignmentsProperty);
        OnPropertyChanged(AssignmentsProperty);
      }
      base.OnChildChanged(e);
    }

    private class NoDuplicateProject : Csla.Rules.BusinessRule
    {
      protected override void Execute(Csla.Rules.RuleContext context)
      {
        var target = (ResourceEdit)context.Target;
        foreach (var item in target.Assignments)
        {
          var count = target.Assignments.Count(r => r.ProjectId == item.ProjectId);
          if (count > 1)
          {
            context.AddErrorResult("Duplicate projects not allowed");
            return;
          }
        }
      }
    }

    public static void GetResource(int id, EventHandler<DataPortalResult<ResourceEdit>> callback)
    {
      DataPortal.BeginFetch<ResourceEdit>(id, callback);
    }

    public static void Exists(int id, Action<bool> result)
    {
      var cmd = new ResourceExistsCommand(id);
      DataPortal.BeginExecute<ResourceExistsCommand>(cmd, (o, e) =>
      {
        if (e.Error != null)
          throw e.Error;
        else
          result(e.Object.ResourceExists);
      });
    }

    public static void DeleteResource(int id, EventHandler<DataPortalResult<ResourceEdit>> callback)
    {
      DataPortal.BeginDelete<ResourceEdit>(id, callback);
    }

#if SILVERLIGHT
    public static void NewResource(EventHandler<DataPortalResult<ResourceEdit>> callback)
    {
      DataPortal.BeginCreate<ResourceEdit>(callback, DataPortal.ProxyModes.LocalOnly);
    }
#else
    public static void NewResource(EventHandler<DataPortalResult<ResourceEdit>> callback)
    {
      DataPortal.BeginCreate<ResourceEdit>(callback);
    }

    public static ResourceEdit NewResource()
    {
      return DataPortal.Create<ResourceEdit>();
    }

    public static ResourceEdit GetResource(int id)
    {
      return DataPortal.Fetch<ResourceEdit>(id);
    }

    public static void DeleteResource(int id)
    {
      DataPortal.Delete<ResourceEdit>(id);
    }

    public static bool Exists(int id)
    {
      var cmd = new ResourceExistsCommand(id);
      cmd = DataPortal.Execute<ResourceExistsCommand>(cmd);
      return cmd.ResourceExists;
    }

    [RunLocal]
    protected override void DataPortal_Create()
    {
      base.DataPortal_Create();
    }

    private void DataPortal_Fetch(int id)
    {
      using (var ctx = ProjectTracker.Dal.DalFactory.GetManager())
      {
        var dal = ctx.GetProvider<ProjectTracker.Dal.IResourceDal>();
        var data = dal.Fetch(id);
        using (BypassPropertyChecks)
        {
          Id = data.Id;
          FirstName = data.FirstName;
          LastName = data.LastName;
          TimeStamp = data.LastChanged;
          Assignments = DataPortal.FetchChild<ResourceAssignments>(id);
        }
      }
    }

    protected override void DataPortal_Insert()
    {
      using (var ctx = ProjectTracker.Dal.DalFactory.GetManager())
      {
        var dal = ctx.GetProvider<ProjectTracker.Dal.IResourceDal>();
        using (BypassPropertyChecks)
        {
          var item = new ProjectTracker.Dal.ResourceDto
          {
            FirstName = this.FirstName,
            LastName = this.LastName
          };
          dal.Insert(item);
          Id = item.Id;
          TimeStamp = item.LastChanged;
        }
        FieldManager.UpdateChildren(this);
      }
    }

    protected override void DataPortal_Update()
    {
      using (var ctx = ProjectTracker.Dal.DalFactory.GetManager())
      {
        var dal = ctx.GetProvider<ProjectTracker.Dal.IResourceDal>();
        using (BypassPropertyChecks)
        {
          var item = new ProjectTracker.Dal.ResourceDto
          {
            Id = this.Id,
            FirstName = this.FirstName,
            LastName = this.LastName,
            LastChanged = this.TimeStamp
          };
          dal.Update(item);
          TimeStamp = item.LastChanged;
        }
        FieldManager.UpdateChildren(this);
      }
    }

    protected override void DataPortal_DeleteSelf()
    {
      using (BypassPropertyChecks)
        DataPortal_Delete(this.Id);
    }

    private void DataPortal_Delete(int id)
    {
      using (var ctx = ProjectTracker.Dal.DalFactory.GetManager())
      {
        Assignments.Clear();
        FieldManager.UpdateChildren(this);
        var dal = ctx.GetProvider<ProjectTracker.Dal.IResourceDal>();
        dal.Delete(id);
      }
    }
#endif
  }
}