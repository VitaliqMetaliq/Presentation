﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Storage.Database;

#nullable disable

namespace Storage.Database.Migrations
{
    [DbContext(typeof(StorageDbContext))]
    [Migration("20221229124309_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Storage.Database.Entities.BaseCurrencyEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("EngName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ISOCharCode")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("IsoCode");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ParentCode")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("RId");

                    b.HasKey("Id");

                    b.ToTable("BaseCurrencies");
                });

            modelBuilder.Entity("Storage.Database.Entities.DailyCurrencyEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("BaseCurrencyId")
                        .HasColumnType("integer");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("Value")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("BaseCurrencyId");

                    b.HasIndex("CurrencyId");

                    b.ToTable("DailyCurrencies");
                });

            modelBuilder.Entity("Storage.Database.Entities.DailyCurrencyEntity", b =>
                {
                    b.HasOne("Storage.Database.Entities.BaseCurrencyEntity", "BaseCurrency")
                        .WithMany("BaseDailyCurrencyEntities")
                        .HasForeignKey("BaseCurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Storage.Database.Entities.BaseCurrencyEntity", "Currency")
                        .WithMany("DailyCurrencyEntities")
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BaseCurrency");

                    b.Navigation("Currency");
                });

            modelBuilder.Entity("Storage.Database.Entities.BaseCurrencyEntity", b =>
                {
                    b.Navigation("BaseDailyCurrencyEntities");

                    b.Navigation("DailyCurrencyEntities");
                });
#pragma warning restore 612, 618
        }
    }
}
