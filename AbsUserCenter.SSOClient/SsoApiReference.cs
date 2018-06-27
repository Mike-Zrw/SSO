using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Script.Serialization;

namespace AbsUserCenter.SSOClient
{
    public class SsoApiReference : BaseSsoApiReference
    {
        public SsoApiReference(string baseurl) : base(baseurl)
        {
        }
        public SessionUser GetUserDataByToken(string token)
        {
            using (HttpResponseMessage response = HttpClientToken(token).PostAsync("Api/Account/GetUserDataByToken", null).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    SessionUser su = new JavaScriptSerializer().Deserialize<SessionUser>(result);
                    return su;
                }
                else
                {
                    throw new Exception("api响应失败：" + new JavaScriptSerializer().Serialize(response.Content));
                }
            }
        }
    }
}
