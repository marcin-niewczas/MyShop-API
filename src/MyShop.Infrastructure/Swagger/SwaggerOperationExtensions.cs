using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using MyShop.Application.EndpointQueries.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.Abstractions;
using MyShop.Core.RepositoryQueryParams.Commons;
using MyShop.Core.Utils;

namespace MyShop.Infrastructure.Swagger;
public static class SwaggerOperationExtensions
{
    public static OpenApiOperation SetPaginationParameters(
        this OpenApiOperation operation,
        string pageNumberParameterName = nameof(IPaginationQueryParams.PageNumber),
        string pageSizeParameterName = nameof(IPaginationQueryParams.PageSize),
        bool isRequired = true
        )
    {
        operation.SetPageNumberParameter(pageNumberParameterName, isRequired);
        operation.SetPageSizeParameter(pageSizeParameterName, isRequired);

        return operation;
    }

    public static OpenApiOperation SetPageNumberParameter(
        this OpenApiOperation operation,
        string parameterName = nameof(IPaginationQueryParams.PageNumber),
        bool isRequired = true
        )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(parameterName, nameof(parameterName));

        var parameter = operation.Parameters.Single(p => p.Name.Equals(parameterName));
        parameter.SetAsRequired(isRequired);
        parameter.TransformNameToCamelCase();

        parameter.Schema = new OpenApiSchema()
        {
            Type = "integer",
            Format = "int32",
            Minimum = 1,
            Example = new OpenApiInteger(1)
        };

