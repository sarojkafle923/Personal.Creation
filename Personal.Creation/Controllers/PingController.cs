using DocuSign.eSign.Model;
using Microsoft.AspNetCore.Mvc;
using Personal.Creation.Interfaces.DocuSign;

namespace Personal.Creation.Controllers;

[Route("api/test")]
public class PingController : Controller
{
    private readonly IDocuSignEnvelopeService docuSignEnvelopeService;
    private readonly IDocuSignService docuSignService;

    public PingController(IDocuSignService docuSignService, IDocuSignEnvelopeService docuSignEnvelopeService)
    {
        this.docuSignService = docuSignService;
        this.docuSignEnvelopeService = docuSignEnvelopeService;
    }

    [HttpGet("ping")]
    public async Task<IActionResult> Ping(string signerEmail, string signerName, string ccEmail, string ccName)
    {
        var login = docuSignService.DocuSignLogin();
        var envelopeDefinition =
            docuSignEnvelopeService.MakeSampleEnvelopeDefinition(signerEmail, signerName, ccEmail, ccName);
        var sendEmail = await docuSignService.SendViaEmail(login, envelopeDefinition);
        return Ok(sendEmail);
    }

    [HttpGet("status")]
    public async Task<IActionResult> GetFormData(string envelopeId)
    {
        var login = docuSignService.DocuSignLogin();
        var status = await docuSignService.RequestEnvelopeStatus(login, envelopeId);
        var formData = new EnvelopeFormData();
        if (status == "completed") formData = await docuSignService.GetEnvelopeFormData(login, envelopeId);

        return Ok(formData);
    }
}