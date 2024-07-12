using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Application.EndpointQueries.Interfaces;
public interface IPaginationQueryParams
{
    int PageNumber { get; }
    int PageSize { get; }
}
