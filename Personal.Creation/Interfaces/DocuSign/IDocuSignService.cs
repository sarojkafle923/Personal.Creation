using DocuSign.Client.Library.Models.Login;
using DocuSign.eSign.Model;

namespace Personal.Creation.Interfaces.DocuSign;

public interface IDocuSignService
{
    public Login DocuSignLogin();

    public Task<EnvelopeSummary> SendViaEmail(Login login, EnvelopeDefinition envelopeDefinition);

    public Task<string> RequestEnvelopeStatus(Login login, string envelopeId);

    public Task<EnvelopeFormData> GetEnvelopeFormData(Login login, string envelopeId);
}