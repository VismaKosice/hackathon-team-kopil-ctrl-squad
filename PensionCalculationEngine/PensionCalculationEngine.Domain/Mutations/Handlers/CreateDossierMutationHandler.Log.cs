using Microsoft.Extensions.Logging;

namespace PensionCalculationEngine.Domain.Mutations.Handlers;

internal sealed partial class CreateDossierMutationHandler
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Dossier created: dossier_id={DossierId} person_id={PersonId}")]
    private partial void LogDossierCreated(Guid dossierId, Guid personId);
}
