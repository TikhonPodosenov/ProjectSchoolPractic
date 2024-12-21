using System.Collections.Generic;
using ProjectSchool.Models; 

namespace ProjectSchool.DB 
{
    public interface ISqlQueryExecutor
    {
        List<T> ExecuteQuery<T>(string sql, params object[] parameters);
    }
}