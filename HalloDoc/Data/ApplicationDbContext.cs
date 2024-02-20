using System;
using System.Collections.Generic;
using HalloDoc.Models;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.Data;

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

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<Business> Businesses { get; set; }

    public virtual DbSet<Concierge> Concierges { get; set; }

    public virtual DbSet<Physician> Physicians { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<RequestBusiness> RequestBusinesses { get; set; }

    public virtual DbSet<RequestClient> RequestClients { get; set; }

    public virtual DbSet<RequestConcierge> RequestConcierges { get; set; }

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
                .HasConstraintName("admin_asp_net_user_id_fkey");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AdminCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("admin_created_by_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.AdminModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("admin_modified_by_fkey");
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
