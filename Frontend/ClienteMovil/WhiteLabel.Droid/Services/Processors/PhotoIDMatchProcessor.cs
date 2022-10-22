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
    public class PhotoIDMatchProcessor : Java.Lang.Object, IFaceTecFaceScanProcessor, IFaceTecIDScanProcessor, IProcessor
    {
        private bool success = false;

        string _latestExternalDatabaseRefId = Guid.NewGuid().ToString();

        public PhotoIDMatchProcessor(Context context, string sessionToken)
        {
            /*FaceTecCustomization.SetIDScanUploadMessageOverrides(
                "Subiendo\nEscaneo frontal de ID\nEncriptado",
                "Cargando...\nConexión lenta", 
                "Carga completada", 
                "Procesando Escaneo de ID",
                "Subiendo\nReverso de ID", 
                "Cargando...\nConexión lenta", 
                "Carga completada", 
                "Procesando Reverso de ID", 
                "Cargando\nInformación confirmada", 
                "Cargando...\nConexión lenta", 
                "Carga completada",
                "Procesando",
                "Subiendo detalles NFC\nEncriptado",
                "Cargando...\nConexión lenta",
                "Carga completada",
                "Procesando\nDetalles NFC");*/

            FaceTecSessionActivity.CreateAndLaunchSession(context, this, this, sessionToken);
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
                                            new JProperty("lowQualityAuditTrailImage", p0.GetLowQualityAuditTrailCompressedBase64()[0]),
                                            new JProperty("externalDatabaseRefID", _latestExternalDatabaseRefId));

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

                var response = client.PostAsync(FacetecConsts.BaseURL + "/enrollment-3d", content2).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    dynamic parsedResponse = JObject.Parse(result);

                    bool wasProcessed = parsedResponse.wasProcessed;
                    string scanResultBlob = parsedResponse.scanResultBlob;

                    if (wasProcessed)
                    {
                        FaceTecCustomization.OverrideResultScreenSuccessMessage = "!Prueba de vida confirmada!";

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
                System.Diagnostics.Debug.WriteLine(ex.Message);
                p1.Cancel();
            }
        }

        public void ProcessIDScanWhileFaceTecSDKWaits(FaceTecIDScanResult p0, IFaceTecIDScanResultCallback p1)
        {
            if (p0.Status != FaceTecIDScanStatus.Success)
            {
                p1.Cancel();
                return;
            }

            try
            {
                var minMatchLevel = 3;  //No puede ser 0

                var parameters = new JObject();
                parameters.Add(new JProperty("idScan", p0.IDScanBase64));
                if (p0.FrontImagesCompressedBase64?.Count > 0)
                {
                    parameters.Add(new JProperty("idScanFrontImage", p0.FrontImagesCompressedBase64[0]));
                }
                if (p0.BackImagesCompressedBase64?.Count > 0)
                {
                    parameters.Add(new JProperty("idScanBackImage", p0.BackImagesCompressedBase64[0]));
                }
                parameters.Add(new JProperty("minMatchLevel", minMatchLevel));
                parameters.Add(new JProperty("externalDatabaseRefID", _latestExternalDatabaseRefId));

                var content = new StringContent(parameters.ToString(), Encoding.UTF8, "application/json");
                var content2 = new ProgressableStreamContent(content, (sent, total) =>
                {
                    var percent = ((float)sent / total) * 100f;
                    p1.UploadProgress(percent);
                });

                var agente = FaceTecSDK.CreateFaceTecAPIUserAgentString(p0.SessionId);

                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("X-Device-Key", FacetecConsts.DeviceKeyIdentifier);
                client.DefaultRequestHeaders.Add("X-User-Agent", agente);

                var response = client.PostAsync(FacetecConsts.BaseURL + "/match-3d-2d-idscan", content2).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    dynamic parsedResponse = JObject.Parse(result);

                    bool wasProcessed = parsedResponse.wasProcessed;
                    string scanResultBlob = parsedResponse.scanResultBlob;

                    if (wasProcessed)
                    {
                        FaceTecCustomization.SetIDScanResultScreenMessageOverrides(
                                "El rostro 3D\ncoincide con la Identificación", // Successful scan of ID front-side (ID Types with no back-side).
                                "El rostro 3D\ncoincide con la Identificación", // Successful scan of ID front-side (ID Types that do have a back-side).
                                "Reverso de ID Capturado", // Successful scan of the ID back-side.
                                "Verificación con credencial\nCompletado", // Successful upload of final IDScan containing User-Confirmed ID Text.
                                "Información del chip NFC\nVerificada", // Successful upload of the scanned NFC chip information.
                                "El rostro no coincide\nLo suficiente", // Case where a Retry is needed because the Face on the Photo ID did not Match the User's Face highly enough.
                                "El Documento de identidad\nNo es completamente visible", // Case where a Retry is needed because a Full ID was not detected with high enough confidence.
                                "El texto de la identificación no es visible", // Case where a Retry is needed because the OCR did not produce good enough results and the User should Retry with a better capture.
                                "Tipo de ID no admitida\nUtilice una identificación diferente", // Case where there is likely no OCR Template installed for the document the User is attempting to scan.
                                "Información de escaneo NFC\nSubida exitosamente" // Case where NFC Scan was skipped due to the user's interaction or an unexpected error.
                        );

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
                System.Diagnostics.Debug.WriteLine(ex.Message);
                p1.Cancel();
            }
        }

        public bool isSuccess()
        {
            return success;
        }
    }
}