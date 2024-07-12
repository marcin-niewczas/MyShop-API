using MyShop.Core.Abstractions.Repositories;
using MyShop.Core.Exceptions;
using MyShop.Core.Models.BaseEntities;
using MyShop.Core.Utils;
using MyShop.Core.ValueObjects.Categories;

namespace MyShop.Core.Models.Products;
public sealed class Category : BaseTimestampEntity
{
    public CategoryName Name { get; private set; } = default!;
    public HierarchyDetail HierarchyDetail { get; private set; } = default!;
    public Category? ParentCategory { get; private set; }
    public Guid? ParentCategoryId { get; private set; } = default!;
    public IReadOnlyCollection<Category>? ChildCategories { get; private set; } = default!;
    public IReadOnlyCollection<Product> Products { get; private set; } = default!;

    /// <summary>
    /// Constructor for root category.
    /// </summary>
    /// <param name="name"></param>
    /// <exception cref="ArgumentException"></exception>
    public Category(string name)
    {
        Name = name;
        ParentCategoryId = null;
        HierarchyDetail = new(Name, Id);
    }

    /// <summary>
    /// Constructor for child category.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parentCategory"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public Category(CategoryName name, Category parentCategory)
    {
        if (parentCategory is null or { HierarchyDetail: null })
            throw new ArgumentNullException(nameof(parentCategory), $"{nameof(ParentCategory)} or {nameof(ParentCategory)}.{nameof(HierarchyDetail)} cannot be null for create Child {nameof(Category)}.");

        Name = name;
        ParentCategoryId = parentCategory.Id;
        HierarchyDetail = new(Name, parentCategory);
    }

    private Category() { }

    public async Task IncludeHigherCategoriesAsync(ICategoryRepository categoryRepository, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(categoryRepository);

        ParentCategory = await categoryRepository.GetHigherCategoriesByCategoryAsync(this, cancellationToken);
    }

    public async Task IncludeLowerCategoriesAsync(ICategoryRepository categoryRepository, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(categoryRepository);

        ChildCategories = await categoryRepository.GetLowerCategoriesByCategoryAsync(this, cancellationToken: cancellationToken);
    }

    public void UpdateRootCategory(CategoryName name)
    {
        if (ChildCategories is null)
            throw new InvalidOperationException($"{nameof(ChildCategories)} must be included for update Root {nameof(Category)}.");

        var oldHierarchyName = HierarchyDetail.HierarchyName;
        Name = name;
        HierarchyDetail.Update(Name.Value.ToPluralize());
        UpdateRootChildrenRecursive(oldHierarchyName, ChildCategories);
    }

    private void UpdateRootChildrenRecursive(string oldRootHierarchyName, IEnumerable<Category>? childCategories)
    {
        if (childCategories is null)
            return;

        foreach (var child in childCategories)
        {
            if (Name == child.Name)
                throw new BadRequestException($"{nameof(Category)} with {nameof(Name)} equals '{Name}' exist in hierarchy.");

            child.HierarchyDetail.Update(child.HierarchyDetail.HierarchyName.ReplaceFirst(oldRootHierarchyName, HierarchyDetail.HierarchyName));
            UpdateRootChildrenRecursive(oldRootHierarchyName, child.ChildCategories);
        }
    }

    public void UpdateChildrenCategory(CategoryName name)
    {
        HierarchyDetail.Update(HierarchyDetail.HierarchyName.ReplaceLast(Name, name));

        if (name.Value.Equals(Name.Value, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new BadRequestException($"{nameof(Category)} have the same {nameof(Name)}.");
        }

        Name = name;
    }
}

public sealed class HierarchyDetail
{
    public CategoryLevel Level { get; private set; } = default!;
    public string HierarchyName { get; private set; } = default!;
    public string EncodedHierarchyName { get; private set; } = default!;
    public Guid RootCategoryId { get; private set; }

    public HierarchyDetail(string categoryName, Guid rootCategoryId)
    {
        Level = 0;
        HierarchyName = categoryName.ToPluralize();
        EncodedHierarchyName = HierarchyName.ToEncodedName();
        RootCategoryId = rootCategoryId;
    }

    public HierarchyDetail(string nameCategory, Category parentCategory)
    {
        Level = parentCategory.HierarchyDetail.Level + 1;
        HierarchyName = parentCategory.ParentCategoryId is null
            ? $"{parentCategory.HierarchyDetail.HierarchyName} {nameCategory}"
            : parentCategory.HierarchyDetail.HierarchyName.ReplaceLast(parentCategory.Name, nameCategory);
        EncodedHierarchyName = HierarchyName.ToEncodedName();
        RootCategoryId = parentCategory.HierarchyDetail.RootCategoryId;
    }

    private HierarchyDetail() { }

    internal void Update(string hierarchyName)
    {
        HierarchyName = hierarchyName;
        EncodedHierarchyName = HierarchyName.ToEncodedName();
    }
}
