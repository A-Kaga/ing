﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sakuno.ING.Data;

namespace Sakuno.ING.Data.Migrations
{
    [DbContext(typeof(SettingsManager))]
    partial class SettingsManagerModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rc1-32029");

            modelBuilder.Entity("Sakuno.ING.Data.SettingDbEntry", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.ToTable("SettingEntries");
                });
#pragma warning restore 612, 618
        }
    }
}
