using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationAuthorization
{
    public interface IUserAuthorization
    {
        string Operation { get; set; }
        Dictionary<string, string> AuthorizationParameters { get; set; }
    }
}
