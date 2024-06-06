using System.Data;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace WebAPI_TwoFactor.Clases
{
    public class Usuarios
    {

        //Cadena de conexion a la base de datos 
        public Usuarios()
        {
            //Aqui va la cadena de conexion del servidor SQL Server
            //Ejemplo: Data Source=NombredelServidorLocal;Initial Catalog=NombreDeLaDB;Integrated Security=True
            AccesoDatos.cadenaConexion = @"";
        }

        #region SetSecret
        //Metodo para actualizar el campo 2FA de la tabla Usuarios donde el email sea igual a el correo 
        public void SetSecret(string email, string code, string aplicacion)
        {

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@email", email),
                new SqlParameter("@etiqueta", aplicacion),
                new SqlParameter("code", code) 
            };

            AccesoDatos.ExecuteStoredProcedure("SP_CREATE_CODE", parameters);
        }
        #endregion
        #region GetSecret
        //Metodo para obtener el token de 2FA
        public string GetSecret(string email,string aplicacion)
        {
            DataTable dt = AccesoDatos.GetTmpDataTable($"SELECT Token FROM dbo.TwoFactorSecret WHERE Etiqueta = '{aplicacion}' and IdUsuario = (SELECT Id FROM Usuario WHERE Email = '{email}')");
            if (dt.Rows.Count == 1)
            {
                return dt.Rows[0]["Token"].ToString();
            }
            else
            {
                return "";
            }
        }
        #endregion
    }
}
