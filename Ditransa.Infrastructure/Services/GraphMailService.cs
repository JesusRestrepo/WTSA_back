using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Graph;
using Ditransa.Application.Interfaces;
using Ditransa.Application.DTOs;

namespace Ditransa.Infrastructure.Services
{
    public class GraphMailService : IMailService
    {
        private readonly IConfiguration _configuration;

        private readonly string? clientId;

        private readonly string? tenantId;

        private readonly string? clientSecret;

        private readonly string? email;

        private readonly string[] scopes;

        private GraphServiceClient _graphClient;

        public GraphMailService(IConfiguration configuration)
        {
            _configuration = configuration;

            clientId = _configuration["Graph:ClientId"];
            tenantId = _configuration["Graph:TenantId"];
            clientSecret = _configuration["Graph:ClientSecret"];
            email = _configuration["Graph:Email"];
            scopes = new string[] { "https://graph.microsoft.com/.default" };
        }

        public async Task SendEmail(string subject, string[] toRecipients, string body, bool isHtml, FileDto[] attachments)
        {
            var token = await GetToken();

            var url = $"https://graph.microsoft.com/v1.0/users/{email}/sendMail";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var message = new Dictionary<string, object>
            {
                { "subject", subject },
                {
                    "body",
                    new Dictionary<string, object>
            {
                { "contentType", isHtml ? "html" : "text"},
                { "content", body },
            }
                }
            };

            var recipients = new List<Dictionary<string, object>>();

            foreach (var recipient in toRecipients)
            {
                var objectRecipient = new Dictionary<string, object>
                {
                    {
                        "emailAddress",
                        new Dictionary<string, object>
                {
                    { "address", recipient },
                }
                    }
                };
                recipients.Add(objectRecipient);
            }

            message.Add("toRecipients", recipients);

            if (attachments != null && attachments.Any())
            {
                var listAttachments = new List<Dictionary<string, object>>();

                foreach (var attachment in attachments)
                {
                    var objectAttachment = new Dictionary<string, object>
                  {
                      {
                        "@odata.type", "#microsoft.graph.fileAttachment"
                    },
                      {
                        "name", attachment.Name
                    },
                      {
                        "contentBytes", Convert.ToBase64String(attachment.Content)
                    }
                };
                    listAttachments.Add(objectAttachment);
                }

                message.Add("attachments", listAttachments);
            }

            var requestObject = new Dictionary<string, object>
            {
                { "message", message },
                { "saveToSentItems", "true" },
            };

            var requestContent = new StringContent(JsonConvert.SerializeObject(requestObject), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(url, requestContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error al enviar el correo. StatusCode: {response.StatusCode}");
            }
        }

        private async Task<string> GetToken()
        {
            string token = string.Empty;
            string url = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";

            var httpClient = new HttpClient();

            var requestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("scope", "https://graph.microsoft.com/.default"),
            });

            var response = await httpClient.PostAsync(url, requestContent);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var objectResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);
                token = objectResponse["access_token"].ToString();
            }

            return token;
        }
    }

    public class File
    {
        public string Name { get; set; }

        public byte[] Content { get; set; }
    }
}