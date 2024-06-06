using Microsoft.AspNetCore.Mvc;
using TwoFactorAuthNet;
using TwoFactorAuthNet.Providers.Qr;
using WebAPI_TwoFactor.Clases;

namespace WebAPI_TwoFactor.Controllers
{
    public class TwoFactorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        #region QR HTML
        //Genera un codigo QR y a traves de una etiqueta HTML
        [HttpGet, Route("GetQRCode")]
        public string GetQRCode(string email, string Aplicacion)
        {
            var tfa = new TwoFactorAuth(Aplicacion, 6, 30, Algorithm.SHA1, new ImageChartsQrCodeProvider());
            var secret = tfa.CreateSecret(160);

            Usuarios usu = new Usuarios();
            usu.SetSecret(email, secret, Aplicacion);
            usu = null;

            string imgQR = tfa.GetQrCodeImageAsDataUri(email, secret);
            string imgHTML = $"<img src='{imgQR}'>";
            return imgHTML;
        }
        #endregion
        #region QR Swagger
        //Genera un codigo QR y lo muestra en Swagger (prueba)
        [HttpGet, Route("GetQRCodeAsImage")]
        public FileContentResult GetQRCodeAsImage(string email, string aplicacion)
        {
            var tfa = new TwoFactorAuth(aplicacion, 6, 30, Algorithm.SHA1, new ImageChartsQrCodeProvider());
            var secret = tfa.CreateSecret(160);

            Usuarios usu = new Usuarios();
            usu.SetSecret(email, secret, aplicacion);
            usu = null;
            string imgQR = tfa.GetQrCodeImageAsDataUri(email, secret);
            imgQR = imgQR.Replace("data:image/png;base64,", "");
            byte[] picture = Convert.FromBase64String(imgQR);
            return File(picture, "image/png");
        }
        #endregion
        #region Validar QR
        //Valida el codigo de autenticacion con el usuario
        [HttpGet, Route("ValidarQRCode")]
        public bool ValidarCodigo(string email, string aplicacion, string code)
        {
            Usuarios usu = new Usuarios();
            string secret = usu.GetSecret(email, aplicacion);
            usu = null;

            var tfa = new TwoFactorAuth("MiCodigo {IdAplicacion}", 6, 30, Algorithm.SHA1);
            return tfa.VerifyCode(secret, code, 2);
        }
        #endregion

    }
}
