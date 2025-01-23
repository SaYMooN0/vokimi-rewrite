using SharedKernel.Common;
using SharedKernel.Common.EntityIds;
using SharedKernel.Common.errors;

namespace TestCreationService.Domain.Common;

public class EntitiesOrderController<T> where T : EntityId
{
    private EntitiesOrderController() { }
    private Dictionary<T, ushort> _entityOrders { get; init; }
    public bool IsShuffled { get; set; }
    public static EntitiesOrderController<T> Empty(bool isShuffled) => new() { _entityOrders = new(), IsShuffled = isShuffled };
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
    private void ReevaluateOrders() {
        var orderedKeys = _entityOrders.Keys
            .OrderBy(key => _entityOrders[key])
            .ToList();

        for (ushort i = 0; i < orderedKeys.Count; i++) {
            _entityOrders[orderedKeys[i]] = (ushort)(i + 1);
        }
    }

    public void AddToEnd(Entity<T> entity) {
        if (_entityOrders.ContainsKey(entity.Id)) {
            throw new InvalidOperationException($"Entity with Id {entity.Id} is already in the controller.");
        }

        ushort newOrder = (ushort)(_entityOrders.Count + 1);
        //_entityOrders = new();
        _entityOrders.Add(entity.Id, newOrder);
    }
    public void RemoveEntity(Entity<T> entity) {
        _entityOrders.Remove(entity.Id);
        ReevaluateOrders();
    }
    public IReadOnlyList<(EntityType Entity, ushort Order)> GetItemsWithOrders<EntityType>(IEnumerable<EntityType> entities) where EntityType : Entity<T> {
        return entities
            .Select(e => (e, _entityOrders[e.Id]))
            .OrderBy(e => e.Item2)
            .ToList()
            .AsReadOnly();
    }
    public bool Contains(T id) => _entityOrders.ContainsKey(id);
    public T[] EntityIds() => _entityOrders.Keys.ToArray();
    public EntitiesOrderController<T> WithoutEntityIds(IEnumerable<T> idsToRemove) {
        ushort order = 1;
        var newIds = EntityIds()
            .Except(idsToRemove)
            .OrderBy(id => _entityOrders[id])
            .ToDictionary(id => id, id => order++);
        return new() {
            _entityOrders = newIds,
            IsShuffled = IsShuffled
        };
    }
    //public IReadOnlyList<EntityType> GetItemsByShuffledState<EntityType>(IEnumerable<EntityType> entities) where EntityType : Entity<T> {
    //    if (IsShuffled) {
    //        return entities
    //            .OrderBy(_ => Guid.NewGuid())
    //            .ToImmutableList();
    //    }

    //    return entities
    //        .OrderBy(e => _entityOrders[e.Id])
    //        .ToImmutableList();
    //}
    //public ErrOrNothing RemoveByOrder(ushort orderToRemove) {
    //    var entityToRemove = _entityOrders.FirstOrDefault(e => e.Value == orderToRemove).Key;
    //    _entityOrders.Remove(entityToRemove);
    //    ReevaluateOrders();
    //    return ErrOrNothing.Nothing;
    //}
}
