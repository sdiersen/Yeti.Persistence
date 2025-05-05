

using ErrorHandling;

using Microsoft.Extensions.Logging;

using Persistence.DTOs.Transaction;
using Persistence.ModelValidations;
using Persistence.ModelValidations.Transaction;
using Persistence.Repositories;

namespace Persistence.Services.Transaction;
public class EntryServices
{
    private readonly ILogger<EntryServices> _logger;
    private readonly RepositoryFactory _repositoryFactory;
    private readonly ModelValidationFactory _modelValidationFactory;
    private readonly EntryValidation _entryValidation;

    public EntryServices(
        ILogger<EntryServices> logger,
        RepositoryFactory repositoryFactory,
        ModelValidationFactory modelValidationFactory)
    {
        _logger = logger;
        _repositoryFactory = repositoryFactory;
        _modelValidationFactory = modelValidationFactory;
        _entryValidation = _modelValidationFactory.CreateEntryValidation();
    }

    //public ReturnValue CreateEntry(EntryDTO entryDTO)
    //{

    //}
}
