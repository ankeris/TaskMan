﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TaskManagement.Models
{
    public partial class ManagementContext : DbContext
    {
        public ManagementContext()
        {
        }

        public ManagementContext(DbContextOptions<ManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<JAccountTask> JAccountTask { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<Task> Task { get; set; }
        public virtual DbSet<TaskState> TaskState { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Management;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.AccountCreatedDateTime)
                    .HasColumnName("Account_Created_DateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.AccountEmail)
                    .IsRequired()
                    .HasColumnName("Account_Email")
                    .HasMaxLength(120);

                entity.Property(e => e.AccountPassword)
                    .IsRequired()
                    .HasColumnName("Account_Password");

                entity.Property(e => e.AccountRoleId).HasColumnName("Account_Role_ID");

                entity.Property(e => e.AccountRoleLastChangeDateTime)
                    .HasColumnName("Account_Role_LastChange_DateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.AccountUserBirthDate)
                    .HasColumnName("Account_User_BirthDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.AccountUserCompanyId).HasColumnName("Account_User_Company_ID");

                entity.Property(e => e.AccountUserCompanyLastChangeDateTime)
                    .HasColumnName("Account_User_Company_LastChange_DateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.AccountUserFirstName)
                    .HasColumnName("Account_User_FirstName")
                    .HasMaxLength(80);

                entity.Property(e => e.AccountUserLastName)
                    .HasColumnName("Account_User_LastName")
                    .HasMaxLength(80);

                entity.HasOne(d => d.AccountRole)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.AccountRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Account__Account__25869641");

                entity.HasOne(d => d.AccountUserCompany)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.AccountUserCompanyId)
                    .HasConstraintName("FK__Account__Account__29572725");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.CommentId).HasColumnName("Comment_ID");

                entity.Property(e => e.CommentAccountId).HasColumnName("Comment_Account_ID");

                entity.Property(e => e.CommentProjectId).HasColumnName("Comment_Project_ID");

                entity.Property(e => e.CommentTaskId).HasColumnName("Comment_Task_ID");

                entity.Property(e => e.CommentText)
                    .HasColumnName("Comment_Text")
                    .HasMaxLength(4000);

                entity.HasOne(d => d.CommentAccount)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.CommentAccountId)
                    .HasConstraintName("FK__Comment__Comment__37A5467C");

                entity.HasOne(d => d.CommentProject)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.CommentProjectId)
                    .HasConstraintName("FK__Comment__Comment__38996AB5");

                entity.HasOne(d => d.CommentTask)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.CommentTaskId)
                    .HasConstraintName("FK__Comment__Comment__398D8EEE");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.CompanyId).HasColumnName("Company_ID");

                entity.Property(e => e.CompanyCreatedDateTime)
                    .HasColumnName("Company_Created_DateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.CompanyCreatorAccountId).HasColumnName("Company_Creator_Account_ID");

                entity.Property(e => e.CompanyInfo)
                    .HasColumnName("Company_Info")
                    .HasMaxLength(400);

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasColumnName("Company_Name")
                    .HasMaxLength(200);

                entity.HasOne(d => d.CompanyCreatorAccount)
                    .WithMany(p => p.Company)
                    .HasForeignKey(d => d.CompanyCreatorAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Company__Company__286302EC");
            });

            modelBuilder.Entity<JAccountTask>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.TaskId });

                entity.ToTable("J_AccountTask");

                entity.Property(e => e.AccountId).HasColumnName("Account_ID");

                entity.Property(e => e.TaskId).HasColumnName("Task_ID");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.JAccountTask)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__J_Account__Accou__33D4B598");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.JAccountTask)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__J_Account__Task___34C8D9D1");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.ProjectId).HasColumnName("Project_ID");

                entity.Property(e => e.ProjectActive).HasColumnName("Project_Active");

                entity.Property(e => e.ProjectCreatedDateTime)
                    .HasColumnName("Project_Created_DateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.ProjectCreatorAccountId).HasColumnName("Project_Creator_Account_ID");

                entity.Property(e => e.ProjectDeadline)
                    .HasColumnName("Project_Deadline")
                    .HasColumnType("date");

                entity.Property(e => e.ProjectDescription)
                    .HasColumnName("Project_Description")
                    .HasMaxLength(4000);

                entity.Property(e => e.ProjectEndDateTime)
                    .HasColumnName("Project_End_DateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.ProjectName)
                    .IsRequired()
                    .HasColumnName("Project_Name")
                    .HasMaxLength(150);

                entity.HasOne(d => d.ProjectCreatorAccount)
                    .WithMany(p => p.Project)
                    .HasForeignKey(d => d.ProjectCreatorAccountId)
                    .HasConstraintName("FK__Project__Project__2C3393D0");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.Property(e => e.TaskId).HasColumnName("Task_ID");

                entity.Property(e => e.TaskCreatedDateTime)
                    .HasColumnName("Task_Created_DateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.TaskDescription)
                    .HasColumnName("Task_Description")
                    .HasMaxLength(3000);

                entity.Property(e => e.TaskName)
                    .IsRequired()
                    .HasColumnName("Task_Name")
                    .HasMaxLength(200);

                entity.Property(e => e.TaskProjectId).HasColumnName("Task_Project_ID");

                entity.Property(e => e.TaskTaskStateId).HasColumnName("Task_TaskState_ID");

                entity.Property(e => e.TaskTaskStateLastChangeDateTime)
                    .HasColumnName("Task_TaskState_LastChange_DateTime")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.TaskTaskState)
                    .WithMany(p => p.Task)
                    .HasForeignKey(d => d.TaskTaskStateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Task__Task_TaskS__30F848ED");
            });

            modelBuilder.Entity<TaskState>(entity =>
            {
                entity.Property(e => e.TaskStateId).HasColumnName("TaskState_ID");

                entity.Property(e => e.TaskStateName)
                    .IsRequired()
                    .HasColumnName("TaskState_Name")
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.Property(e => e.UserRoleId).HasColumnName("UserRole_ID");

                entity.Property(e => e.UserRoleName)
                    .HasColumnName("UserRole_Name")
                    .HasMaxLength(150);
            });
        }
    }
}
