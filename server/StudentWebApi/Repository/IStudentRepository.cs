using StudentWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentWebApi.Repository
{
    public interface IStudentRepository
    {
        Task<StudentApiResponse> GetAllStudents(string sortBy, int page, int pageSize);
        Task<Student> GetStudent(string id);
        Task Create(Student student);
        Task<bool> Update(Student student);
        Task<bool> Delete(string id);
        Task<long> DeleteAllData();
        Task<long> GetStudentCount();
    }
}
