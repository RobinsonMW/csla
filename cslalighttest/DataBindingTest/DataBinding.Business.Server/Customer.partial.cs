﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla;

namespace DataBinding.Business
{
  public partial class Customer
  {
    private Customer() { }

    #region Server Factories

    public static Customer Load(int Id, string name, DateTime birthDate, bool isPreferred)
    {
      return DataPortal.FetchChild<Customer>(Id, name, birthDate, isPreferred);
    }

    #endregion

    #region Data Access

    private void Child_Fetch(int id, string name, DateTime birthDate, bool isPreferred)
    {
      LoadProperty<int>(IdProperty, id);
      LoadProperty<string>(NameProperty, name);
      LoadProperty<DateTime>(BirthDateProperty, birthDate);
      LoadProperty<bool>(IsPreferredProperty, isPreferred);
      ValidationRules.CheckRules();
    }
    
    private void Child_Update()
    {
      // simulating update...
    }

    #endregion
  }
}
