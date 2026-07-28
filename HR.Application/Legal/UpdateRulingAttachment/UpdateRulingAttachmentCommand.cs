using Modules.Shared.Application.Messaging;

namespace HR.Application.Legal.UpdateRulingAttachment
{
    public record UpdateRulingAttachmentCommand(
        Guid RulingId,
        string AttachmentPath
    ) : ICommand;
}
