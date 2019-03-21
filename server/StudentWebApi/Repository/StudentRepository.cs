using MongoDB.Bson;
using MongoDB.Driver;
using StudentWebApi.Data;
using StudentWebApi.Helpers;
using StudentWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentWebApi.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly IStudentContext _context;

        public StudentRepository(IStudentContext context)
        {
            _context = context;
        }

        public async Task<StudentApiResponse> GetAllStudents(string sort, int page, int pageSize)
        {
            var students = await _context.Students.Find(_ => true).ToListAsync();
            return new StudentApiResponse { Items = StudentHelper.ApplySort(StudentHelper.ApplyPaging(students, page, pageSize), sort).ToList(), TotalCount = students.Count() };
        }

        public Task<Student> GetStudent(string id)
        {
            FilterDefinition<Student> filter = Builders<Student>.Filter.Eq(m => m.Id, id);

            return _context
                    .Students
                    .Find(filter)
                    .FirstOrDefaultAsync();
        }

        public async Task Create(Student student)
        {
            await _context.Students.InsertOneAsync(student);
        }

        public async Task<bool> Update(Student student)
        {
            ReplaceOneResult updateResult =
                await _context
                        .Students
                        .ReplaceOneAsync(
                            filter: g => g.Id == student.Id,
                            replacement: student);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Student> filter = Builders<Student>.Filter.Eq(m => m.Id, id);

            DeleteResult deleteResult = await _context
                                                .Students
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<long> DeleteAllData()
        {
            DeleteResult deletedResult = await _context
                                                .Students
                                                .DeleteManyAsync(new BsonDocument());

            return  deletedResult.DeletedCount;
        }

        public async Task<long> GetStudentCount()
        {   
            return await _context
                            .Students
                            .CountDocumentsAsync(new BsonDocument());            
        }
    }
}