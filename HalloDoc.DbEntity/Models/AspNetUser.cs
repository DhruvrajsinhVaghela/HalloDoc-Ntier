using System;
using System.Collections.Generic;
using System.Net;

namespace HalloDoc.DbEntity.Models;

public partial class AspNetUser
{
    public int Id { get; set; }

    public string? UserName { get; set; }

    public string? PasswordHash { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public IPAddress? Ip { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual ICollection<Admin> AdminAspNetUsers { get; set; } = new List<Admin>();

    public virtual ICollection<Admin> AdminCreatedByNavigations { get; set; } = new List<Admin>();

    public virtual ICollection<Admin> AdminModifiedByNavigations { get; set; } = new List<Admin>();

    public virtual ICollection<Business> BusinessCreatedByNavigations { get; set; } = new List<Business>();

    public virtual ICollection<Business> BusinessModifiedByNavigations { get; set; } = new List<Business>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Physician> PhysicianAspNetUsers { get; set; } = new List<Physician>();

    public virtual ICollection<Physician> PhysicianCreatedByNavigations { get; set; } = new List<Physician>();

    public virtual ICollection<Physician> PhysicianModifiedByNavigations { get; set; } = new List<Physician>();

    public virtual ICollection<RequestNote> RequestNoteCreatedByNavigations { get; set; } = new List<RequestNote>();

    public virtual ICollection<RequestNote> RequestNoteModifiedByNavigations { get; set; } = new List<RequestNote>();

    public virtual ICollection<Role> RoleCreatedByNavigations { get; set; } = new List<Role>();

    public virtual ICollection<Role> RoleModifiedByNavigations { get; set; } = new List<Role>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<AspNetRole> Roles { get; set; } = new List<AspNetRole>();
}
