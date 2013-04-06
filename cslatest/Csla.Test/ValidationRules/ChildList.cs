﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla.Serialization;
using Csla.Serialization.Mobile;
using Csla.Core;

namespace Csla.Test.ValidationRules
{
  [Serializable]
  public class ChildList : BusinessListBase<ChildList, Child>
  {
    public static ChildList NewList()
    {
      return Csla.DataPortal.CreateChild<ChildList>();
    }
  }
}
