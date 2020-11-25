using HVIT.Security;
using HVITCore.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Data.Entity;
using QuanLyKhachSanApp.Models;

namespace QuanLyKhachSanApp.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : BaseApiController
    {
        public class LoginForm
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string RePassword { get; set; }
            public string ChangePasswordKey { get; set; }
            public string OldPassword { get; set; }
        }

        public class UserModel
        {
            public Users User { get; set; }
            public List<int> Groups { get; set; }

            public UserModel()
            {
                Groups = new List<int>();
            }
        }

        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login(LoginForm login)
        {
            using (dbQuanLyKhachSan db = new dbQuanLyKhachSan())
            {
                Users user = db.Users.SingleOrDefault(x => x.UserName == login.Username);
                if (user != null)
                {
                    string passwordSalt = user.PasswordSalt;
                    string passwordInput = AuthenticationHelper.GetMd5Hash(passwordSalt + login.Password);
                    string passwordUser = user.Password;

                    if (passwordInput.Equals(passwordUser))
                    {
                        TokenProvider tokenProvider = new TokenProvider();
                        TokenIdentity token = tokenProvider.GenerateToken(user.UserId, login.Username,
                            Request.Headers.UserAgent.ToString(),
                            "", Guid.NewGuid().ToString(),
                            DateTime.Now.Ticks);
                        token.SetAuthenticationType("Custom");
                        token.SetIsAuthenticated(true);
                        db.AccessTokens.Add(new AccessTokens()
                        {
                            Token = token.Token,
                            EffectiveTime = new DateTime(token.EffectiveTime),
                            ExpiresIn = token.ExpiresTime,
                            IP = token.IP,
                            UserAgent = token.UserAgent,
                            UserName = token.Name
                        });
                        db.SaveChanges();
                        var nhanVien = db.NhanVien
                            .Include(x => x.BoPhan)
                            .Select(x => new
                            {
                                x.NhanVienID,
                                x.UsersID,
                                x.BoPhanID,
                                x.TenNhanVien,
                                x.SoDienThoai,
                                x.BoPhan.TenBoPhan
                            })
                            .FirstOrDefault(x => x.UsersID == user.UserId);
                        return Ok(
                            new
                            {
                                AccessToken = token,
                                Profile = new
                                {
                                    UserId = user.UserId,
                                    Username = user.UserName,
                                    NhanVien = nhanVien,
                                }
                            }
                        );
                    }
                }
                return Ok("Login failed!");
            }
        }

        [AuthorizeUser, HttpGet]
        [Route("logout")]
        public IHttpActionResult Logout(int UserID)
        {
            using (var db = new dbQuanLyKhachSan())
            {
                var user = db.Users.Where(x => x.UserId == UserID).FirstOrDefault();
                if (user == null)
                    return BadRequest("invalid UserID");
                db.SaveChanges();
                return Ok();
            }
        }
        [AuthorizeUser, HttpGet]
        [Route("validate-token")]
        public IHttpActionResult ValidateToken()
        {
            TokenIdentity tokenIdentity = ClaimsPrincipal.Current.Identity as TokenIdentity;
            return Ok();
        }
        
    }
    public class AllowCrossSiteJsonAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            filterContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            base.OnActionExecuting(filterContext);
        }
    }
}
