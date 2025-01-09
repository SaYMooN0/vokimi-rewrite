using AuthenticationService.Infrastructure.Middleware.eventual_consistency_middleware;
using Dapper;
using SharedKernel.Common;

namespace AuthenticationService.Infrastructure.Persistence;

internal abstract class BaseRepository
{
    protected readonly UnitOfWork _unitOfWork;

    protected BaseRepository(UnitOfWork unitOfWork) {
        _unitOfWork = unitOfWork;
    }


    protected async Task<int> ExecuteAsync(string sql, object? param = null) {
        int affectedRows = await _unitOfWork.Connection.ExecuteAsync(sql, param, _unitOfWork.Transaction);
        if (param is AggregateRoot aggregateRoot) {
            _unitOfWork.TrackAggregate(aggregateRoot);
        }

        return affectedRows;
    }


    protected async Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? param = null) {
        var result = await _unitOfWork.Connection.QuerySingleOrDefaultAsync<T>(sql, param, _unitOfWork.Transaction);
        if (result is AggregateRoot aggregateRoot) {
            _unitOfWork.TrackAggregate(aggregateRoot);
        }
        return result;
    }

    protected async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null) {
        var result = await _unitOfWork.Connection.QueryAsync<T>(sql, param, _unitOfWork.Transaction);
        foreach (var item in result) {
            if (item is AggregateRoot aggregateRoot) {
                _unitOfWork.TrackAggregate(aggregateRoot);
            }
        }
        return result;
    }
}