using System.Threading.Tasks;
using DocuSign.Client.Library.Models.Login;
using DocuSign.eSign.Model;

namespace DocuSign.Client.Library.Interfaces;

public interface ISendEnvelopeViaEmailService
{
    public Task<EnvelopeSummary> SendEnvelopeViaEmail(Login loginDetail, EnvelopeDefinition envelopeDefinition);
}