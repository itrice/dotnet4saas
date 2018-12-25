﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using iTrice.SAAS.TenantManager.Data;

namespace iTrice.SAAS.TenantManager.Migrations
{
    [DbContext(typeof(TenantContext))]
    partial class TenantContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("iTrice.SAAS.TenantManager.Models.SystemLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateTime");

                    b.HasKey("Id");

                    b.ToTable("SystemLog");
                });

            modelBuilder.Entity("iTrice.SAAS.TenantManager.Models.Tenant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("DataBase");

                    b.Property<int>("Flag");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<string>("URL");

                    b.HasKey("Id");

                    b.ToTable("Tenant");
                });
#pragma warning restore 612, 618
        }
    }
}
