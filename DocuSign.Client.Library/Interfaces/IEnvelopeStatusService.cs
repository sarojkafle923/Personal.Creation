using System.Threading.Tasks;
using DocuSign.Client.Library.Models.Login;

namespace DocuSign.Client.Library.Interfaces;

public interface IEnvelopeStatusService
{
    public Task<string> GetEnvelopeStatusService(Login loginDetail, string envelopeId);
}