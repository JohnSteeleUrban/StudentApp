using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentWebApi.Models;

namespace StudentWebApi.Helpers
{
    public class StudentHelper
    {
        
        public static string FortmatSortBy(string sortBy)
        {
            sortBy = sortBy == null ? "" : sortBy.ToLower();
            switch (sortBy)
            {
                case "firstname":
                    return "FirstName";
                case "lastname":
                    return "LastName";
                case "email":
                    return "Email";
                case "lastupdated":
                    return "LastUpdated";
                default:
                    goto case "firstname";
            }
        }

        public static int GetSortOrder(SortOrder order)
        {
            switch (order)
            {
                case SortOrder.Asc:
                    return 1;
                case SortOrder.Desc:
                    return -1;
                default:
                    goto case SortOrder.Asc;
            }

        }
    }
}
