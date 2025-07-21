// MomoAdapter.cs - Adapter Pattern for BookingController
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

public interface IThanhToanGateway
{
    string TaoUrlThanhToan(decimal soTien, string maDonHang, string returnUrl);
}

public class MomoAdapter : IThanhToanGateway
{
    private readonly string endpoint = "https://test-payment.momo.vn/v2/gateway/api/create";
    private readonly string partnerCode = "MOMO";
    private readonly string accessKey = "F8BBA842ECF85";
    private readonly string secretKey = "K951B6PE1waDMi640xX08PD3vg6EkVlz";

    public string TaoUrlThanhToan(decimal soTien, string maDonHang, string returnUrl)
    {
        string requestId = Guid.NewGuid().ToString();
        string orderId = maDonHang;
        string orderInfo = $"Thanh toan don Booking #{maDonHang}";
        string amount = ((int)soTien).ToString();

        string rawHash = $"accessKey={accessKey}&amount={amount}&extraData=&ipnUrl={returnUrl}&orderId={orderId}&orderInfo={orderInfo}&partnerCode={partnerCode}&redirectUrl={returnUrl}&requestId={requestId}&requestType=payWithMethod";
        string signature = CreateSignature(rawHash, secretKey);

        var request = new CollectionLinkRequest
        {
            partnerCode = partnerCode,
            accessKey = accessKey,
            requestId = requestId,
            amount = amount,
            orderId = orderId,
            orderInfo = orderInfo,
            redirectUrl = returnUrl,
            ipnUrl = returnUrl,
            extraData = "",
            requestType = "payWithMethod",
            autoCapture = true,
            lang = "vi",
            partnerName = "MoMo Payment",
            storeId = "ATHENA Bookstore",
            orderGroupId = "",
            signature = signature
        };

        using (var client = new HttpClient())
        {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = client.PostAsync(endpoint, content).Result;
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var data = JsonConvert.DeserializeObject<MomoResponse>(responseBody);
            return data.payUrl;
        }
    }

    private string CreateSignature(string rawData, string secretKey)
    {
        byte[] keyByte = Encoding.UTF8.GetBytes(secretKey);
        byte[] messageBytes = Encoding.UTF8.GetBytes(rawData);
        using (var hmacsha256 = new HMACSHA256(keyByte))
        {
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
            return BitConverter.ToString(hashmessage).Replace("-", "").ToLower();
        }
    }

    private class MomoResponse
    {
        public string payUrl { get; set; }
    }

    private class CollectionLinkRequest
    {
        public string partnerCode { get; set; }
        public string accessKey { get; set; }
        public string requestId { get; set; }
        public string amount { get; set; }
        public string orderId { get; set; }
        public string orderInfo { get; set; }
        public string redirectUrl { get; set; }
        public string ipnUrl { get; set; }
        public string extraData { get; set; }
        public string requestType { get; set; }
        public bool autoCapture { get; set; }
        public string lang { get; set; }
        public string partnerName { get; set; }
        public string storeId { get; set; }
        public string orderGroupId { get; set; }
        public string signature { get; set; }
    }
}
