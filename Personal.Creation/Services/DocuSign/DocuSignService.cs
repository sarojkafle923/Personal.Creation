using DocuSign.Client.Library.Interfaces;
using DocuSign.Client.Library.Models.Login;
using DocuSign.Client.Library.Models.Request;
using DocuSign.eSign.Model;
using Personal.Creation.Interfaces.DocuSign;

namespace Personal.Creation.Services.DocuSign;

public class DocuSignService : IDocuSignService
{
    private readonly IConfiguration configuration;
    private readonly IEnvelopeFormDataService envelopeFormDataService;
    private readonly IEnvelopeStatusService envelopeStatusService;
    private readonly ISendEnvelopeViaEmailService sendEnvelopeViaEmailService;
    private readonly IUserLoginService userLoginService;

    public DocuSignService(IUserLoginService userLoginService, IConfiguration configuration,
        ISendEnvelopeViaEmailService sendEnvelopeViaEmailService, IEnvelopeStatusService envelopeStatusService,
        IEnvelopeFormDataService envelopeFormDataService)
    {
        this.userLoginService = userLoginService;
        this.configuration = configuration;
        this.sendEnvelopeViaEmailService = sendEnvelopeViaEmailService;
        this.envelopeStatusService = envelopeStatusService;
        this.envelopeFormDataService = envelopeFormDataService;
    }

    public Login DocuSignLogin()
    {
        var docuSignLoginRequest = new LoginRequest
        {
            ClientId = configuration["DocuSign:ClientId"],
            userId = configuration["DocuSign:ImpersonatedUserId"],
            AuthServer = configuration["DocuSign:AuthServer"],
            PrivateKeyBase64 = configuration["DocuSign:PrivateKeyBase64String"]
        };

        return userLoginService.Login(docuSignLoginRequest);
    }

    public async Task<EnvelopeSummary> SendViaEmail(Login login, EnvelopeDefinition envelopeDefinition)
    {
        // check and update login detail
        var isLoginRequired = userLoginService.IsLoginRequire(login.User);
        var updatedLogin = login;
        if (isLoginRequired) updatedLogin = DocuSignLogin();

        var envelopeSummary = await sendEnvelopeViaEmailService.SendEnvelopeViaEmail(updatedLogin, envelopeDefinition);
        return envelopeSummary;
    }

    public async Task<string> RequestEnvelopeStatus(Login login, string envelopeId)
    {
        //check and update login detail
        var isLoginRequired = userLoginService.IsLoginRequire(login.User);
        var updateLogin = login;
        if (isLoginRequired) updateLogin = DocuSignLogin();

        return await envelopeStatusService.GetEnvelopeStatusService(updateLogin, envelopeId);
    }

    public async Task<EnvelopeFormData> GetEnvelopeFormData(Login login, string envelopeId)
    {
        //check and update login detail 
        var isLoginRequired = userLoginService.IsLoginRequire(login.User);
        var updateLogin = login;
        if (isLoginRequired) updateLogin = DocuSignLogin();
        return await envelopeFormDataService.GetFormData(login, envelopeId);
    }
}