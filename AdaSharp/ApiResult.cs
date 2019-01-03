using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AdaSharp
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Status
    {
        [EnumMember(Value = "success")]
        Success,
        [EnumMember(Value = "fail")]
        Fail,
        [EnumMember(Value = "error")]
        Error
    }

    public class ApiResult
    {
        public Status Status { get; set; }
        public Meta Meta { get; set; }
        public string Message { get; set; }
    }

    public class ApiResult<T> : ApiResult
    {
        public T Data { get; set; }
    }

    public class Meta
    {
        public Pagination Pagination { get; set; }
    }

    public class Pagination
    {
        public ulong TotalPages { get; set; }
        public ulong Page { get; set; }
        public ushort PerPage { get; set; }
        public ulong TotalEntries { get; set; }
    }
}
