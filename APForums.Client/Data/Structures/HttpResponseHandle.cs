using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data.Structures
{

    public abstract class HttpResponseHandle<T>
    {
#nullable enable
        public string? Error;
#nullable disable

        public T Status;
    }


    public abstract class HttpResponseHandleWithData<T, D> : HttpResponseHandle<T>
    {
        public D Data { get; set; }
    }
}
