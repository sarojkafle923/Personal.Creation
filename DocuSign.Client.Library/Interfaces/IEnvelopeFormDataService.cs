using System.Threading.Tasks;
using DocuSign.Client.Library.Models.Login;
using DocuSign.eSign.Model;

namespace DocuSign.Client.Library.Interfaces;

public interface IEnvelopeFormDataService
{
    public Task<EnvelopeFormData> GetFormData(Login loginDetail, string envelopeId);
}