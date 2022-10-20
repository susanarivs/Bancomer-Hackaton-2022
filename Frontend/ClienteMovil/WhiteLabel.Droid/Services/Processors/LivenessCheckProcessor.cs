using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Facetec.Sdk;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace WhiteLabel.Droid.Services.Processors
{
    public class LivenessCheckProcessor : Java.Lang.Object, IFaceTecFaceScanProcessor, IProcessor
    {
        private bool success = false;

        public LivenessCheckProcessor(Context context, string sessionToken)
        {
            FaceTecSessionActivity.CreateAndLaunchSession(context, this, sessionToken);
        }

        public void ProcessSessionWhileFaceTecSDKWaits(FaceTecSessionResult p0, IFaceTecFaceScanResultCallback p1)
        {
            if (p0.Status != FaceTecSessionStatus.SessionCompletedSuccessfully)
            {
                p1.Cancel();
                return;
            }

            try
            {
                var parameters = new JObject(
                                            new JProperty("faceScan", p0.FaceScanBase64),
                                            new JProperty("auditTrailImage", p0.GetAuditTrailCompressedBase64()[0]),
                                            new JProperty("lowQualityAuditTrailImage", p0.GetLowQualityAuditTrailCompressedBase64()[0]));

                //subir aqui
                var content = new StringContent(parameters.ToString(), Encoding.UTF8, "application/json");
                var content2 = new ProgressableStreamContent(content, (sent, total) =>
                {
                    var percent = ((float)sent / total) * 100f;
                    p1.UploadProgress(percent);
                });

                var agente = FaceTecSDK.CreateFaceTecAPIUserAgentString(p0.SessionId);

                var client = new HttpClient();
                //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
                client.DefaultRequestHeaders.Add("X-Device-Key", FacetecConsts.DeviceKeyIdentifier);
                client.DefaultRequestHeaders.Add("X-User-Agent", agente);

                var response = client.PostAsync(FacetecConsts.BaseURL + "/liveness-3d", content2).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    dynamic parsedResponse = JObject.Parse(result);

                    bool wasProcessed = parsedResponse.wasProcessed;
                    string scanResultBlob = parsedResponse.scanResultBlob;

                    if (wasProcessed)
                    {
                        FaceTecCustomization.OverrideResultScreenSuccessMessage = "Simon, estas vivo!";

                        success = p1.ProceedToNextStep(scanResultBlob);
                    }
                    else
                    {
                        p1.Cancel();
                    }
                }

            }
            catch (Exception ex)
            {
                //No se pudo crear el payload
                p1.Cancel();
            }
        }

        public bool isSuccess()
        {
            return success;
        }
    }
}