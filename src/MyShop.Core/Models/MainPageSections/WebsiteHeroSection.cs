using MyShop.Core.Exceptions;
using MyShop.Core.HelperModels;
using MyShop.Core.Models.BaseEntities;
using MyShop.Core.Models.Photos;
using MyShop.Core.Utils;
using MyShop.Core.ValueObjects.MainPageSections;

namespace MyShop.Core.Models.MainPageSections;
public sealed class WebsiteHeroSection : MainPageSection
{
    public WebsiteHeroSectionLabel Label { get; private set; } = default!;
    public WebsiteHeroSectionDisplayType DisplayType { get; private set; } = default!;
    public IReadOnlyCollection<WebsiteHeroSectionPhoto> Photos { get; private set; } = default!;
    public IReadOnlyCollection<WebsiteHeroSectionItem> WebsiteHeroSectionItems => _websiteHeroSectionItems;
    private readonly List<WebsiteHeroSectionItem> _websiteHeroSectionItems = default!;

    public WebsiteHeroSection(
        WebsiteHeroSectionLabel label,
        WebsiteHeroSectionDisplayType displayType,
        MainPageSectionPosition position
        ) : base(
            MainPageSectionType.WebsiteHeroSection,
            position
            )
    {
        ArgumentNullException.ThrowIfNull(nameof(label));
        ArgumentNullException.ThrowIfNull(nameof(displayType));

        Label = label;
        DisplayType = displayType;
    }

    private WebsiteHeroSection() { }

    public void Update(WebsiteHeroSectionLabel label)
    {
        if (Label == label)
        {
            throw new BadRequestException($"Nothing change in {nameof(WebsiteHeroSection)}.");
        }

        Label = label;
    }

    public WebsiteHeroSectionItem AddItem(
        WebsiteHeroSectionItem item
        )
    {
        if (item.Position is null)
        {
            _websiteHeroSectionItems.Add(item);
            return item;
        }

        if (_websiteHeroSectionItems is null)
        {
            throw new InvalidOperationException($"{nameof(WebsiteHeroSectionItems)} must be included.");
        }

        var maxCount = WebsiteHeroSectionItemPosition.Max + 1;

        if (_websiteHeroSectionItems.Count >= maxCount)
        {
            throw new BadRequestException($"The {nameof(WebsiteHeroSection)} can contains max. {maxCount} activated items.");
        }

        if (item.Position > _websiteHeroSectionItems.Count)
        {
            throw new BadRequestException(
                _websiteHeroSectionItems.Count switch
                {
                    > 0 => $"The {nameof(Position)} must be between 0 and {_websiteHeroSectionItems.Count}.",
                    _ => $"The {nameof(Position)} must equals to 0."
                }
                );
        }

        foreach (var existItem in _websiteHeroSectionItems.Where(i => i.Position != null && i.Position >= item.Position))
        {
            existItem.UpdatePosition(existItem.Position + 1);
        }

        _websiteHeroSectionItems.Add(item);

        return item;
    }

    public WebsiteHeroSectionItem RemoveItem(
       Guid id
       )
    {
        var item = _websiteHeroSectionItems.FirstOrDefault(e => e.Id == id)
            ?? throw new NotFoundException(
                $"The {nameof(WebsiteHeroSectionItem)} with '{id}' {nameof(IEntity.Id)} not belong to {nameof(WebsiteHeroSection)} '{Id}'."
                );


        if (item.Position is null)
        {
            _websiteHeroSectionItems.Remove(item);
            return item;
        }

        if (_websiteHeroSectionItems is null)
        {
            throw new InvalidOperationException($"{nameof(WebsiteHeroSectionItems)} must be included.");
        }

        foreach (var existItem in _websiteHeroSectionItems.Where(i => i.Position != null && i.Position > item.Position))
        {
            existItem.UpdatePosition(existItem.Position - 1);
        }

        _websiteHeroSectionItems.Remove(item);

        return item;
    }

