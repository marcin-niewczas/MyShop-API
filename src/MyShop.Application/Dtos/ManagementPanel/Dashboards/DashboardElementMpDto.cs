using MyShop.Core.Abstractions;
using MyShop.Core.Exceptions;
using MyShop.Core.ValueObjects.Orders;
using System.Text.Json.Serialization;

namespace MyShop.Application.Dtos.ManagementPanel.Dashboards;
public enum DashboardElement
{
    OneValue = 0,
    Chart,
    Group
}

public enum DashboardDataUnit
{
    None = 0,
    Date,
    Currency
}

public enum DashboardElementSize
{
    Small = 0,
    Medium,
    Large
}

public enum GroupValuesDashboardElementType
{
    Statistics = 0,
    Ranks
}

public sealed record DashboardChartType : IAllowedValues
{
    public static IReadOnlyCollection<object> AllowedValues => [
        Pie,
        Line,
        Bar
    ];

    public const string Pie = "pie";
    public const string Line = "line";
    public const string Bar = "bar";

    public string Value { get; }

    public DashboardChartType(string value)
    {
        if (!AllowedValues.Contains(value))
            throw new ArgumentException(AllowedValuesError.Message<DeliveryMethod>());

        Value = value;
    }

    public static implicit operator string(DashboardChartType value)
        => value.Value;

    public static implicit operator DashboardChartType(string value)
        => new(value);

    public override string ToString()
        => Value.ToString();
}

[JsonDerivedType(typeof(OneValueDashboardElementMpDto))]
[JsonDerivedType(typeof(ChartDashboardElementMpDto))]
[JsonDerivedType(typeof(GroupValuesDashboardElementMpDto))]
public abstract record BaseDashboardElementMpDto : IDto
{
    public string? Title { get; init; }
    public string? Icon { get; init; }
    public DashboardElement DashboardElement { get; init; }
    public DashboardElementSize DashboardElementSize { get; init; }
    public DashboardDataUnit DashboardDataUnit { get; init; }

    protected BaseDashboardElementMpDto(
        DashboardDataUnit dashboardDataUnit,
        string? title,
        string? icon,
        DashboardElementSize dashboardElementSize
        )
    {
        DashboardDataUnit = dashboardDataUnit;
        Title = title;
        Icon = icon;
        DashboardElementSize = dashboardElementSize;
    }
}

public sealed record OneValueDashboardElementMpDto : BaseDashboardElementMpDto
{
    public object Value { get; init; }
    public RouterInfo? RouterInfo { get; init; }

    public OneValueDashboardElementMpDto(
        object value,
        DashboardDataUnit dashboardDataUnit = DashboardDataUnit.None,
        string? title = null,
        string? icon = null,
        DashboardElementSize dashboardElementSize = DashboardElementSize.Small,
        RouterInfo? routerInfo = null
        ) : base(
            dashboardDataUnit,
            title,
            icon,
            dashboardElementSize
            )
    {
        DashboardElement = DashboardElement.OneValue;
        Value = value;
        DashboardDataUnit = dashboardDataUnit;
        DashboardElementSize = dashboardElementSize;

        if (routerInfo is not null)
        {
            RouterInfo = routerInfo;
        }
    }
}

public sealed record RouterInfo(string ResourceType, string ResourceId);

public sealed record GroupValuesDashboardElementMpDto : BaseDashboardElementMpDto
{
    public IEnumerable<OneValueDashboardElementMpDto> Values { get; init; }
    public GroupValuesDashboardElementType GroupValuesDashboardElementType { get; init; }

    public GroupValuesDashboardElementMpDto(
        IEnumerable<OneValueDashboardElementMpDto> values,
        GroupValuesDashboardElementType groupValuesDashboardElementType,
        DashboardDataUnit dashboardDataUnit = DashboardDataUnit.None,
        string? title = null,
        string? icon = null,
        DashboardElementSize dashboardElementSize = DashboardElementSize.Medium
        ) : base(
            dashboardDataUnit,
            title,
            icon,
            dashboardElementSize
            )
    {
        DashboardElement = DashboardElement.Group;
        Values = values;
        DashboardDataUnit = dashboardDataUnit;
        GroupValuesDashboardElementType = groupValuesDashboardElementType;
        DashboardElementSize = dashboardElementSize;
    }
}

public sealed record ChartDashboardElementMpDto : BaseDashboardElementMpDto
{
    public string DashboardChartType { get; init; }
    public bool ShowDataLabels { get; init; }
    public DashboardDataUnit DashboardLabelUnit { get; init; }
    public IReadOnlyCollection<object> Labels { get; init; }
    public IReadOnlyCollection<object> Data { get; init; }
    public IReadOnlyCollection<string> BackgroundColors { get; init; } = default!;

    public ChartDashboardElementMpDto(
        DashboardChartType dashboardChartType,
        IEnumerable<object> labels,
        IEnumerable<object> data,
        bool withBackgroundColor = false,
        DashboardDataUnit dashboardLabelUnit = DashboardDataUnit.None,
        DashboardDataUnit dashboardDataUnit = DashboardDataUnit.None,
        string? title = null,
        string? icon = null,
        DashboardElementSize dashboardElementSize = DashboardElementSize.Medium,
        bool showDataLabels = false
        ) : base(
            dashboardDataUnit,
            title,
            icon,
            dashboardElementSize
            )
    {
        DashboardElement = DashboardElement.Chart;
        DashboardChartType = dashboardChartType;
        Labels = labels.ToList();
        Data = data.ToList();
        DashboardLabelUnit = dashboardLabelUnit;
        DashboardDataUnit = dashboardDataUnit;
        DashboardElementSize = dashboardElementSize;
        ShowDataLabels = showDataLabels;

        if (withBackgroundColor)
        {
            var listOfColors = new List<string>();

            var random = new Random();

            for (var i = 0; i < Labels.Count; i++)
            {
                listOfColors.Add($"rgb({random.Next(256)},{random.Next(256)},{random.Next(256)})");
            }

            BackgroundColors = listOfColors.AsReadOnly();
        }
    }
}