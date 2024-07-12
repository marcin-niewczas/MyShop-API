using MyShop.Core.Exceptions;
using MyShop.Core.Models.BaseEntities;
using MyShop.Core.ValueObjects.MainPageSections;

namespace MyShop.Core.Models.MainPageSections;
public abstract class MainPageSection : BaseTimestampEntity
{
    public MainPageSectionType MainPageSectionType { get; private set; } = default!;
    public MainPageSectionPosition Position { get; private set; } = default!;

    protected MainPageSection(
        MainPageSectionType mainPageSectionType,
        MainPageSectionPosition position
        )
    {
        MainPageSectionType = mainPageSectionType;
        Position = position;
    }

    protected MainPageSection() { }

    public void UpdatePosition(MainPageSectionPosition position)
    {
        ArgumentNullException.ThrowIfNull(nameof(position));

        if (Position == position)
        {
            throw new BadRequestException($"The {nameof(Position)} is same for {nameof(MainPageSection)} '{Id}'.");
        }

        Position = position;
    }
}