    public WebsiteHeroSectionItem ActivateItem(Guid id)
    {
        if (_websiteHeroSectionItems is null)
        {
            throw new InvalidOperationException($"{nameof(WebsiteHeroSectionItems)} must be included.");
        }

        var item = _websiteHeroSectionItems.FirstOrDefault(i => i.Id == id)
            ?? throw new ArgumentNullException(nameof(id));

        if (item.Position is not null)
        {
            throw new ArgumentException($"The {nameof(WebsiteHeroSectionItem)} with '{item.Id}' {nameof(IEntity.Id)} is active.");
        }

        if (item.WebsiteHeroSectionId != Id)
        {
            throw new ArgumentException($"The {nameof(WebsiteHeroSectionItem)} with '{item.Id}' {nameof(IEntity.Id)} not belong to {nameof(WebsiteHeroSection)} '{Id}'.");
        }

        if (_websiteHeroSectionItems.Any(e => e.Id == item.Id && e.Position is not null))
        {
            throw new BadRequestException(
                $"The {nameof(WebsiteHeroSectionItem)} with '{item.Id}' {nameof(IEntity.Id)} is active."
                );
        }

        var maxCount = WebsiteHeroSectionItemPosition.Max + 1;

        if (_websiteHeroSectionItems.Count >= maxCount)
        {
            throw new BadRequestException($"The {nameof(WebsiteHeroSection)} can contains max. {maxCount} activated items.");
        }

        item.UpdatePosition(0);

        foreach (var existItem in _websiteHeroSectionItems.Where(i => i.Position != null && i.Id != item.Id))
        {
            existItem.UpdatePosition(existItem.Position + 1);
        }

        return item;
    }

    public WebsiteHeroSectionItem DeactivationItem(Guid id)
    {
        if (_websiteHeroSectionItems is null)
        {
            throw new InvalidOperationException($"{nameof(WebsiteHeroSectionItems)} must be included.");
        }

        var item = _websiteHeroSectionItems.FirstOrDefault(i => i.Id == id)
            ?? throw new ArgumentNullException(nameof(id));

        if (item.Position is null)
        {
            throw new ArgumentException($"The {nameof(WebsiteHeroSectionItem)} with '{item.Id}' {nameof(IEntity.Id)} isn't active.");
        }

        if (item.WebsiteHeroSectionId != Id)
        {
            throw new ArgumentException($"The {nameof(WebsiteHeroSectionItem)} with '{item.Id}' {nameof(IEntity.Id)} not belong to {nameof(WebsiteHeroSection)} '{Id}'.");
        }

        if (_websiteHeroSectionItems.Any(e => e.Id == item.Id && e.Position is null))
        {
            throw new BadRequestException(
                $"The {nameof(WebsiteHeroSectionItem)} with '{item.Id}' {nameof(IEntity.Id)} is active."
                );
        }

        var oldPosition = item.Position;

        item.UpdatePosition(new(null));

        foreach (var existItem in _websiteHeroSectionItems.Where(i => i.Position != null && i.Position > oldPosition))
        {
            existItem.UpdatePosition(existItem.Position - 1);
        }

        return item;
    }

    public void UpdatePositionsOfWebsiteHeroSectionItems(
        IReadOnlyCollection<ValuePosition<Guid>> idPositions
        )
    {
        var minPosition = WebsiteHeroSectionItemPosition.Min;
        var maxPositions = _websiteHeroSectionItems.Count - 1;

        if (idPositions.Any(x => x.Position < minPosition || x.Position > maxPositions))
        {
            throw new BadRequestException(
                $"The {nameof(WebsiteHeroSectionItem.Position)} for {nameof(WebsiteHeroSectionItem)} '{Id}' must be inclusive between {minPosition} and {maxPositions}."
            );
        }

        foreach (var idPosition in idPositions)
        {
            (_websiteHeroSectionItems
                .FirstOrDefault(x => x.Id == idPosition.Value)
                ?? throw new NotFoundException($"Not found {nameof(WebsiteHeroSectionItem)} '{idPosition.Value}' in {nameof(WebsiteHeroSection)} '{Id}'."))
            .UpdatePosition(idPosition.Position);
        }

        var allowedPositions = Enumerable.Range(WebsiteHeroSectionItemPosition.Min, _websiteHeroSectionItems.Count).OfType<int?>();

        if (_websiteHeroSectionItems.HasDuplicateBy(v => v.Position) || _websiteHeroSectionItems.Any(v => !allowedPositions.Contains(v.Position)))
        {
            throw new BadRequestException($"Invalid positions. Positions must be unique and not contains gaps.");
        }
    }
}
