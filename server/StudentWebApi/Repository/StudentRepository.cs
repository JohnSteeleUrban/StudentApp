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

        public async Task<StudentApiResponse> GetAllStudents(string sortBy, SortOrder order, int page, int pageSize)
        {
            var students = await _context.Students.Find(FilterDefinition<Student>.Empty)
                                .Sort(string.Format( "{{{0}: {1}}}", StudentHelper.FortmatSortBy(sortBy), StudentHelper.GetSortOrder(order)))
                                .Skip((page - 1) * pageSize)
                                .Limit(pageSize)
                                .ToListAsync(); 
            return new StudentApiResponse { Items = students, TotalCount = await GetStudentCount() };
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