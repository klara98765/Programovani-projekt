using Filmoteka.Models;

namespace Filmoteka.Repositories;

public interface IWatchStatusRepository
{
    IReadOnlyList<WatchStatus> GetAll();
}
