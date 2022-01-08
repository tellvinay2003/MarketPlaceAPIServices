using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MarketPlaceService.API.Utilities
{
    public static class ValidatorExtensions
    {
        public static bool ValidateEmptyGuid(this Guid guid)
        {
            return guid == Guid.Empty;
        }

        public static bool ValidateInteger(this int i)
        {
            return i < 1;
        }

        public static bool ValidateObjectForNull(this object o)
        {
            return o==null;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> data)
        {
            return data == null ||  data.Count() == 0;
        }

        public static bool IsUniqueConstraintViolated(this Exception ex)
        {
           if (ex is DbUpdateException dbUpdateEx)
            {
                if (dbUpdateEx.InnerException != null)
                {
                    if (dbUpdateEx.InnerException is SqlException sqlException)
                    {
                        switch (sqlException.Number)
                        {
                        case 2627:  // Unique constraint error
                        case 547:   // Constraint check violation
                        case 2601:  // Duplicated key row error
                                    // Constraint violation exception
                            // A custom exception of yours for concurrency issues
                            return true;
                        default:
                            // A custom exception of yours for other DB issues
                            return false;
                        }
                    }
                }    
            }
            return false;
        }
    }
}
