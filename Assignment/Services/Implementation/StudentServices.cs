using Data.DataContext;
using Data.Entity;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class StudentServices : IStudentServices
    {
        private readonly AssignmentContext context;

        public StudentServices(AssignmentContext context)
        {
            this.context = context;
        }

        public async Task<DataViewModel> GetStudentData(DataViewModel obj)
        {
            DataViewModel dataObj = new DataViewModel();
            List<Student> students = await context.Students.Include(a => a.CourseNavigation).ToListAsync();
            if (!string.IsNullOrWhiteSpace(obj.searchKey))
            {
                students = students.Where(a => a.FirstName.ToLower().Contains(obj.searchKey.ToLower()) || a.LastName!.ToLower().Contains(obj.searchKey.ToLower())).ToList();
            }
            if (obj.requestedPage == 0)
            {
                obj.requestedPage = 1;
            }
            if (obj.totalEntity == 0)
            {
                obj.totalEntity = 3;
            }
            dataObj.Students = students;
            return dataObj;
        }

        //requestclients = requestclients.Where(a =>
        //                        (string.IsNullOrWhiteSpace(obj.name) || a.Firstname.ToLower().Contains(obj.name.ToLower()) || a.Lastname!.ToLower().Contains(obj.name.ToLower())) &&
        //                        (string.IsNullOrWhiteSpace(obj.email) || a.Email!.ToLower().Contains(obj.email.ToLower())) &&
        //                        (string.IsNullOrWhiteSpace(obj.phone) || a.Phonenumber!.Contains(obj.phone))).ToList();

        //dataObj.totalPages = (int) Math.Ceiling(requestclients.Count() / (double) obj.totalEntity);
        //dataObj.currentpage = obj.requestedPage;
        //    requestclients = requestclients.Skip((obj.requestedPage - 1) * obj.totalEntity).Take(obj.totalEntity).ToList();
        //dataObj.totalEntity = obj.totalEntity;


        public async Task DeleteStudent(int studentId)
        {
            if (studentId != 0)
            {
                Student? student = await context.Students.FirstOrDefaultAsync(a => a.Id == studentId);
                if (student != null)
                {
                    context.Students.Remove(student);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<StudentForm> EditStudentFormData(StudentForm obj)
        {
            if (obj == null)
            {
                return new StudentForm();
            }
            StudentForm dataObj = new StudentForm();
            Student? student = await context.Students.FirstOrDefaultAsync(a => a.Id == obj.studentId);
            if (student != null)
            {
                dataObj.flag = 0;
                dataObj.firstName = student.FirstName;
                dataObj.lastName = student.LastName;
                dataObj.email = student.Email;
                dataObj.course = student.Course;
                dataObj.studentId = student.Id;
                dataObj.gender = student.Gender;
            }
            return dataObj;
        }

        public async Task AddNewStudent(StudentForm obj)
        {
            if (obj != null)
            {
                Student student = new Student
                {
                    FirstName = obj.firstName,
                    LastName = obj.lastName,
                    Email = obj.email,
                    Course = obj.course,
                    Gender = obj.gender,
                    Grade = obj.grade,
                };
                await context.Students.AddAsync(student);
                await context.SaveChangesAsync();
            }
        }

        public async Task EditExistingStudent(StudentForm obj)
        {
            Student student = new Student();
            if(obj.studentId != 0)
            {
                student = await context.Students.FirstOrDefaultAsync(a=>a.Id == obj.studentId);
                student.FirstName = obj.firstName;
                student.LastName = obj.lastName;
                student.Gender = obj.gender;
                student.Course = obj.course;
                student.Email = obj.email;

                context.Update(student);
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> ValidateCourse(string course)
        {
            Course? existCourse = await context.Courses.FirstOrDefaultAsync(a => a.Name == course); 
            if(existCourse != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
