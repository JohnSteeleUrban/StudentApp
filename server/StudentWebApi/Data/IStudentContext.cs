using MongoDB.Driver;
using StudentWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentWebApi.Data
{
    public interface IStudentContext
    {
        IMongoCollection<Student> Students { get; }
    }
}
