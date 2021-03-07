﻿// <auto-generated />
using DBlab2;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DBlab2.Migrations
{
    [DbContext(typeof(LibraryContext))]
    partial class LibraryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.3");

            modelBuilder.Entity("DBlab2.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("DBlab2.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("PublisherId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Title")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PublisherId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("DBlab2.BookAuthor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AuthorId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BookId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("BookId");

                    b.ToTable("BookAuthors");
                });

            modelBuilder.Entity("DBlab2.Cathedra", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FacultyId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FacultyId");

                    b.ToTable("Cathedras");
                });

            modelBuilder.Entity("DBlab2.Faculty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Faculties");
                });

            modelBuilder.Entity("DBlab2.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CathedraId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Number")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SpecialityId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CathedraId");

                    b.HasIndex("SpecialityId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("DBlab2.LibraryEmployee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<int>("PositionId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PositionId");

                    b.ToTable("LibraryEmployees");
                });

            modelBuilder.Entity("DBlab2.Position", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("PosName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("DBlab2.Publisher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Publishers");
                });

            modelBuilder.Entity("DBlab2.Speciality", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FacultyId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("Number")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FacultyId");

                    b.ToTable("Specialities");
                });

            modelBuilder.Entity("DBlab2.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<int>("GroupId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<int>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("DBlab2.StudentCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BookId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DueDate")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LibraryEmployeeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ReturnedDate")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StudentId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TakenDate")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("LibraryEmployeeId");

                    b.HasIndex("StudentId");

                    b.ToTable("StudentCards");
                });

            modelBuilder.Entity("DBlab2.Teacher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CathedraId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("SecondName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CathedraId");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("DBlab2.TeacherCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BookId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LibraryEmployeeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ReturnedDate")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TakenDate")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TeacherId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("LibraryEmployeeId");

                    b.HasIndex("TeacherId");

                    b.ToTable("TeacherCards");
                });

            modelBuilder.Entity("DBlab2.Book", b =>
                {
                    b.HasOne("DBlab2.Publisher", "Publisher")
                        .WithMany("Books")
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Publisher");
                });

            modelBuilder.Entity("DBlab2.BookAuthor", b =>
                {
                    b.HasOne("DBlab2.Author", "Author")
                        .WithMany("BookAuthors")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DBlab2.Book", "Book")
                        .WithMany("BookAuthors")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Book");
                });

            modelBuilder.Entity("DBlab2.Cathedra", b =>
                {
                    b.HasOne("DBlab2.Faculty", "Faculty")
                        .WithMany("Cathedras")
                        .HasForeignKey("FacultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Faculty");
                });

            modelBuilder.Entity("DBlab2.Group", b =>
                {
                    b.HasOne("DBlab2.Cathedra", "Cathedra")
                        .WithMany("Groups")
                        .HasForeignKey("CathedraId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DBlab2.Speciality", "Speciality")
                        .WithMany("Groups")
                        .HasForeignKey("SpecialityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cathedra");

                    b.Navigation("Speciality");
                });

            modelBuilder.Entity("DBlab2.LibraryEmployee", b =>
                {
                    b.HasOne("DBlab2.Position", "Position")
                        .WithMany("LibraryEmployees")
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Position");
                });

            modelBuilder.Entity("DBlab2.Speciality", b =>
                {
                    b.HasOne("DBlab2.Faculty", "Faculty")
                        .WithMany("Specialities")
                        .HasForeignKey("FacultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Faculty");
                });

            modelBuilder.Entity("DBlab2.Student", b =>
                {
                    b.HasOne("DBlab2.Group", "Group")
                        .WithMany("Students")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");
                });

            modelBuilder.Entity("DBlab2.StudentCard", b =>
                {
                    b.HasOne("DBlab2.Book", "Book")
                        .WithMany("StudentCards")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DBlab2.LibraryEmployee", "LibraryEmployee")
                        .WithMany("StudentCards")
                        .HasForeignKey("LibraryEmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DBlab2.Student", "Student")
                        .WithMany("StudentCards")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("LibraryEmployee");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("DBlab2.Teacher", b =>
                {
                    b.HasOne("DBlab2.Cathedra", "Cathedra")
                        .WithMany("Teachers")
                        .HasForeignKey("CathedraId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cathedra");
                });

            modelBuilder.Entity("DBlab2.TeacherCard", b =>
                {
                    b.HasOne("DBlab2.Book", "Book")
                        .WithMany("TeacherCards")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DBlab2.LibraryEmployee", "LibraryEmployee")
                        .WithMany("TeacherCards")
                        .HasForeignKey("LibraryEmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DBlab2.Teacher", "Teacher")
                        .WithMany("TeacherCards")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("LibraryEmployee");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("DBlab2.Author", b =>
                {
                    b.Navigation("BookAuthors");
                });

            modelBuilder.Entity("DBlab2.Book", b =>
                {
                    b.Navigation("BookAuthors");

                    b.Navigation("StudentCards");

                    b.Navigation("TeacherCards");
                });

            modelBuilder.Entity("DBlab2.Cathedra", b =>
                {
                    b.Navigation("Groups");

                    b.Navigation("Teachers");
                });

            modelBuilder.Entity("DBlab2.Faculty", b =>
                {
                    b.Navigation("Cathedras");

                    b.Navigation("Specialities");
                });

            modelBuilder.Entity("DBlab2.Group", b =>
                {
                    b.Navigation("Students");
                });

            modelBuilder.Entity("DBlab2.LibraryEmployee", b =>
                {
                    b.Navigation("StudentCards");

                    b.Navigation("TeacherCards");
                });

            modelBuilder.Entity("DBlab2.Position", b =>
                {
                    b.Navigation("LibraryEmployees");
                });

            modelBuilder.Entity("DBlab2.Publisher", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("DBlab2.Speciality", b =>
                {
                    b.Navigation("Groups");
                });

            modelBuilder.Entity("DBlab2.Student", b =>
                {
                    b.Navigation("StudentCards");
                });

            modelBuilder.Entity("DBlab2.Teacher", b =>
                {
                    b.Navigation("TeacherCards");
                });
#pragma warning restore 612, 618
        }
    }
}
