using System.Text;
using DocuSign.eSign.Model;
using Personal.Creation.Interfaces.DocuSign;

namespace Personal.Creation.Services.DocuSign;

public class DocuSignEnvelopeService : IDocuSignEnvelopeService
{
    public EnvelopeDefinition MakeSampleEnvelopeDefinition(string signerEmail, string signerName, string ccEmail,
        string ccName)
    {
        var envelopeDefinition = new EnvelopeDefinition();

        // add documents
        var sampleDocument = CreateDocument(SampleDocument(signerEmail, signerName, ccEmail, ccName),
            "Sample Document", "html", "1");
        envelopeDefinition.Documents = new List<Document> { sampleDocument };

        // create a signer recipient to sign the document, identified by name and email
        var signer1 = new Signer
        {
            Email = signerEmail,
            Name = signerName,
            RecipientId = "1",
            RoutingOrder = "1",
            EmailNotification = new RecipientEmailNotification
            {
                EmailSubject = "Sample Signing Document",
                EmailBody = "Looks Like working"
            }
        };

        // routingOrder (lower means earlier) determines the order of deliveries
        // to the recipients. Parallel routing order is supported by using the
        // same integer as the order for two or more recipients.

        // create a cc recipient to receive a copy of the documents, identified by name and email
        var cc1 = new CarbonCopy
        {
            Email = ccEmail,
            Name = ccName,
            RecipientId = "2",
            RoutingOrder = "2",
            EmailNotification = new RecipientEmailNotification
            {
                EmailSubject = "Just look And Enjoy",
                EmailBody = "Good to see you!! :)"
            }
        };

        // Create signHere fields (also known as tabs) on the documents,
        // We're using anchor (autoPlace) positioning
        // The DocuSign platform searches throughout your envelope's
        // documents for matching anchor strings. So the
        // signHere2 tab will be used in both document 2 and 3 since they
        // use the same anchor string for their "signer 1" tabs.
        var signHere1 = new SignHere
        {
            AnchorString = "**signature_1**",
            AnchorUnits = "pixels",
            AnchorYOffset = "10",
            AnchorXOffset = "20"
        };

        var signHere2 = new SignHere
        {
            AnchorString = "/sn1/",
            AnchorUnits = "pixels",
            AnchorYOffset = "10",
            AnchorXOffset = "20"
        };

        // Tabs are set per recipient / signer
        var signer1Tabs = new Tabs
        {
            SignHereTabs = new List<SignHere> { signHere1, signHere2 }
        };
        signer1.Tabs = signer1Tabs;

        // Add the recipients to the envelope object
        var recipients = new Recipients
        {
            Signers = new List<Signer> { signer1 },
            CarbonCopies = new List<CarbonCopy> { cc1 }
        };
        envelopeDefinition.Recipients = recipients;

        // Request that the envelope be sent by setting |status| to "sent".
        // To request that the envelope be created as a draft, set to "created"
        envelopeDefinition.Status = "sent";

        return envelopeDefinition;
    }

    private Document CreateDocument(byte[] documentByteArray, string name, string fileExtension, string id)
    {
        return new Document
        {
            DocumentBase64 = Convert.ToBase64String(documentByteArray),
            Name = name,
            FileExtension = fileExtension,
            DocumentId = id
        };
    }

    private static byte[] SampleDocument(string signerEmail, string signerName, string ccEmail, string ccName)
    {
        // Data for this method
        // signerEmail
        // signerName
        // ccEmail
        // ccName
        return Encoding.UTF8.GetBytes(
            " <!DOCTYPE html>\n" +
            "    <html>\n" +
            "        <head>\n" +
            "          <meta charset=\"UTF-8\">\n" +
            "        </head>\n" +
            "        <body style=\"font-family:sans-serif;margin-left:2em;\">\n" +
            "        <h1 style=\"font-family: 'Trebuchet MS', Helvetica, sans-serif;\n" +
            "            color: darkblue;margin-bottom: 0;\">World Wide Corp</h1>\n" +
            "        <h2 style=\"font-family: 'Trebuchet MS', Helvetica, sans-serif;\n" +
            "          margin-top: 0px;margin-bottom: 3.5em;font-size: 1em;\n" +
            "          color: darkblue;\">Order Processing Division</h2>\n" +
            "        <h4>Ordered by " + signerName + "</h4>\n" +
            "        <p style=\"margin-top:0em; margin-bottom:0em;\">Email: " + signerEmail + "</p>\n" +
            "        <p style=\"margin-top:0em; margin-bottom:0em;\">Copy to: " + ccName + ", " + ccEmail + "</p>\n" +
            "        <p style=\"margin-top:3em;\">\n" +
            "  Candy bonbon pastry jujubes lollipop wafer biscuit biscuit. Topping brownie sesame snaps sweet roll pie. Croissant danish biscuit soufflé caramels jujubes jelly. Dragée danish caramels lemon drops dragée. Gummi bears cupcake biscuit tiramisu sugar plum pastry. Dragée gummies applicake pudding liquorice. Donut jujubes oat cake jelly-o. Dessert bear claw chocolate cake gummies lollipop sugar plum ice cream gummies cheesecake.\n" +
            "        </p>\n" +
            "        <!-- Note the anchor tag for the signature field is in white. -->\n" +
            "        <h3 style=\"margin-top:3em;\">Agreed: <span style=\"color:white;\">**signature_1**/</span></h3>\n" +
            "        </body>\n" +
            "    </html>");
    }
}