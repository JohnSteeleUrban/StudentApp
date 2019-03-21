using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StudentWebApi.Data;
using StudentWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentWebApi.Services
{
    public class StudentService
    {
        private readonly IMongoCollection<Student> _students;

        public StudentService(IOptions<Settings> settings)
        {
            _students = (IMongoCollection<Student>)new StudentContext(settings);
        }

        public async Task<List<Student>> Get()
        {
            return await _students.Find(student => true).ToListAsync();
        }

        public async Task<Student> Get(string id)
        {
            return await _students.Find(student => student.Id == id).FirstOrDefaultAsync();
        }

        public async Task Create(Student student)
        {
          
            await _students.InsertOneAsync(student);
            
        }

        public async Task<bool> Update(string id, Student student)
        {
            ReplaceOneResult updateResult = await _students.ReplaceOneAsync(s => s.Id == id, student);
            return updateResult.IsAcknowledged
                && updateResult.ModifiedCount > 0;
        }

        public async Task Remove(Student student)
        {
            await _students.DeleteOneAsync(s => s.Id == student.Id);
        }

        public async Task<bool> Remove(string id)
        {
            DeleteResult deleteResult = await _students.DeleteOneAsync(student => student.Id == id);
            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }
    }
}
