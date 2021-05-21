using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PaytmShellSample.PaymentGateway
{
	public enum PaytmErrors
    {
		SUCCESS,
		FAILURE
	}

	public class PayTmPayment
	{
		public static Tuple<PaytmErrors, string,string> GenerateTransactionToken(string amountValueUptoTwoDecimals, string custId, string orderID,string websiteName, string requestType, string currency = "INR")
		{
			try
			{
				Dictionary<string, object> body = new Dictionary<string, object>();
				Dictionary<string, string> head = new Dictionary<string, string>();
				Dictionary<string, object> requestBody = new Dictionary<string, object>();
				Dictionary<string, string> txnAmount = new Dictionary<string, string>();
				Dictionary<string, string> userInfo = new Dictionary<string, string>();
				Dictionary<string, object> UPIPaymentOptions = new Dictionary<string, object>();

				UPIPaymentOptions.Add("mode", "UPI");
				UPIPaymentOptions.Add("channels", new[] { "UPI","UPIPUSH","UPIPUSHEXPRESS" });

				txnAmount.Add("value", amountValueUptoTwoDecimals);
				txnAmount.Add("currency", currency);
				userInfo.Add("custId", custId); //userInfo.Add("custId", "Utk_1");


				body.Add("requestType", requestType); //body.Add("requestType", "Payment");
				body.Add("mid", "qFKjOk51773158003561"); //body.Add("mid", "qFKjOk51773158003561");
				body.Add("websiteName", websiteName); //body.Add("websiteName", "WEBSTAGING");
				body.Add("orderId", orderID); //body.Add("orderId", "order_1");
				body.Add("txnAmount", txnAmount);
				body.Add("userInfo", userInfo);
				body.Add("callbackUrl", "https://securegw-stage.paytm.in/theia/paytmCallback?ORDER_ID=" + orderID); //body.Add("callbackUrl", "https://merchant.com/callback");
				body.Add("enablePaymentMode", new object[] { UPIPaymentOptions });


				string paytmChecksum = Paytm.Checksum.generateSignature(JsonConvert.SerializeObject(body), "k0ZxZSES5XJZ9G@r");

				head.Add("signature", paytmChecksum);

				requestBody.Add("body", body);
				requestBody.Add("head", head);

				string post_data = JsonConvert.SerializeObject(requestBody);

				string url = "https://securegw-stage.paytm.in/theia/api/v1/initiateTransaction?mid=" + "qFKjOk51773158003561" + "&orderId=" + orderID;

				HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);

				webRequest.Method = WebRequestMethods.Http.Post;
				webRequest.ContentType = "application/json";
				webRequest.ContentLength = post_data.Length;

				using (StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream()))
				{
					requestWriter.Write(post_data);
				}

				string responseData = string.Empty;

				using (StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
				{
					responseData = responseReader.ReadToEnd();

					JObject returnedobject = JObject.Parse(responseData);

					var resultStatus =  returnedobject["body"]["resultInfo"]["resultStatus"].Value<string>();

					var resultMessage = returnedobject["body"]["resultInfo"]["resultMsg"].Value<string>();

					if (string.Compare(resultStatus, "S", true) == 0)
					{
						string txnToken = returnedobject["body"]["txnToken"].Value<string>();


						return new Tuple<PaytmErrors, string,string>(PaytmErrors.SUCCESS, txnToken,resultMessage);
					}
					else
					{
						string nullTransactionToken = null;

						return new Tuple<PaytmErrors, string,string>(PaytmErrors.FAILURE, nullTransactionToken, resultMessage);
					}
				}
			}
			catch (Exception exception)
			{
				string nullTransactionToken = null;

				return new Tuple<PaytmErrors, string,string>(PaytmErrors.FAILURE, nullTransactionToken,"ERROR OCCURED,BUT NO MESSAGE SENT");
			}

			return null;
		}
	}
}
