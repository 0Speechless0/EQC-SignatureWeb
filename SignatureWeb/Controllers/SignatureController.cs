using EQC.Services;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignatureWeb.Shared.Models;

namespace SignatureWeb.Server.Controllers
{
    [ApiController]
    [Route("api")]
    public class SignatureController : Controller
    {
        APIService apiService;
        private readonly IDbContextFactory<MyDbContext> _db;
        public SignatureController(IDbContextFactory<MyDbContext> db)
        {
            _db = db;
            apiService = new APIService();
        }

        [HttpPost("register")]
        public bool register(APIService.Token token)
        {
            HttpContext.Session.SetString("token", token.Value);
            apiService.addToken(token);
            return true;
        }

        [HttpPost("register2")]
        public bool register2(string token)
        {

            return true;
        }

        [HttpPost("uploadImage")]
        public void uploadImage(ConstCheckSignature m)
        {
            using (var context = _db.CreateDbContext())
            {
                var checkFinding = context.constCheckSignatures.Where(r => r.ConstCheckSeq == m.ConstCheckSeq).FirstOrDefault();
                if(checkFinding != null)
                {
                    m.CreateTime = DateTime.Now;
                    context.constCheckSignatures.Add(m);
                }
                else
                {
                    m.ModifyTime = DateTime.Now;
                    context.Entry(checkFinding).CurrentValues.SetValues(m);
                }
                context.SaveChanges();
            }
        }


    }
}
