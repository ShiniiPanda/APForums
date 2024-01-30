using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace APForums.Client.Data.Structures
{
    public class BasicHttpResponse : HttpResponseHandle<HttpStatusCode>
    {


    }

    public class BasicHttpResponseWithData<D> : HttpResponseHandleWithData<HttpStatusCode, D>
    {

    }
}
