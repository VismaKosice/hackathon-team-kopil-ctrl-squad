using Microsoft.Extensions.Logging;
using PensionCalculationEngine.Domain.Common;
using PensionCalculationEngine.Domain.Mutations.Properties;
using PensionCalculationEngine.Shared.Models.PartialRequests;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.EndSituation;

namespace PensionCalculationEngine.Domain.Mutations.Handlers;

/// <summary>Handles <c>create_dossier</c> — initialises the dossier and adds the participant person.</summary>
internal sealed partial class CreateDossierMutationHandler : IMutationHandler
{
    private readonly ILogger<CreateDossierMutationHandler> _logger;

    public CreateDossierMutationHandler(ILogger<CreateDossierMutationHandler> logger)
    {
        _logger = logger;
    }

    public string MutationDefinitionName => Constants.MutationDefinitions.CreateDossier;

    public ValueTask HandleAsync(
        MutationBaseRequest mutation,
        CalculationContext context,
        CancellationToken cancellationToken = default)
    {
        if (context.Dossier is not null)
        {
            context.AddCritical(Constants.MessageCode.DossierAlreadyExists);
            return ValueTask.CompletedTask;
        }

        var props = CreateDossierProperties.From(mutation.MutationProperties);

        if (!props.BirthDateValid
            || props.BirthDate is null
            || props.BirthDate.Value > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            context.AddCritical(Constants.MessageCode.InvalidBirthDate);
            return ValueTask.CompletedTask;
        }

        if (string.IsNullOrWhiteSpace(props.Name))
        {
            context.AddCritical(Constants.MessageCode.InvalidName);
            return ValueTask.CompletedTask;
        }

        context.Dossier = new DossierResponse
        {
            Id = props.DossierId.ToString(),
            Status = Constants.DossierStatus.Active,
            RetirementDate = null,
            Persons =
            [
                new PersonResponse
                {
                    Id = props.PersonId,
                    Role = Constants.PersonRole.Participant,
                    Name = props.Name,
                    BirthDate = props.BirthDate.Value,
                },
            ],
            Policies = [],
        };

        LogDossierCreated(props.DossierId, props.PersonId);
        return ValueTask.CompletedTask;
    }
}
