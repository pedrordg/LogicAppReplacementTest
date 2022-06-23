using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LogicAppCallTest
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            //check if manual task activated
            var manualTaskActivated = true;

            if (manualTaskActivated)
            {
                //if true, call task logic app
                var requestBody = new RequestPayload();
                requestBody.assignedToEntityId = "";
                requestBody.assignmentType = "UserGroup";
                requestBody.correlationId = "6da916fa-6c02-4137-982c-c9a84fc84c14";
                requestBody.createdBy = "9c6841aa-2edd-4fa0-8b1e-b8a5211f51cb";
                requestBody.data = "{\"processKey\":\"Cancel-Mandate\",\"processName\":\"Cancel-Mandate\",\"operationId\":\"6da916fa-6c02-4137-982c-c9a84fc84c14\",\"entityRelations\":null,\"mandateId\":\"c2e93d3c-dccf-473f-86fb-17723a8e767b\"}";
                requestBody.externalId = "f53c3a5e-75a4-4d47-bdc4-cc0754ff5cf5";
                requestBody.fourEyeSubjectId = "00000000-0000-0000-0000-000000000000";
                requestBody.operationId = "6da916fa-6c02-4137-982c-c9a84fc84c14";
                requestBody.sourceId = "6da916fa-6c02-4137-982c-c9a84fc84c14";
                requestBody.sourceName = "Cancel Mandate";
                requestBody.status = "Not Started";
                requestBody.subject = "Approve cancel e-mandate";
                requestBody.taskType = 0;

                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://prod-213.westeurope.logic.azure.com:443/workflows/6476bc8967d64e0ebb83fbd48a88ec18/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=1KNYxXnbqAGQOE3lmXOLeV8QQenOo6-LDwgdCZQqdoo"),
                    Content = new StringContent(JsonConvert.SerializeObject(requestBody), System.Text.Encoding.UTF8, "application/json")
                };
                request.Headers.Add("x-command-id", Guid.NewGuid().ToString());
                request.Headers.Add("x-external-id", "f53c3a5e-75a4-4d47-bdc4-cc0754ff5cf5");
                request.Headers.Add("x-request-id", Guid.NewGuid().ToString());
                request.Headers.Add("x-user-id", "f53c3a5e-75a4-4d47-bdc4-cc0754ff5cf5");
                request.Headers.Add("User-Agent", "PostmanRuntime/7.29.0");

                var response = await client.SendAsync(request).ConfigureAwait(false);
                var responseBody = await response.Content.ReadAsStringAsync();

                //expect 202 accepted =>
                if (response != null && response.StatusCode != HttpStatusCode.Accepted)
                {
                    return;
                }

                //fetch location =>
                if (string.IsNullOrEmpty(response.Headers.Location?.AbsoluteUri))
                {
                    return;
                }
                var httpResponseLocationUri = response.Headers.Location;

                //periodically pool http response location until a 200 ok response is received (task was completed on csp portal)
                //TODO: condition to stop the infinite loop eventually...
                do
                {
                    request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = httpResponseLocationUri
                    };

                    response = await client.SendAsync(request).ConfigureAwait(false);

                    Thread.Sleep(5000);
                }
                while (response.StatusCode != HttpStatusCode.OK);
            }

            //send start activity service bus message

            //call public api cancel mandate 

            //send end activity service bus message
        }

        public static byte[] Encoding<T>(this string content) where T : Encoding, new()
        {
            if (content == null) return new List<byte>().ToArray();

            Encoding encoding = new T();

            return encoding.GetBytes(content);
        }
    }

}
