using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StudentWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentWebApi.Data
{
    public class StudentContext : IStudentContext
    {
        private readonly IMongoDatabase _database;

        public StudentContext(IOptions<Settings> settings, IMongoClient client)
        {
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Student> Students => _database.GetCollection<Student>("Students");
    }
}
