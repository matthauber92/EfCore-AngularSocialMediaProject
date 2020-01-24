using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Helpers
{
    public class Result
    {
        public Result()
        {
        }

        public Result(Result result)
        {
            this.Exception = result.Exception;
            this.EndTime = result.EndTime;
            this.Page = result.Page;
            this.ResultsPerPage = result.ResultsPerPage;
            this.StartTime = result.StartTime;
            this.TotalResults = result.TotalResults;
            this.SortProperty = result.SortProperty;
            this.IsAscending = result.IsAscending;
        }

        public Exception Exception { get; set; }
        public int? Page { get; set; }
        public int? ResultsPerPage { get; set; }
        public int? TotalResults { get; set; }
        public int? TotalPages
        {

            get
            {
                if (ResultsPerPage.HasValue && TotalResults.HasValue)
                    return (TotalResults.Value % ResultsPerPage.Value) == 0 ?
                        TotalResults.Value / ResultsPerPage.Value :
                        (TotalResults.Value / ResultsPerPage.Value) + 1;
                return null;
            }
        }
        public bool HasMore
        {
            get
            {
                if (TotalPages.HasValue && Page.HasValue)
                    return Page.Value < TotalPages;
                return false;
            }
        }
        public string SortProperty { get; set; }
        public bool IsAscending { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool HasError
        {
            get
            {
                if (Exception != null)
                    return true;

                return false;
            }
        }
    }

    [Serializable]
    public class Result<T> : Result
    {
        public Result()
            : base()
        {
        }

        public bool HasValue
        {
            get
            {
                if (HasError)
                    return false;

                if (Value != null && Value is string)
                {
                    string stringvalue = (string)(object)Value;
                    stringvalue.Trim();
                    return stringvalue.Length > 0;
                }
                else if (Value is ICollection)
                {
                    return ((ICollection)Value).Count > 0;
                }
                else if (typeof(T).IsArray)
                {
                    Array a = (Array)(object)Value;
                    return a.Length > 0;
                }

                return Value != null;
            }
        }

        public Result(Result result)
            : base(result)
        {
        }

        public T Value { get; set; }
    }
}
