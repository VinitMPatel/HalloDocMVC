using Services.ViewModels;

namespace Services.Interfaces
{
    public interface IStudentServices
    {
        Task AddNewStudent(StudentForm obj);
        Task DeleteStudent(int studentId);
        Task EditExistingStudent(StudentForm obj);
        Task<StudentForm> EditStudentFormData(StudentForm obj);
        Task<DataViewModel> GetStudentData(DataViewModel obj);
        Task<bool> ValidateCourse(string course);
    }
}