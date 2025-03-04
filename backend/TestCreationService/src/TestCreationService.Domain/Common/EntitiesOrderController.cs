using System.Collections.Immutable;
using SharedKernel.Common.domain;
using SharedKernel.Common.domain.entity;
using SharedKernel.Common.errors;

namespace TestCreationService.Domain.Common;

public class EntitiesOrderController<T> where T : EntityId
{
    private EntitiesOrderController() { }
    private Dictionary<T, ushort> _entityOrders { get; init; }
    public bool IsShuffled { get; set; }

    public static EntitiesOrderController<T> Empty(bool isShuffled) =>
        new() { _entityOrders = new(), IsShuffled = isShuffled };

    public static ErrOr<EntitiesOrderController<T>> CreateNew(
        bool isShuffled,
        Dictionary<T, ushort> entityOrders
    ) {
        var orders = entityOrders.Values.OrderBy(v => v).ToArray();
        for (ushort i = 0; i < orders.Length; i++) {
            if (orders[i] != i + 1) {
                return Err.ErrFactory.InvalidData(
                    "Entity orders are not in a valid sequence.",
                    details: $"Expected order {i + 1}, but found {orders[i]} at position {i}."
                );
            }
        }

        return new EntitiesOrderController<T> {
            _entityOrders = new Dictionary<T, ushort>(entityOrders),
            IsShuffled = isShuffled
        };
    }

    public int Count => _entityOrders.Count;
    public ImmutableHashSet<T> EntityIds() => _entityOrders.Keys.ToImmutableHashSet();

    private void ReevaluateOrders() {
        var orderedKeys = _entityOrders.Keys
            .OrderBy(key => _entityOrders[key])
            .ToList();

        for (ushort i = 0; i < orderedKeys.Count; i++) {
            _entityOrders[orderedKeys[i]] = (ushort)(i + 1);
        }
    }

    public void AddToEnd(T entityId) {
        if (_entityOrders.ContainsKey(entityId)) {
            throw new InvalidOperationException($"Entity with Id {entityId} is already in the controller.");
        }

        ushort newOrder = (ushort)(_entityOrders.Count + 1);
        _entityOrders.Add(entityId, newOrder);
    }

    public void RemoveEntity(T entityId) {
        _entityOrders.Remove(entityId);
        ReevaluateOrders();
    }

    public ImmutableArray<(EntityType Entity, ushort Order)> GetItemsWithOrders<EntityType>(
        IEnumerable<EntityType> entities) where EntityType : Entity<T> {
        return entities
            .Select(e => (e, _entityOrders[e.Id]))
            .OrderBy(e => e.Item2)
            .ToList()
            .ToImmutableArray();
    }

    public bool Contains(T id) => _entityOrders.ContainsKey(id);

    public EntitiesOrderController<T> Copy() {
        ushort order = 1;
        var newIds = EntityIds()
            .OrderBy(id => _entityOrders[id])
            .ToDictionary(id => id, id => order++);
        return new() {
            _entityOrders = newIds,
            IsShuffled = IsShuffled
        };
    }
}