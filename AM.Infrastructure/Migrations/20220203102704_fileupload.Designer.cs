﻿// <auto-generated />
using System;
using AM.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AM.Infrastructure.Migrations
{
    [DbContext(typeof(AMContext))]
    [Migration("20220203102704_fileupload")]
    partial class fileupload
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AM.Domain.BlogAggregate.Blog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasMaxLength(3000)
                        .HasColumnType("nvarchar(3000)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Image")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("ShortDescription")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Blog", "dbo");
                });

            modelBuilder.Entity("AM.Domain.ContactUsAggregate.ContactUs", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasMaxLength(1500)
                        .HasColumnType("nvarchar(1500)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("Phone")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("ContactUs", "dbo");
                });

            modelBuilder.Entity("AM.Domain.Deal", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<int>("ClosingStatus")
                        .HasColumnType("int");

                    b.Property<string>("ContractFile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Cost")
                        .HasColumnType("float");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DueTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long>("ListingId")
                        .HasColumnType("bigint");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PaymentStatus")
                        .HasColumnType("bit");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("TrackingCode")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.HasIndex("ListingId");

                    b.ToTable("Deals", "dbo");
                });

            modelBuilder.Entity("AM.Domain.ListingAggregate.Listing", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Amount")
                        .HasMaxLength(50)
                        .HasColumnType("float");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("DeliveryMethod")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("HasAmount")
                        .HasColumnType("bit");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsService")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("UnitPrice")
                        .HasColumnType("float");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Listing", "dbo");
                });

            modelBuilder.Entity("AM.Domain.NegotiateAggregate.Negotiate", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("BuyerId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsFinished")
                        .HasColumnType("bit");

                    b.Property<long>("ListingId")
                        .HasColumnType("bigint");

                    b.Property<long>("SellerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ListingId");

                    b.ToTable("Negotiate", "dbo");
                });

            modelBuilder.Entity("AM.Domain.NotificationAggregate.Notification", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("NotificationBody")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("NotificationTitle")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<long>("SenderId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notification", "dbo");
                });

            modelBuilder.Entity("AM.Domain.NotificationAggregate.Recipient", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsReed")
                        .HasColumnType("bit");

                    b.Property<long>("NotificationId")
                        .HasColumnType("bigint");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("NotificationId");

                    b.ToTable("Recipient", "dbo");
                });

            modelBuilder.Entity("AM.Domain.ResetPasswordAggregate.ResetPassword", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExprateDateTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsValid")
                        .HasColumnType("bit");

                    b.Property<Guid>("ResetUrl")
                        .HasMaxLength(32)
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("ResetPassword", "dbo");
                });

            modelBuilder.Entity("AM.Domain.RoleAggregate.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Roles", "dbo");
                });

            modelBuilder.Entity("AM.Domain.Supplied.PurchasedAggregate.PurchasedItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<long>("ListingId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ListingId");

                    b.ToTable("PurchasedItem", "dbo");
                });

            modelBuilder.Entity("AM.Domain.Supplied.PurchasedAggregate.SuppliedItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<long>("ListingId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ListingId");

                    b.ToTable("SuppliedItem", "dbo");
                });

            modelBuilder.Entity("AM.Domain.UserAggregate.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid>("ActivationGuid")
                        .HasMaxLength(36)
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("Avatar")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("City")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("CompanyName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Country")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<long?>("DealId")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FaceBookUrl")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("FirstName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("InstagramUrl")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<string>("LinkdinUrl")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<long>("PostalCode")
                        .HasColumnType("bigint");

                    b.Property<long?>("PurchasedItemId")
                        .HasColumnType("bigint");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<long?>("SuppliedItemId")
                        .HasColumnType("bigint");

                    b.Property<string>("TwitterUrl")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("User_Type")
                        .HasColumnType("int");

                    b.Property<long>("VatNumber")
                        .HasColumnType("bigint");

                    b.Property<string>("WebUrl")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("DealId");

                    b.HasIndex("PurchasedItemId");

                    b.HasIndex("RoleId");

                    b.HasIndex("SuppliedItemId");

                    b.ToTable("Users", "dbo");
                });

            modelBuilder.Entity("AM.Domain.BlogAggregate.Blog", b =>
                {
                    b.HasOne("AM.Domain.UserAggregate.User", "User")
                        .WithMany("Blogs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("AM.Domain.Deal", b =>
                {
                    b.HasOne("AM.Domain.ListingAggregate.Listing", "Listing")
                        .WithMany("DealList")
                        .HasForeignKey("ListingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Listing");
                });

            modelBuilder.Entity("AM.Domain.ListingAggregate.Listing", b =>
                {
                    b.HasOne("AM.Domain.UserAggregate.User", "User")
                        .WithMany("Listings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("AM.Domain.ListingAggregate.ListingOperation", "ListingOperations", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<double>("Count")
                                .HasColumnType("float");

                            b1.Property<double>("CurrentAmount")
                                .HasColumnType("float");

                            b1.Property<long>("DealId")
                                .HasColumnType("bigint");

                            b1.Property<string>("Description")
                                .HasMaxLength(300)
                                .HasColumnType("nvarchar(300)");

                            b1.Property<long>("ListingId")
                                .HasColumnType("bigint");

                            b1.Property<DateTime>("OperationTime")
                                .HasColumnType("datetime2");

                            b1.Property<bool>("OperationType")
                                .HasColumnType("bit");

                            b1.Property<long>("UserId")
                                .HasColumnType("bigint");

                            b1.HasKey("Id");

                            b1.HasIndex("ListingId");

                            b1.ToTable("ListingOperation", "dbo");

                            b1.WithOwner("Listing")
                                .HasForeignKey("ListingId");

                            b1.Navigation("Listing");
                        });

                    b.Navigation("ListingOperations");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AM.Domain.NegotiateAggregate.Negotiate", b =>
                {
                    b.HasOne("AM.Domain.ListingAggregate.Listing", "Listing")
                        .WithMany("NegotiateList")
                        .HasForeignKey("ListingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("AM.Domain.NegotiateAggregate.Message", "Messages", b1 =>
                        {
                            b1.Property<long>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("bigint")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<DateTime>("CreationTime")
                                .HasColumnType("datetime2");

                            b1.Property<string>("FilePathString")
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)");

                            b1.Property<bool>("IsRead")
                                .HasColumnType("bit");

                            b1.Property<string>("MessageBody")
                                .IsRequired()
                                .HasMaxLength(500)
                                .HasColumnType("nvarchar(500)");

                            b1.Property<long>("NegotiateId")
                                .HasColumnType("bigint");

                            b1.Property<bool>("UserEntity")
                                .HasColumnType("bit");

                            b1.Property<long>("UserId")
                                .HasColumnType("bigint");

                            b1.HasKey("Id");

                            b1.HasIndex("NegotiateId");

                            b1.ToTable("NegitiateMessages");

                            b1.WithOwner("Negotiate")
                                .HasForeignKey("NegotiateId");

                            b1.Navigation("Negotiate");
                        });

                    b.Navigation("Listing");

                    b.Navigation("Messages");
                });

            modelBuilder.Entity("AM.Domain.NotificationAggregate.Notification", b =>
                {
                    b.HasOne("AM.Domain.UserAggregate.User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("AM.Domain.NotificationAggregate.Recipient", b =>
                {
                    b.HasOne("AM.Domain.NotificationAggregate.Notification", "Notification")
                        .WithMany("Recipient")
                        .HasForeignKey("NotificationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Notification");
                });

            modelBuilder.Entity("AM.Domain.RoleAggregate.Role", b =>
                {
                    b.OwnsMany("AM.Domain.RoleAggregate.Permission", "Permissions", b1 =>
                        {
                            b1.Property<long>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("bigint")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<int>("Code")
                                .HasColumnType("int");

                            b1.Property<int>("RoleId")
                                .HasColumnType("int");

                            b1.HasKey("Id");

                            b1.HasIndex("RoleId");

                            b1.ToTable("RolePermissions");

                            b1.WithOwner("Role")
                                .HasForeignKey("RoleId");

                            b1.Navigation("Role");
                        });

                    b.Navigation("Permissions");
                });

            modelBuilder.Entity("AM.Domain.Supplied.PurchasedAggregate.PurchasedItem", b =>
                {
                    b.HasOne("AM.Domain.ListingAggregate.Listing", "Listing")
                        .WithMany("PurchaseList")
                        .HasForeignKey("ListingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Listing");
                });

            modelBuilder.Entity("AM.Domain.Supplied.PurchasedAggregate.SuppliedItem", b =>
                {
                    b.HasOne("AM.Domain.ListingAggregate.Listing", "Listing")
                        .WithMany("SupplyList")
                        .HasForeignKey("ListingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Listing");
                });

            modelBuilder.Entity("AM.Domain.UserAggregate.User", b =>
                {
                    b.HasOne("AM.Domain.Deal", "Deal")
                        .WithMany("Users")
                        .HasForeignKey("DealId");

                    b.HasOne("AM.Domain.Supplied.PurchasedAggregate.PurchasedItem", "PurchasedItem")
                        .WithMany("Users")
                        .HasForeignKey("PurchasedItemId");

                    b.HasOne("AM.Domain.RoleAggregate.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AM.Domain.Supplied.PurchasedAggregate.SuppliedItem", "SuppliedItem")
                        .WithMany("Users")
                        .HasForeignKey("SuppliedItemId");

                    b.Navigation("Deal");

                    b.Navigation("PurchasedItem");

                    b.Navigation("Role");

                    b.Navigation("SuppliedItem");
                });

            modelBuilder.Entity("AM.Domain.Deal", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("AM.Domain.ListingAggregate.Listing", b =>
                {
                    b.Navigation("DealList");

                    b.Navigation("NegotiateList");

                    b.Navigation("PurchaseList");

                    b.Navigation("SupplyList");
                });

            modelBuilder.Entity("AM.Domain.NotificationAggregate.Notification", b =>
                {
                    b.Navigation("Recipient");
                });

            modelBuilder.Entity("AM.Domain.RoleAggregate.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("AM.Domain.Supplied.PurchasedAggregate.PurchasedItem", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("AM.Domain.Supplied.PurchasedAggregate.SuppliedItem", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("AM.Domain.UserAggregate.User", b =>
                {
                    b.Navigation("Blogs");

                    b.Navigation("Listings");

                    b.Navigation("Notifications");
                });
#pragma warning restore 612, 618
        }
    }
}
