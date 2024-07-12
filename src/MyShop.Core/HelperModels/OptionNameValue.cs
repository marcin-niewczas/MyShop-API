using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.HelperModels;
public record OptionNameValue(
    string Name,
    string Value
    );

public record OptionNameId(
    Guid Id,
    string Name
    );

public record OptionNameValueId(
    Guid Id,
    string Name,
    string Value
    );
