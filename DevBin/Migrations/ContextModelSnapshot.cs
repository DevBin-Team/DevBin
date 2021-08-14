﻿// <auto-generated />
using System;
using DevBin.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DevBin.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_general_ci")
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.8");

            modelBuilder.Entity("DevBin.Models.Exposure", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("id");

                    b.Property<bool>("AllowEdit")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("allowEdit");

                    b.Property<bool>("IsPrivate")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("isPrivate");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("isPublic");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)")
                        .HasColumnName("name");

                    b.Property<bool>("RegisteredOnly")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("registeredOnly");

                    b.HasKey("Id");

                    b.ToTable("exposures");
                });

            modelBuilder.Entity("DevBin.Models.Paste", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("id");

                    b.Property<int?>("AuthorId")
                        .HasColumnType("int(11)")
                        .HasColumnName("authorId");

                    b.Property<string>("Cache")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("cache");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)")
                        .HasColumnName("code");

                    b.Property<DateTime>("Datetime")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(6)
                        .HasColumnType("datetime(6)")
                        .HasColumnName("datetime")
                        .HasDefaultValueSql("current_timestamp(6)");

                    b.Property<int>("ExposureId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("exposureId")
                        .HasDefaultValueSql("'1'");

                    b.Property<int?>("SyntaxId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("syntaxId")
                        .HasDefaultValueSql("'1'");

                    b.Property<string>("Title")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("title")
                        .HasDefaultValueSql("'''Unnamed Paste'''");

                    b.Property<DateTime?>("UpdateDatetime")
                        .HasMaxLength(6)
                        .HasColumnType("datetime(6)")
                        .HasColumnName("updateDatetime");

                    b.Property<int>("Views")
                        .HasColumnType("int(11)")
                        .HasColumnName("views");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "AuthorId" }, "FK_pastes_Author_ToUsers");

                    b.HasIndex(new[] { "ExposureId" }, "FK_pastes_Exposure_ToExposures");

                    b.HasIndex(new[] { "SyntaxId" }, "FK_pastes_Syntax_ToSyntaxes");

                    b.HasIndex(new[] { "Code" }, "code")
                        .IsUnique();

                    b.ToTable("pastes");
                });

            modelBuilder.Entity("DevBin.Models.Session", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    b.Property<DateTime>("Datetime")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(6)
                        .HasColumnType("datetime(6)")
                        .HasDefaultValueSql("current_timestamp(6)");

                    b.Property<string>("Token")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int(11)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "UserId" }, "IX_Sessions_UserId");

                    b.ToTable("sessions");
                });

            modelBuilder.Entity("DevBin.Models.Syntaxes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("name");

                    b.Property<string>("Pretty")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("pretty");

                    b.HasKey("Id");

                    b.ToTable("syntaxes");
                });

            modelBuilder.Entity("DevBin.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)")
                        .HasColumnName("id");

                    b.Property<string>("ApiToken")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)")
                        .HasColumnName("apiToken");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("email");

                    b.Property<byte[]>("Password")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("binary(60)")
                        .HasColumnName("password")
                        .IsFixedLength(true);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("username");

                    b.Property<bool>("Verified")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("verified");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "ApiToken" }, "KEY_apiToken_UQ")
                        .IsUnique();

                    b.HasIndex(new[] { "Username" }, "KEY_username_UQ")
                        .IsUnique();

                    b.HasIndex(new[] { "Email" }, "email_UNIQUE")
                        .IsUnique();

                    b.ToTable("users");
                });

            modelBuilder.Entity("DevBin.Models.Paste", b =>
                {
                    b.HasOne("DevBin.Models.User", "Author")
                        .WithMany("Pastes")
                        .HasForeignKey("AuthorId")
                        .HasConstraintName("FK_pastes_Author_ToUsers")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DevBin.Models.Exposure", "Exposure")
                        .WithMany("Pastes")
                        .HasForeignKey("ExposureId")
                        .HasConstraintName("FK_pastes_Exposure_ToExposures")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DevBin.Models.Syntaxes", "Syntax")
                        .WithMany("Pastes")
                        .HasForeignKey("SyntaxId")
                        .HasConstraintName("FK_pastes_Syntax_ToSyntaxes")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Author");

                    b.Navigation("Exposure");

                    b.Navigation("Syntax");
                });

            modelBuilder.Entity("DevBin.Models.Session", b =>
                {
                    b.HasOne("DevBin.Models.User", "User")
                        .WithMany("Sessions")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_Sessions_users_UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DevBin.Models.Exposure", b =>
                {
                    b.Navigation("Pastes");
                });

            modelBuilder.Entity("DevBin.Models.Syntaxes", b =>
                {
                    b.Navigation("Pastes");
                });

            modelBuilder.Entity("DevBin.Models.User", b =>
                {
                    b.Navigation("Pastes");

                    b.Navigation("Sessions");
                });
#pragma warning restore 612, 618
        }
    }
}
