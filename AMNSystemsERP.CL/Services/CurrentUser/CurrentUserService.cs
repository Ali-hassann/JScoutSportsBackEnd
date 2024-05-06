using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace AMNSystemsERP.CL.Services.CurrentLogin
{
    public class CurrentLoginService : ICurrentLoginService
    {
        private string _userName { get; set; }
        public string UserName
        {
            get => _userName;
            set => _userName = value;
        }

        private string _outletName { get; set; }
        public string OutletName
        {
            get => _outletName;
            set => _outletName = value.ToString() ?? "";
        }

        private string _address { get; set; }
        public string Address
        {
            get => _address;
            set => _address = value.ToString() ?? "";
        }

        private string _outletImage { get; set; }
        public string OutletImage
        {
            get => _outletImage;
            set => _outletImage = value.ToString() ?? "";
        }

        private string _organizationId { get; set; }
        public long OrganizationId
        {
            get
            {
                try
                {
                    return !string.IsNullOrEmpty(_organizationId)
                            ? Convert.ToInt64(_organizationId)
                            : -1;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            set => _organizationId = value.ToString();
        }

        private string _outletId { get; set; }
        public long OutletId
        {
            get
            {
                try
                {
                    return !string.IsNullOrEmpty(_outletId)
                            ? Convert.ToInt64(_outletId)
                            : -1;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            set => _outletId = value.ToString();
        }

        public CurrentLoginService(IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                var headers = httpContextAccessor.HttpContext?.Request?.Headers;
                if (headers?.Keys?.Contains("currentlogin") ?? false)
                {
                    CurrentLogin user;
                    headers.TryGetValue("currentlogin", out var currentLogin);
                    headers.TryGetValue("OutletName", out var outletName);
                    headers.TryGetValue("Address", out var address);
                    headers.TryGetValue("OutletImage", out var image);
                    if (!string.IsNullOrEmpty(currentLogin))
                    {
                        user = JsonSerializer.Deserialize<CurrentLogin>(currentLogin);

                        if (user != null)
                        {
                            _outletId = user.OutletId ?? "0";
                            _organizationId = user.OrganizationId ?? "0";
                            _userName = user.UserName ?? "SYSTEM";
                        }

                        if (!string.IsNullOrEmpty(outletName)
                            && !string.IsNullOrEmpty(address)
                            && !string.IsNullOrEmpty(image))
                        {
                            _outletName = outletName;
                            _address = address;
                            _outletImage = image;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    class CurrentLogin
    {
        public string UserName { get; set; }
        public string OrganizationId { get; set; }
        public string OutletId { get; set; }
        public string OutletName { get; set; }
        public string Address { get; set; }
        public string CurrentOutletId { get; set; }
        public string OutletImage { get; set; }
    }
}
