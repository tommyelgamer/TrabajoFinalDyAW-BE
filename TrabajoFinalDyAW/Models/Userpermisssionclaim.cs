﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace TrabajoFinalDyAW.Models;

public partial class Userpermisssionclaim
{
    public Guid UserpermissionclaimId { get; set; }

    public string PermissionclaimName { get; set; }

    public Guid UserId { get; set; }

    public virtual Permissionclaim PermissionclaimNameNavigation { get; set; }

    public virtual User User { get; set; }
}