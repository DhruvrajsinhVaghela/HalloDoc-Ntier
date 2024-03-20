using System;
using System.Collections.Generic;
using HalloDoc.DbEntity.Models;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.DbEntity.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<AdminRegion> AdminRegions { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<Blockrequest> Blockrequests { get; set; }

    public virtual DbSet<Business> Businesses { get; set; }

    public virtual DbSet<CaseTag> CaseTags { get; set; }

    public virtual DbSet<Concierge> Concierges { get; set; }

    public virtual DbSet<HealthProfessional> HealthProfessionals { get; set; }

    public virtual DbSet<HealthProfessionalType> HealthProfessionalTypes { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Physician> Physicians { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<RequestBusiness> RequestBusinesses { get; set; }

    public virtual DbSet<RequestClient> RequestClients { get; set; }

    public virtual DbSet<RequestConcierge> RequestConcierges { get; set; }

    public virtual DbSet<RequestNote> RequestNotes { get; set; }

    public virtual DbSet<RequestStatusLog> RequestStatusLogs { get; set; }

    public virtual DbSet<RequestWiseFile> RequestWiseFiles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("User ID = postgres;Password=Dhruv@123;Server=localhost;Port=5432;Database=HalloDoc;Integrated Security=true;Pooling=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("admin_pkey");

            entity.ToTable("admin");

            entity.Property(e => e.AdminId).HasColumnName("admin_id");
            entity.Property(e => e.Address1)
                .HasMaxLength(500)
                .HasColumnName("address1");
            entity.Property(e => e.Address2)
                .HasMaxLength(500)
                .HasColumnName("address2");
            entity.Property(e => e.AltPhone)
                .HasMaxLength(20)
                .HasColumnName("alt_phone");
            entity.Property(e => e.AspNetUserId).HasColumnName("asp_net_user_id");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.Mobile)
                .HasMaxLength(20)
                .HasColumnName("mobile");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");
            entity.Property(e => e.RegionId).HasColumnName("region_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Zip)
                .HasMaxLength(10)
                .HasColumnName("zip");

            entity.HasOne(d => d.AspNetUser).WithMany(p => p.AdminAspNetUsers)
                .HasForeignKey(d => d.AspNetUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("admin_asp_net_user_id_fkey");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AdminCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("admin_created_by_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.AdminModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("admin_modified_by_fkey");
        });

        modelBuilder.Entity<AdminRegion>(entity =>
        {
            entity.HasKey(e => e.AdminRegionId).HasName("admin_region_pkey");

            entity.ToTable("admin_region");

            entity.Property(e => e.AdminRegionId).HasColumnName("admin_region_id");
            entity.Property(e => e.AdminId).HasColumnName("admin_id");
            entity.Property(e => e.RegionId).HasColumnName("region_id");

            entity.HasOne(d => d.Admin).WithMany(p => p.AdminRegions)
                .HasForeignKey(d => d.AdminId)
                .HasConstraintName("admin_region_admin_id_fkey");

            entity.HasOne(d => d.Region).WithMany(p => p.AdminRegions)
                .HasForeignKey(d => d.RegionId)
                .HasConstraintName("admin_region_region_id_fkey");
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("asp_net_roles_pkey");

            entity.ToTable("asp_net_roles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(256)
                .HasColumnName("name");
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("asp_net_users_pkey");

            entity.ToTable("asp_net_users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.Email)
                .HasMaxLength(256)
                .HasColumnName("email");
            entity.Property(e => e.Ip)
                .HasDefaultValueSql("inet_client_addr()")
                .HasColumnName("ip");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(128)
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.PhoneNumber)
                .HasColumnType("character varying")
                .HasColumnName("phone_number");
            entity.Property(e => e.UserName)
                .HasMaxLength(256)
                .HasColumnName("user_name");

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("asp_net_user_roles_role_id_fkey"),
                    l => l.HasOne<AspNetUser>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("asp_net_user_roles_user_id_fkey"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId").HasName("asp_net_user_roles_primary_key");
                        j.ToTable("asp_net_user_roles");
                        j.IndexerProperty<int>("UserId")
                            .ValueGeneratedOnAdd()
                            .HasColumnName("user_id");
                        j.IndexerProperty<int>("RoleId")
                            .ValueGeneratedOnAdd()
                            .HasColumnName("role_id");
                    });
        });

        modelBuilder.Entity<Blockrequest>(entity =>
        {
            entity.HasKey(e => e.BlockrequestId).HasName("blockrequests_pkey");

            entity.ToTable("blockrequests");

            entity.Property(e => e.BlockrequestId).HasColumnName("blockrequestId");
            entity.Property(e => e.CreatedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("IP");
            entity.Property(e => e.IsActive)
                .HasColumnType("bit(1)")
                .HasColumnName("isActive");
            entity.Property(e => e.ModifiedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(50)
                .HasColumnName("phonenumber");
            entity.Property(e => e.Reason)
                .HasColumnType("character varying")
                .HasColumnName("reason");
            entity.Property(e => e.RequestId).HasColumnName("request_id");

            entity.HasOne(d => d.Request).WithMany(p => p.Blockrequests)
                .HasForeignKey(d => d.RequestId)
                .HasConstraintName("blockrequests_request_id_fkey");
        });

        modelBuilder.Entity<Business>(entity =>
        {
            entity.HasKey(e => e.BusinessId).HasName("business_pkey");

            entity.ToTable("business");

            entity.Property(e => e.BusinessId).HasColumnName("business_id");
            entity.Property(e => e.Address1)
                .HasMaxLength(500)
                .HasColumnName("address1");
            entity.Property(e => e.Address2)
                .HasMaxLength(500)
                .HasColumnName("address2");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .HasColumnName("city");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.FaxNumber)
                .HasMaxLength(20)
                .HasColumnName("fax_number");
            entity.Property(e => e.Ip)
                .HasDefaultValueSql("inet_client_addr()")
                .HasColumnName("ip");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.IsRegistered).HasColumnName("is_registered");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phone_number");
            entity.Property(e => e.RegionId).HasColumnName("region_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.ZipCode)
                .HasMaxLength(10)
                .HasColumnName("zip_code");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BusinessCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("business_created_by_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.BusinessModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("business_modified_by_fkey");

            entity.HasOne(d => d.Region).WithMany(p => p.Businesses)
                .HasForeignKey(d => d.RegionId)
                .HasConstraintName("business_region_id_fkey");
        });

        modelBuilder.Entity<CaseTag>(entity =>
        {
            entity.HasKey(e => e.CaseTagId).HasName("CaseTag_pkey");

            entity.ToTable("CaseTag");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Concierge>(entity =>
        {
            entity.HasKey(e => e.ConciergeId).HasName("concierge_pkey");

            entity.ToTable("concierge");

            entity.Property(e => e.ConciergeId).HasColumnName("concierge_id");
            entity.Property(e => e.Address)
                .HasMaxLength(150)
                .HasColumnName("address");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .HasColumnName("city");
            entity.Property(e => e.ConciergeName)
                .HasMaxLength(100)
                .HasColumnName("concierge_name");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.RegionId).HasColumnName("region_id");
            entity.Property(e => e.RoleId)
                .HasMaxLength(20)
                .HasColumnName("role_id");
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .HasColumnName("state");
            entity.Property(e => e.Street)
                .HasMaxLength(50)
                .HasColumnName("street");
            entity.Property(e => e.ZipCode)
                .HasMaxLength(50)
                .HasColumnName("zip_code");

            entity.HasOne(d => d.Region).WithMany(p => p.Concierges)
                .HasForeignKey(d => d.RegionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("concierge_region_id_fkey");
        });

        modelBuilder.Entity<HealthProfessional>(entity =>
        {
            entity.HasKey(e => e.VendorId).HasName("health_professionals_pkey");

            entity.ToTable("health_professionals");

            entity.Property(e => e.VendorId).HasColumnName("vendor_id");
            entity.Property(e => e.Address)
                .HasMaxLength(150)
                .HasColumnName("address");
            entity.Property(e => e.BusinessContact)
                .HasMaxLength(100)
                .HasColumnName("business_contact");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.FaxNumber)
                .HasMaxLength(50)
                .HasColumnName("fax_number");
            entity.Property(e => e.Ip)
                .HasDefaultValueSql("inet_client_addr()")
                .HasColumnName("ip");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(100)
                .HasColumnName("phone_number");
            entity.Property(e => e.Profession).HasColumnName("profession");
            entity.Property(e => e.RegionId).HasColumnName("region_id");
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .HasColumnName("state");
            entity.Property(e => e.VendorName)
                .HasMaxLength(50)
                .HasColumnName("vendor_name");
            entity.Property(e => e.Zip)
                .HasMaxLength(50)
                .HasColumnName("zip");

            entity.HasOne(d => d.ProfessionNavigation).WithMany(p => p.HealthProfessionals)
                .HasForeignKey(d => d.Profession)
                .HasConstraintName("health_professionals_profession_fkey");

            entity.HasOne(d => d.Region).WithMany(p => p.HealthProfessionals)
                .HasForeignKey(d => d.RegionId)
                .HasConstraintName("health_professionals_region_id_fkey");
        });

        modelBuilder.Entity<HealthProfessionalType>(entity =>
        {
            entity.HasKey(e => e.HealthProfessionalId).HasName("health_professional_type_pkey");

            entity.ToTable("health_professional_type");

            entity.Property(e => e.HealthProfessionalId).HasColumnName("health_professional_id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.ProfessionName)
                .HasMaxLength(50)
                .HasColumnName("profession_name");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_details_pkey");

            entity.ToTable("order_details");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BusinessContact)
                .HasMaxLength(100)
                .HasColumnName("business_contact");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.FaxNumber)
                .HasMaxLength(50)
                .HasColumnName("fax_number");
            entity.Property(e => e.NoOfRefill).HasColumnName("no_of_refill");
            entity.Property(e => e.Prescription).HasColumnName("prescription");
            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.VendorId).HasColumnName("vendor_id");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("order_details_created_by_fkey");

            entity.HasOne(d => d.Request).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.RequestId)
                .HasConstraintName("order_details_request_id_fkey");

            entity.HasOne(d => d.Vendor).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.VendorId)
                .HasConstraintName("order_details_vendor_id_fkey");
        });

        modelBuilder.Entity<Physician>(entity =>
        {
            entity.HasKey(e => e.PhysicianId).HasName("physician_pkey");

            entity.ToTable("physician");

            entity.Property(e => e.PhysicianId).HasColumnName("physician_id");
            entity.Property(e => e.Address1)
                .HasMaxLength(500)
                .HasColumnName("address1");
            entity.Property(e => e.Address2)
                .HasMaxLength(500)
                .HasColumnName("address2");
            entity.Property(e => e.AdminNotes)
                .HasMaxLength(500)
                .HasColumnName("admin_notes");
            entity.Property(e => e.AltPhone)
                .HasMaxLength(20)
                .HasColumnName("alt_phone");
            entity.Property(e => e.AspNetUserId).HasColumnName("asp_net_user_id");
            entity.Property(e => e.BusinessName)
                .HasMaxLength(100)
                .HasColumnName("business_name");
            entity.Property(e => e.BusinessWebsite)
                .HasMaxLength(200)
                .HasColumnName("business_website");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.IsAgreementDoc).HasColumnName("is_agreement_doc");
            entity.Property(e => e.IsBackgroundDoc).HasColumnName("is_background_doc");
            entity.Property(e => e.IsCredentialDoc).HasColumnName("is_credential_doc");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.IsLicenseDoc).HasColumnName("is_license_doc");
            entity.Property(e => e.IsNonDisclosureDoc).HasColumnName("is_non_disclosure_doc");
            entity.Property(e => e.IsTokenGenerate).HasColumnName("is_token_generate");
            entity.Property(e => e.IsTrainingDoc).HasColumnName("is_training_doc");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.MedicalLicense)
                .HasMaxLength(500)
                .HasColumnName("medical_license");
            entity.Property(e => e.Mobile)
                .HasMaxLength(20)
                .HasColumnName("mobile");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");
            entity.Property(e => e.NpiNumber)
                .HasMaxLength(500)
                .HasColumnName("npi_number");
            entity.Property(e => e.Photo)
                .HasMaxLength(100)
                .HasColumnName("photo");
            entity.Property(e => e.RegionId).HasColumnName("region_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Signature)
                .HasMaxLength(100)
                .HasColumnName("signature");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.SyncEmailAddress)
                .HasMaxLength(50)
                .HasColumnName("sync_email_address");
            entity.Property(e => e.ZipCode)
                .HasMaxLength(10)
                .HasColumnName("zip_code");

            entity.HasOne(d => d.AspNetUser).WithMany(p => p.PhysicianAspNetUsers)
                .HasForeignKey(d => d.AspNetUserId)
                .HasConstraintName("physician_asp_net_user_id_fkey");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PhysicianCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("physician_created_by_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.PhysicianModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("physician_modified_by_fkey");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.RegionId).HasName("region_pkey");

            entity.ToTable("region");

            entity.Property(e => e.RegionId).HasColumnName("region_id");
            entity.Property(e => e.Abbreviation)
                .HasMaxLength(50)
                .HasColumnName("abbreviation");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("request_pkey");

            entity.ToTable("request");

            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.AcceptedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("accepted_date");
            entity.Property(e => e.CallType).HasColumnName("call_type");
            entity.Property(e => e.CaseNumber)
                .HasMaxLength(100)
                .HasColumnName("case_number");
            entity.Property(e => e.CaseTag)
                .HasMaxLength(50)
                .HasColumnName("case_tag");
            entity.Property(e => e.CaseTagPhysician)
                .HasMaxLength(50)
                .HasColumnName("case_tag_physician");
            entity.Property(e => e.CompletedByPhysician).HasColumnName("completed_by_physician");
            entity.Property(e => e.ConfirmationNumber)
                .HasMaxLength(20)
                .HasColumnName("confirmation_number");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
            entity.Property(e => e.DeclinedBy)
                .HasMaxLength(250)
                .HasColumnName("declined_by");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("is_deleted");
            entity.Property(e => e.IsMobile)
                .HasColumnType("bit(1)")
                .HasColumnName("is_mobile");
            entity.Property(e => e.IsUrgentEmailSent).HasColumnName("is_urgent_email_sent");
            entity.Property(e => e.LasWellnessDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("las_wellness_date");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.LastReservationDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("last_reservation_date");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");
            entity.Property(e => e.PatientAccountId).HasColumnName("patient_account_id");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(23)
                .HasColumnName("phone_number");
            entity.Property(e => e.PhysicianId).HasColumnName("physician_id");
            entity.Property(e => e.RelationName)
                .HasMaxLength(100)
                .HasColumnName("relation_name");
            entity.Property(e => e.RequestType)
                .HasDefaultValueSql("2")
                .HasColumnName("request_type");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("1")
                .HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.CreatedUser).WithMany(p => p.RequestCreatedUsers)
                .HasForeignKey(d => d.CreatedUserId)
                .HasConstraintName("request_created_user_id_fkey");

            entity.HasOne(d => d.Physician).WithMany(p => p.Requests)
                .HasForeignKey(d => d.PhysicianId)
                .HasConstraintName("request_physician_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.RequestUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("request_user_id_fkey");
        });

        modelBuilder.Entity<RequestBusiness>(entity =>
        {
            entity.HasKey(e => e.RequestBusinessId).HasName("request_business_pkey");

            entity.ToTable("request_business");

            entity.Property(e => e.RequestBusinessId).HasColumnName("request_business_id");
            entity.Property(e => e.BusinessId).HasColumnName("business_id");
            entity.Property(e => e.Ip)
                .HasDefaultValueSql("inet_client_addr()")
                .HasColumnName("ip");
            entity.Property(e => e.RequestId).HasColumnName("request_id");

            entity.HasOne(d => d.Business).WithMany(p => p.RequestBusinesses)
                .HasForeignKey(d => d.BusinessId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("request_business_business_id_fkey");

            entity.HasOne(d => d.Request).WithMany(p => p.RequestBusinesses)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("request_business_request_id_fkey");
        });

        modelBuilder.Entity<RequestClient>(entity =>
        {
            entity.HasKey(e => e.RequestClientId).HasName("request_client_pkey");

            entity.ToTable("request_client");

            entity.Property(e => e.RequestClientId).HasColumnName("request_client_id");
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .HasColumnName("address");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.CommunicationType).HasColumnName("communication_type");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.IntDate).HasColumnName("int_date");
            entity.Property(e => e.IntYear).HasColumnName("int_year");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.IsMobile)
                .HasColumnType("bit(1)")
                .HasColumnName("is_mobile");
            entity.Property(e => e.IsReservationReminderSent).HasColumnName("is_reservation_reminder_sent");
            entity.Property(e => e.IsSetFollowUpSent).HasColumnName("is_set_follow_up_sent");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .HasColumnName("location");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
            entity.Property(e => e.Notes)
                .HasMaxLength(500)
                .HasColumnName("notes");
            entity.Property(e => e.NotiEmail)
                .HasMaxLength(50)
                .HasColumnName("noti_email");
            entity.Property(e => e.NotiMobile)
                .HasMaxLength(20)
                .HasColumnName("noti_mobile");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(23)
                .HasColumnName("phone_number");
            entity.Property(e => e.RegionId).HasColumnName("region_id");
            entity.Property(e => e.RemindHouseCallCount).HasColumnName("remind_house_call_count");
            entity.Property(e => e.RemindReservationCount).HasColumnName("remind_reservation_count");
            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.State)
                .HasMaxLength(100)
                .HasColumnName("state");
            entity.Property(e => e.StrMonth)
                .HasMaxLength(20)
                .HasColumnName("str_month");
            entity.Property(e => e.Street)
                .HasMaxLength(100)
                .HasColumnName("street");
            entity.Property(e => e.ZipCode)
                .HasMaxLength(10)
                .HasColumnName("zip_code");

            entity.HasOne(d => d.Region).WithMany(p => p.RequestClients)
                .HasForeignKey(d => d.RegionId)
                .HasConstraintName("request_client_region_id_fkey");

            entity.HasOne(d => d.Request).WithMany(p => p.RequestClients)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("request_client_request_id_fkey");
        });

        modelBuilder.Entity<RequestConcierge>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("request_concierge_pkey");

            entity.ToTable("request_concierge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ConciergeId).HasColumnName("concierge_id");
            entity.Property(e => e.Ip)
                .HasDefaultValueSql("inet_client_addr()")
                .HasColumnName("ip");
            entity.Property(e => e.RequestId).HasColumnName("request_id");

            entity.HasOne(d => d.Concierge).WithMany(p => p.RequestConcierges)
                .HasForeignKey(d => d.ConciergeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("request_concierge_concierge_id_fkey");

            entity.HasOne(d => d.Request).WithMany(p => p.RequestConcierges)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("request_concierge_request_id_fkey");
        });

        modelBuilder.Entity<RequestNote>(entity =>
        {
            entity.HasKey(e => e.RequestNotesId).HasName("RequestNotes_pkey");

            entity.Property(e => e.AdminNotes).HasMaxLength(500);
            entity.Property(e => e.AdministrativeNotes).HasMaxLength(500);
            entity.Property(e => e.CreatedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.IntDate).HasColumnName("intDate");
            entity.Property(e => e.IntYear).HasColumnName("intYear");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("IP");
            entity.Property(e => e.ModifiedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.PhysicianNotes).HasMaxLength(500);
            entity.Property(e => e.StrMonth)
                .HasMaxLength(20)
                .HasColumnName("strMonth");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.RequestNoteCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("RequestNotes_CreatedBy_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.RequestNoteModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("RequestNotes_ModifiedBy_fkey");

            entity.HasOne(d => d.Request).WithMany(p => p.RequestNotes)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RequestNotes_RequestId_fkey");
        });

        modelBuilder.Entity<RequestStatusLog>(entity =>
        {
            entity.HasKey(e => e.RequestStatusLogId).HasName("request_status_log_pkey");

            entity.ToTable("request_status_log");

            entity.Property(e => e.RequestStatusLogId).HasColumnName("request_status_log_id");
            entity.Property(e => e.AdminId).HasColumnName("admin_id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.Ip)
                .HasDefaultValueSql("inet_client_addr()")
                .HasColumnName("ip");
            entity.Property(e => e.Notes)
                .HasMaxLength(500)
                .HasColumnName("notes");
            entity.Property(e => e.PhysicianId).HasColumnName("physician_id");
            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TransToAdmin).HasColumnName("trans_to_admin");
            entity.Property(e => e.TransToPhysicianId).HasColumnName("trans_to_physician_id");

            entity.HasOne(d => d.Admin).WithMany(p => p.RequestStatusLogs)
                .HasForeignKey(d => d.AdminId)
                .HasConstraintName("request_status_log_admin_id_fkey");

            entity.HasOne(d => d.Physician).WithMany(p => p.RequestStatusLogPhysicians)
                .HasForeignKey(d => d.PhysicianId)
                .HasConstraintName("request_status_log_physician_id_fkey");

            entity.HasOne(d => d.Request).WithMany(p => p.RequestStatusLogs)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("request_status_log_request_id_fkey");

            entity.HasOne(d => d.TransToPhysician).WithMany(p => p.RequestStatusLogTransToPhysicians)
                .HasForeignKey(d => d.TransToPhysicianId)
                .HasConstraintName("request_status_log_trans_to_physician_id_fkey");
        });

        modelBuilder.Entity<RequestWiseFile>(entity =>
        {
            entity.HasKey(e => e.RequestWiseFileId).HasName("request_wise_file_pkey");

            entity.ToTable("request_wise_file");

            entity.Property(e => e.RequestWiseFileId).HasColumnName("request_wise_file_id");
            entity.Property(e => e.AdminId).HasColumnName("admin_id");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.DocType).HasColumnName("doc_type");
            entity.Property(e => e.FileName)
                .HasMaxLength(500)
                .HasColumnName("file_name");
            entity.Property(e => e.Ip)
                .HasDefaultValueSql("inet_client_addr()")
                .HasColumnName("ip");
            entity.Property(e => e.IsCompensation).HasColumnName("is_compensation");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.IsFinalize).HasColumnName("is_finalize");
            entity.Property(e => e.IsFrontSide).HasColumnName("is_front_side");
            entity.Property(e => e.IsPatientRecords).HasColumnName("is_patient_records");
            entity.Property(e => e.PhysicianId).HasColumnName("physician_id");
            entity.Property(e => e.RequestId).HasColumnName("request_id");

            entity.HasOne(d => d.Admin).WithMany(p => p.RequestWiseFiles)
                .HasForeignKey(d => d.AdminId)
                .HasConstraintName("request_wise_file_admin_id_fkey");

            entity.HasOne(d => d.Physician).WithMany(p => p.RequestWiseFiles)
                .HasForeignKey(d => d.PhysicianId)
                .HasConstraintName("request_wise_file_physician_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("user_pkey");

            entity.ToTable("user");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.AspNetUserId).HasColumnName("asp_net_user_id");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(128)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.IntDate).HasColumnName("int_date");
            entity.Property(e => e.IntYear).HasColumnName("int_year");
            entity.Property(e => e.Ip)
                .HasMaxLength(20)
                .HasColumnName("ip");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("is_deleted");
            entity.Property(e => e.IsMobile)
                .HasColumnType("bit(1)")
                .HasColumnName("is_mobile");
            entity.Property(e => e.IsRequestWithEmail)
                .HasColumnType("bit(1)")
                .HasColumnName("is_request_with_email");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.Mobile)
                .HasMaxLength(20)
                .HasColumnName("mobile");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(128)
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_date");
            entity.Property(e => e.RegionId).HasColumnName("region_id");
            entity.Property(e => e.State)
                .HasMaxLength(100)
                .HasColumnName("state");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.StrMonth)
                .HasMaxLength(20)
                .HasColumnName("str_month");
            entity.Property(e => e.Street)
                .HasMaxLength(100)
                .HasColumnName("street");
            entity.Property(e => e.ZipCode)
                .HasMaxLength(10)
                .HasColumnName("zip_code");

            entity.HasOne(d => d.AspNetUser).WithMany(p => p.Users)
                .HasForeignKey(d => d.AspNetUserId)
                .HasConstraintName("user_asp_net_user_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
