using AbsUserCenter.Tool;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbsUserCenter.Token
{
    public class TokenBuilder
    {
        /// <summary>
        /// 获取指定时间时间距离1970年的秒数
        /// </summary>
        /// <param name="time">默认为当前时间</param>
        /// <returns></returns>
        public static long GetTimeSecond(Nullable<DateTime> time = null)
        {
            if (time == null)
                return (DateTime.Now.Ticks - DateTime.Parse("1970-01-01 00:00:00").Ticks) / 10000000;
            return (((DateTime)time).Ticks - DateTime.Parse("1970-01-01 00:00:00").Ticks) / 10000000;
        }
        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string MakeToken(string loginName, int sysid, long userid)
        {
            TokenClaims Claim = GetTokenClaims(loginName, sysid, userid);
            var token = EncodeToken(Claim);
            return token;
        }
        /// <summary>
        /// 生成一个token结构
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="sysid"></param>
        /// <returns></returns>
        private static TokenClaims GetTokenClaims(string loginName, int sysid, long userid)
        {
            long time = GetTimeSecond();
            return new TokenClaims()
            {
                TokenPayload = new TokenPayload()
                {
                    UsrId = userid,
                    Iat = time,
                    Iss = "absucenter",
                    SysId = sysid,
                    Name = loginName,
                    Exp = GetTimeSecond(DateTime.Now.AddSeconds(50))
                }
            };
        }
        /// <summary>
        /// 加密token结构为
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private static string EncodeToken(TokenClaims token)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            var tokenStr = encoder.Encode(token.TokenPayload, ConfigHelper.GetConfig<string>("jwtsecret"));
            return tokenStr;
        }

        /// <summary>
        /// 解密token为token结构
        /// </summary>
        /// <param name="encodetokenStr"></param>
        /// <returns></returns>
        public static TokenClaims DecodeToken(string encodetokenStr)
        {
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
            var json = decoder.Decode(encodetokenStr, ConfigHelper.GetConfig<string>("jwtsecret"), verify: true);//token为之前生成的字符串
            TokenPayload paload = serializer.Deserialize<TokenPayload>(json);
            return new TokenClaims() { TokenPayload = paload };
        }

        /// <summary>
        /// token是否有效
        /// </summary>
        /// <param name="encodetokenStr"></param>
        /// <returns></returns>
        public static bool IsValid(string encodetokenStr)
        {
            TokenClaims claim = DecodeToken(encodetokenStr);
            if (claim == null)
            {
                return false;
            }
            return !IsOverTime(claim);
        }

        /// <summary>
        /// token是否过期
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool IsOverTime(TokenClaims claim)
        {
            long time = GetTimeSecond();
            long overtime = claim.TokenPayload.Exp;
            if (time > overtime)
            {
                return true;
            }
            return false;
        }
    }
}
