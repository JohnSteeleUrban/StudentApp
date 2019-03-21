using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentWebApi.Models
{
    public class StudentApiResponse
    {
        public IEnumerable<Student> Items { get; set; }
        public int TotalCount { get; set; }

    }
}
