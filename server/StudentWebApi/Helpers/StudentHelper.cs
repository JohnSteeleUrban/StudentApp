using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentWebApi.Models;

namespace StudentWebApi.Helpers
{
    public class StudentHelper
    {
        public static IEnumerable<Student> ApplyPaging(List<Student> students, int page, int pageSize)
        {
            return students.Skip(page * pageSize).Take(pageSize);
        }

        public static IEnumerable<Student> ApplySort(IEnumerable<Student> students, string sort)
        {

            sort = sort == null ? "" : sort.ToLower();

            switch (sort)
            {
                case "firstname":
                    return students.OrderBy(x => x.FirstName);
                case "firstname desc":
                    return students.OrderByDescending(x => x.FirstName);
                case "lastname":
                    return students.OrderBy(x => x.LastName);
                case "lastname desc":
                    return students.OrderByDescending(x => x.LastName);
                case "email":
                    return students.OrderBy(x => x.Email);
                case "email desc":
                    return students.OrderByDescending(x => x.Email);
                case "lastupdated":
                    return students.OrderBy(x => x.LastUpdated);
                case "lastupdated desc":
                    return students.OrderByDescending(x => x.LastUpdated);
                default:
                    goto case "firstname";
            }
        }
    }
}
