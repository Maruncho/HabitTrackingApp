﻿// <auto-generated />
using System;
using HTApp.Infrastructure.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HTApp.Infrastructure.EntityModels.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241206183345_StupidMistake")]
    partial class StupidMistake
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HTApp.Infrastructure.EntityModels.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Credits")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<byte>("RefundsPerSession")
                        .HasColumnType("tinyint");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("HTApp.Infrastructure.EntityModels.Core.BadHabit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CreditsFail")
                        .HasColumnType("int");

                    b.Property<int>("CreditsSuccess")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("BadHabits");
                });

            modelBuilder.Entity("HTApp.Infrastructure.EntityModels.Core.GoodHabit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CreditsFail")
                        .HasColumnType("int");

                    b.Property<int>("CreditsSuccess")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("GoodHabits");
                });

            modelBuilder.Entity("HTApp.Infrastructure.EntityModels.Core.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<int?>("SessionId")
                        .HasColumnType("int");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("SessionId");

                    b.HasIndex("TypeId");

                    b.HasIndex("UserId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("HTApp.Infrastructure.EntityModels.Core.TransactionType", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.ToTable("TransactionTypes");

                    b.HasData(
                        new
                        {
                            Id = 0,
                            Message = "Credits From Unknown Source"
                        },
                        new
                        {
                            Id = 1,
                            Message = "Credits From Manual Insertion"
                        },
                        new
                        {
                            Id = 2,
                            Message = "Credits From ... AHHHH I DON'T REMEMBER"
                        },
                        new
                        {
                            Id = 100,
                            Message = "Credits Earned From Finishing a Good Habit"
                        },
                        new
                        {
                            Id = 101,
                            Message = "Credits Lost From Failing a Good Habit"
                        },
                        new
                        {
                            Id = 102,
                            Message = "Credits Lost From Change of Status of a Good Habit"
                        },
                        new
                        {
                            Id = 200,
                            Message = "Credits Eearned From Quitting a Bad Habit"
                        },
                        new
                        {
                            Id = 201,
                            Message = "Credits Lost From Failing To Quit a Bad Habit"
                        },
                        new
                        {
                            Id = 202,
                            Message = "Credits Earned From Change Of Status of a Bad Habit"
                        },
                        new
                        {
                            Id = 300,
                            Message = "Credits Lost From Buying a Treat. Cheers!"
                        },
                        new
                        {
                            Id = 301,
                            Message = "Credits Earned From Refunding a Treat. Uncheers!"
                        });
                });

            modelBuilder.Entity("HTApp.Infrastructure.EntityModels.Core.Treat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CreditsPrice")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<byte>("QuantityPerSession")
                        .HasColumnType("tinyint");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Treats");
                });

            modelBuilder.Entity("HTApp.Infrastructure.EntityModels.SessionModels.Session", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Last")
                        .HasColumnType("bit");

                    b.Property<int?>("PreviousSessionId")
                        .HasColumnType("int");

                    b.Property<byte>("Refunds")
                        .HasColumnType("tinyint");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("PreviousSessionId");

                    b.HasIndex("UserId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("HTApp.Infrastructure.EntityModels.SessionModels.SessionBadHabit", b =>
                {
                    b.Property<int>("SessionId")
                        .HasColumnType("int");

                    b.Property<int>("BadHabitId")
                        .HasColumnType("int");

                    b.Property<bool>("Failed")
                        .HasColumnType("bit");

                    b.HasKey("SessionId", "BadHabitId");

                    b.HasIndex("BadHabitId");

                    b.ToTable("SessionBadHabits");
                });

            modelBuilder.Entity("HTApp.Infrastructure.EntityModels.SessionModels.SessionGoodHabit", b =>
                {
                    b.Property<int>("SessionId")
                        .HasColumnType("int");

                    b.Property<int>("GoodHabitId")
                        .HasColumnType("int");

                    b.Property<bool>("Completed")
                        .HasColumnType("bit");

                    b.HasKey("SessionId", "GoodHabitId");

                    b.HasIndex("GoodHabitId");

                    b.ToTable("SessionGoodHabits");
                });

            modelBuilder.Entity("HTApp.Infrastructure.EntityModels.SessionModels.SessionTreat", b =>
                {
                    b.Property<int>("SessionId")
                        .HasColumnType("int");

                    b.Property<int>("TreatId")
                        .HasColumnType("int");

                    b.Property<byte>("UnitsLeft")
                        .HasColumnType("tinyint");

                    b.HasKey("SessionId", "TreatId");

                    b.HasIndex("TreatId");

                    b.ToTable("SessionTreats");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("HTApp.Infrastructure.EntityModels.Core.BadHabit", b =>
                {
                    b.HasOne("HTApp.Infrastructure.EntityModels.AppUser", "User")
                        .WithMany("BadHabits")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("HTApp.Infrastructure.EntityModels.Core.GoodHabit", b =>
                {
                    b.HasOne("HTApp.Infrastructure.EntityModels.AppUser", "User")
                        .WithMany("GoodHabits")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("HTApp.Infrastructure.EntityModels.Core.Transaction", b =>
                {
                    b.HasOne("HTApp.Infrastructure.EntityModels.SessionModels.Session", "Session")
                        .WithMany("Transactions")
                        .HasForeignKey("SessionId");

                    b.HasOne("HTApp.Infrastructure.EntityModels.Core.TransactionType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HTApp.Infrastructure.EntityModels.AppUser", "User")
                        .WithMany("Transactions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Session");

                    b.Navigation("Type");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HTApp.Infrastructure.EntityModels.Core.Treat", b =>
                {
                    b.HasOne("HTApp.Infrastructure.EntityModels.AppUser", "User")
                        .WithMany("Treats")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("HTApp.Infrastructure.EntityModels.SessionModels.Session", b =>
                {
                    b.HasOne("HTApp.Infrastructure.EntityModels.SessionModels.Session", "PreviousSession")
                        .WithMany()
                        .HasForeignKey("PreviousSessionId");

                    b.HasOne("HTApp.Infrastructure.EntityModels.AppUser", "User")
                        .WithMany("Sessions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PreviousSession");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HTApp.Infrastructure.EntityModels.SessionModels.SessionBadHabit", b =>
                {
                    b.HasOne("HTApp.Infrastructure.EntityModels.Core.BadHabit", "BadHabit")
                        .WithMany("SessionBadHabits")
                        .HasForeignKey("BadHabitId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("HTApp.Infrastructure.EntityModels.SessionModels.Session", "Session")
                        .WithMany("SessionBadHabits")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BadHabit");

                    b.Navigation("Session");
                });

            modelBuilder.Entity("HTApp.Infrastructure.EntityModels.SessionModels.SessionGoodHabit", b =>
                {
                    b.HasOne("HTApp.Infrastructure.EntityModels.Core.GoodHabit", "GoodHabit")
                        .WithMany("SessionGoodHabits")
                        .HasForeignKey("GoodHabitId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("HTApp.Infrastructure.EntityModels.SessionModels.Session", "Session")
                        .WithMany("SessionGoodHabits")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GoodHabit");

                    b.Navigation("Session");
                });

            modelBuilder.Entity("HTApp.Infrastructure.EntityModels.SessionModels.SessionTreat", b =>
                {
                    b.HasOne("HTApp.Infrastructure.EntityModels.SessionModels.Session", "Session")
                        .WithMany("SessionTreats")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HTApp.Infrastructure.EntityModels.Core.Treat", "Treat")
                        .WithMany("SessionTreats")
                        .HasForeignKey("TreatId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Session");

                    b.Navigation("Treat");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("HTApp.Infrastructure.EntityModels.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("HTApp.Infrastructure.EntityModels.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HTApp.Infrastructure.EntityModels.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("HTApp.Infrastructure.EntityModels.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HTApp.Infrastructure.EntityModels.AppUser", b =>
                {
                    b.Navigation("BadHabits");

                    b.Navigation("GoodHabits");

                    b.Navigation("Sessions");

                    b.Navigation("Transactions");

                    b.Navigation("Treats");
                });

            modelBuilder.Entity("HTApp.Infrastructure.EntityModels.Core.BadHabit", b =>
                {
                    b.Navigation("SessionBadHabits");
                });

            modelBuilder.Entity("HTApp.Infrastructure.EntityModels.Core.GoodHabit", b =>
                {
                    b.Navigation("SessionGoodHabits");
                });

            modelBuilder.Entity("HTApp.Infrastructure.EntityModels.Core.Treat", b =>
                {
                    b.Navigation("SessionTreats");
                });

            modelBuilder.Entity("HTApp.Infrastructure.EntityModels.SessionModels.Session", b =>
                {
                    b.Navigation("SessionBadHabits");

                    b.Navigation("SessionGoodHabits");

                    b.Navigation("SessionTreats");

                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