        return operation;
    }

    public static OpenApiOperation SetPageSizeParameter(
        this OpenApiOperation operation,
        string parameterName = nameof(IPaginationQueryParams.PageSize),
        bool isRequired = true
        )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(parameterName, nameof(parameterName));

        var parameter = operation.Parameters.Single(p => p.Name.Equals(parameterName));
        parameter.SetAsRequired(isRequired);
        parameter.TransformNameToCamelCase();

        parameter.Schema = new OpenApiSchema()
        {
            Type = "integer",
            Format = "int32",
            Minimum = CustomValidators.PaginationParams.PageSize.Min,
            Maximum = CustomValidators.PaginationParams.PageSize.Max,
            Example = new OpenApiInteger(10)
        };

        return operation;
    }

    public static OpenApiOperation SetSortParameters<TSortByEnum>(
        this OpenApiOperation operation,
        string sortByParameterName = nameof(ISortQueryParams.SortBy),
        string sortDirectionParameterName = nameof(ISortQueryParams.SortDirection),
        bool isRequired = false
        ) where TSortByEnum : struct, Enum
    {
        operation.SetSortDirectionParameter(sortDirectionParameterName, isRequired);
        operation.SetSortByParameter<TSortByEnum>(sortByParameterName, isRequired);

        return operation;
    }

    public static OpenApiOperation SetSortByParameter<TSortByEnum>(
        this OpenApiOperation operation,
        string parameterName = nameof(ISortQueryParams.SortBy),
        bool isRequired = false
        ) where TSortByEnum : struct, Enum
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(parameterName, nameof(parameterName));

        var parameter = operation.Parameters.Single(p => p.Name.Equals(parameterName));
        parameter.SetAsRequired(isRequired);
        parameter.TransformNameToCamelCase();

        parameter.Schema = new OpenApiSchema()
        {
            Enum = Enum.GetNames<TSortByEnum>().Select(key => new OpenApiString(key)).Cast<IOpenApiAny>().ToList(),
            Type = "string"
        };

        return operation;
    }

    public static OpenApiOperation SetSortDirectionParameter(
        this OpenApiOperation operation,
        string parameterName = nameof(ISortQueryParams.SortDirection),
        bool isRequired = false
        )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(parameterName, nameof(parameterName));

        var parameter = operation.Parameters.Single(p => p.Name.Equals(parameterName));
        parameter.SetAsRequired(isRequired);
        parameter.TransformNameToCamelCase();

        parameter.InitSchema()
                 .SetEnumValues<SortDirection>();

        return operation;
    }

    public static OpenApiOperation SetSearchPhraseParameter(
        this OpenApiOperation operation,
        string parameterName = nameof(ISearchQueryParams.SearchPhrase),
        bool isRequired = false
        )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(parameterName, nameof(parameterName));

        var parameter = operation.Parameters.Single(p => p.Name.Equals(parameterName));
        parameter.SetAsRequired(isRequired);
        parameter.TransformNameToCamelCase();
        parameter.InitSchema()
                .SetType("string");

        return operation;
    }

    public static OpenApiParameter ForParameter(
        this OpenApiOperation operation,
        string parameterName
        )
    {
        ArgumentNullException.ThrowIfNull(operation, nameof(operation));
        ArgumentException.ThrowIfNullOrWhiteSpace(parameterName, nameof(parameterName));

        return operation.Parameters.Single(p => p.Name.Equals(parameterName)); ;
    }

    public static OpenApiParameter TransformNameToCamelCase(
        this OpenApiParameter parameter
        )
    {
        ArgumentNullException.ThrowIfNull(parameter, nameof(parameter));

        parameter.Name = parameter.Name.ToCamelCase();

        return parameter;
    }

    public static OpenApiParameter SetAsRequired(
        this OpenApiParameter operation,
        bool value = true
        )
    {
        ArgumentNullException.ThrowIfNull(operation, nameof(operation));

        operation.Required = value;
        return operation;
    }

    public static void SetSchema(
        this OpenApiParameter parameter,
        OpenApiSchema schema
        )
    {
        ArgumentNullException.ThrowIfNull(parameter, nameof(parameter));
        ArgumentNullException.ThrowIfNull(schema, nameof(schema));

        parameter.Schema = schema;
    }

    public static OpenApiSchema InitSchema(
        this OpenApiParameter parameter
        )
    {
        ArgumentNullException.ThrowIfNull(parameter, nameof(parameter));

        parameter.Schema = new();
        return parameter.Schema;
    }

    public static OpenApiSchema SetType(
        this OpenApiSchema schema,
        string value
        )
    {
        ArgumentNullException.ThrowIfNull(schema, nameof(schema));
        ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

        schema.Type = value;
        return schema;
    }

    public static OpenApiSchema SetFormat(
        this OpenApiSchema schema,
        string value
        )
    {
        ArgumentNullException.ThrowIfNull(schema, nameof(schema));
        ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

        schema.Format = value;
        return schema;
    }

    public static OpenApiParameter SetDescription(
        this OpenApiParameter parameter,
        string description
        )
    {
        ArgumentNullException.ThrowIfNull(parameter, nameof(parameter));
        ArgumentException.ThrowIfNullOrWhiteSpace(description, nameof(description));

        parameter.Description = description;

        return parameter;
    }

    public static OpenApiSchema SetMinimum(
        this OpenApiSchema schema,
        decimal? value
        )
    {
        ArgumentNullException.ThrowIfNull(schema, nameof(schema));

        schema.Minimum = value;
        return schema;
    }

    public static OpenApiSchema SetMaximum(
        this OpenApiSchema schema,
        decimal? value
        )
    {
        ArgumentNullException.ThrowIfNull(schema, nameof(schema));

        schema.Maximum = value;
        return schema;
    }

    public static OpenApiSchema SetEnumValues<TEnum>(
        this OpenApiSchema schema
        ) where TEnum : struct, Enum
    {
        ArgumentNullException.ThrowIfNull(schema, nameof(schema));

        schema.Enum = Enum.GetNames<TEnum>().Select(v => new OpenApiString(v)).Cast<IOpenApiAny>().ToList();
        schema.Type = "string";

        return schema;
    }

    public static OpenApiSchema SetAllowedValues<TAllowedValues>(
        this OpenApiSchema schema
        ) where TAllowedValues : class, IAllowedValues
    {
        ArgumentNullException.ThrowIfNull(schema, nameof(schema));

        schema.Enum = TAllowedValues.AllowedValues.Select(v => new OpenApiString(v.ToString())).Cast<IOpenApiAny>().ToList();
        schema.Type = "string";

        return schema;
    }

    public static OpenApiSchema SetAllowedValues(
        this OpenApiSchema schema,
        IEnumerable<int> allowedValues
        )
    {
        ArgumentNullException.ThrowIfNull(schema, nameof(schema));
        schema.Enum = allowedValues.Select(v => new OpenApiInteger(v)).Cast<IOpenApiAny>().ToList();
        schema.Type = "integer";
        schema.Format = "int32";

        return schema;
    }

    public static OpenApiOperation SetTimestampParameters(
        this OpenApiOperation operation,
        bool isRequired = false,
        string fromDateParamName = nameof(ITimestampQueryParams.FromDate),
        string toDateParamName = nameof(ITimestampQueryParams.ToDate),
        string inclusiveFromDateParamName = nameof(ITimestampQueryParams.InclusiveFromDate),
        string inclusiveToDateParamName = nameof(ITimestampQueryParams.InclusiveToDate)
        )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fromDateParamName, nameof(fromDateParamName));
        ArgumentException.ThrowIfNullOrWhiteSpace(toDateParamName, nameof(toDateParamName));
        ArgumentException.ThrowIfNullOrWhiteSpace(inclusiveFromDateParamName, nameof(inclusiveFromDateParamName));
        ArgumentException.ThrowIfNullOrWhiteSpace(inclusiveToDateParamName, nameof(inclusiveToDateParamName));

        var now = DateTimeOffset.UtcNow;

        var fromDate = operation.Parameters.Single(p => p.Name == fromDateParamName);
        fromDate.Name = fromDateParamName.ToCamelCase();
        fromDate.Description = $"Example: {new DateTimeOffset(now.Year, now.Month, 1, 0, 0, 0, now.Offset).AddMonths(-2):O}";
        fromDate.Required = isRequired;

        var toDate = operation.Parameters.Single(p => p.Name == toDateParamName);
        toDate.Name = toDateParamName.ToCamelCase();
        toDate.Description = $"Example: {new DateTimeOffset(now.Year, now.Month, 1, 0, 0, 0, now.Offset):O}";
        toDate.Required = isRequired;

        var inclusiveFromDate = operation.Parameters.Single(p => p.Name == inclusiveFromDateParamName);
        inclusiveFromDate.Name = inclusiveFromDateParamName.ToCamelCase();
        inclusiveFromDate.Required = isRequired;
        inclusiveFromDate.Schema = new OpenApiSchema()
        {
            Type = "boolean"
        };

        var inclusiveToDate = operation.Parameters.Single(p => p.Name == inclusiveToDateParamName);
        inclusiveToDate.Name = inclusiveToDateParamName.ToCamelCase();
        inclusiveToDate.Required = isRequired;
        inclusiveToDate.Schema = new OpenApiSchema()
        {
            Type = "boolean"
        };

        return operation;
    }
}
