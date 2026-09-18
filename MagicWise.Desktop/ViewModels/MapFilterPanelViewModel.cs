using CommunityToolkit.Mvvm.ComponentModel;
using MagicWise.Core.Models.Enums;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MagicWise.Desktop.ViewModels;

/// <summary>
/// The hamburger (left) panel content shown once a park has been selected: the
/// park's name plus an expandable tree of entity-type categories (Attractions,
/// Restaurants, Shows, ...), each listing every individual entity as a checkbox
/// that controls whether it renders as a pin on the map.
/// </summary>
public partial class MapFilterPanelViewModel : ObservableObject
{
    // Display names and preferred ordering for known entity types. Any entity
    // type not listed here still gets its own category (using its enum name),
    // sorted after all known types. This is data-driven: a category only ever
    // appears once the park actually has at least one entity of that type, so
    // e.g. Hotels naturally stays hidden today (the API never returns any) and
    // will show up automatically the moment that data exists.
    private static readonly Dictionary<EntityType, string> CategoryDisplayNames = new()
    {
        [EntityType.Attraction] = "Attractions",
        [EntityType.Restaurant] = "Restaurants",
        [EntityType.Hotel] = "Hotels",
        [EntityType.Show] = "Shows",
    };

    private static readonly EntityType[] CategoryOrder =
    {
        EntityType.Attraction,
        EntityType.Restaurant,
        EntityType.Show,
        EntityType.Hotel,
    };

    private readonly ObservableCollection<EntityChildViewModel> _children;
    private readonly Action<IReadOnlySet<string>> _onVisibleIdsChanged;

    public MapFilterPanelViewModel(
        ParkViewModel park,
        ObservableCollection<EntityChildViewModel> children,
        Action<IReadOnlySet<string>> onVisibleIdsChanged)
    {
        Park = park;
        _children = children;
        _onVisibleIdsChanged = onVisibleIdsChanged;

        Categories = new ObservableCollection<EntityCategoryViewModel>();

        foreach (var child in _children)
        {
            AddChild(child);
        }

        _children.CollectionChanged += OnChildrenCollectionChanged;

        RaiseVisibleIdsChanged();
    }

    public ParkViewModel Park { get; }

    public ObservableCollection<EntityCategoryViewModel> Categories { get; }

    private void OnChildrenCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
        {
            foreach (EntityChildViewModel child in e.NewItems)
            {
                AddChild(child);
            }
        }

        RaiseVisibleIdsChanged();
    }

    private void AddChild(EntityChildViewModel child)
    {
        var category = Categories.FirstOrDefault(c => c.EntityType == child.EntityType);
        if (category == null)
        {
            var displayName = CategoryDisplayNames.GetValueOrDefault(child.EntityType, child.EntityType.ToString());
            category = new EntityCategoryViewModel(child.EntityType, displayName, RaiseVisibleIdsChanged);
            InsertCategoryInOrder(category);
        }

        category.AddItem(child.Id, child.Name);
    }

    private void InsertCategoryInOrder(EntityCategoryViewModel category)
    {
        var newTypeOrder = Array.IndexOf(CategoryOrder, category.EntityType);
        if (newTypeOrder < 0)
        {
            newTypeOrder = CategoryOrder.Length;
        }

        var insertAt = Categories.Count;
        for (var i = 0; i < Categories.Count; i++)
        {
            var existingTypeOrder = Array.IndexOf(CategoryOrder, Categories[i].EntityType);
            if (existingTypeOrder < 0)
            {
                existingTypeOrder = CategoryOrder.Length;
            }

            if (existingTypeOrder > newTypeOrder)
            {
                insertAt = i;
                break;
            }
        }

        Categories.Insert(insertAt, category);
    }

    private void RaiseVisibleIdsChanged()
    {
        var visible = Categories
            .SelectMany(c => c.Items)
            .Where(i => i.IsChecked)
            .Select(i => i.Id)
            .ToHashSet();

        _onVisibleIdsChanged(visible);
    }
}
