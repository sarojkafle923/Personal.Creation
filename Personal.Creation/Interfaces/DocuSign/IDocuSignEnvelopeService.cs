using DocuSign.eSign.Model;

namespace Personal.Creation.Interfaces.DocuSign;

public interface IDocuSignEnvelopeService
{
    public EnvelopeDefinition MakeSampleEnvelopeDefinition(string signerEmail, string signerName, string ccEmail,
        string ccName);
}