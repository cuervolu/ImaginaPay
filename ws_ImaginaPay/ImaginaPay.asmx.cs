using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Services;
using ws_ImaginaPay.Models;

namespace ws_ImaginaPay
{
    /// <summary>
    /// Descripción breve de ImaginaPay
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class ErrorDetails
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
        public string StackTrace { get; set; }
    }

    public class CustomSoapException : Exception
    {
        public ErrorDetails ErrorDetails { get; }

        public CustomSoapException(string message, ErrorDetails errorDetails) : base(message)
        {
            ErrorDetails = errorDetails;
        }
    }

    public class ImaginaPay : System.Web.Services.WebService
    {
        [WebMethod]
        public int CreatePayment(decimal total, long pedido_id, long metodo_pago_id, long usuario_id)
        {
            try
            {
                using (var dbContext = new Entities())
                {
                    // Obtener el Pedido, MétodoPago y Usuario correspondientes a los IDs proporcionados
                    PEDIDO pedido = dbContext.PEDIDO.FirstOrDefault(p => p.ID_PEDIDO == pedido_id);
                    METODO_PAGO metodoPago = dbContext.METODO_PAGO.FirstOrDefault(mp => mp.ID == metodo_pago_id);
                    USUARIO usuario = dbContext.USUARIO.FirstOrDefault(u => u.ID == usuario_id);

                    if (pedido != null && metodoPago != null && usuario != null)
                    {
                        TRANSACCION transaccion = new TRANSACCION();
                        transaccion.PEDIDO = pedido;
                        transaccion.USUARIO = usuario;
                        transaccion.METODO_PAGO = metodoPago;
                        transaccion.TOTAL_TRANSACCION = total;
                        transaccion.APROBADO = true;
                        transaccion.FECHA = DateTime.Now;

                        // Guardar la nueva transacción en la base de datos
                        dbContext.TRANSACCION.Add(transaccion);
                        dbContext.SaveChanges();

                        // Convertir la estructura XML en una cadena para su retorno
                        return 1;
                    }
                    else
                    {
                        ErrorDetails errorDetails = new ErrorDetails
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            Message = "No se encontraron los registros requeridos para crear la transacción."
                        };

                        throw new CustomSoapException(errorDetails.Message, errorDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorDetails errorDetails = new ErrorDetails
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Se produjo un error al crear la transacción.",
                    ExceptionMessage = ex.Message,
                    StackTrace = ex.StackTrace
                };
                throw new CustomSoapException(errorDetails.Message, errorDetails);
            }
        }

        [WebMethod]
        public TransaccionDTO GetPaymentDetails(long id)
        {
            try
            {
                using (var dbContext = new Entities())
                {
                    TRANSACCION transaccion = dbContext.TRANSACCION.FirstOrDefault(t => t.ID_TRANSACCION == id);

                    if (transaccion != null)
                    {
                        TransaccionDTO transaccionDTO = new TransaccionDTO
                        {
                            ID_TRANSACCION = transaccion.ID_TRANSACCION,
                            TOTAL_TRANSACCION = transaccion.TOTAL_TRANSACCION,
                            PEDIDO_ID = transaccion.PEDIDO_ID,
                            APROBADO = transaccion.APROBADO,
                            FECHA = transaccion.FECHA,
                            METODO_PAGO_ID = transaccion.METODO_PAGO_ID,
                            USUARIO_ID = transaccion.USUARIO_ID
                        };

                        return transaccionDTO;
                    }
                    else
                    {
                        ErrorDetails errorDetails = new ErrorDetails
                        {
                            StatusCode = HttpStatusCode.NotFound,
                            Message = "No se encontraron registros de la transacción indicada."
                        };

                        throw new CustomSoapException(errorDetails.Message, errorDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorDetails errorDetails = new ErrorDetails
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Se produjo un error al buscar la transacción.",
                    ExceptionMessage = ex.Message,
                    StackTrace = ex.StackTrace
                };

                throw new CustomSoapException(errorDetails.Message, errorDetails);
            }
        }

        [WebMethod]
        public List<TransaccionDTO> GetPaymentHistory(string user_rut)
        {
            try
            {
                using (var dbContext = new Entities())
                {
                    List<TransaccionDTO> transacciones = dbContext.TRANSACCION
                    .Where(t => t.USUARIO.RUT == user_rut)
                    .Select(t => new TransaccionDTO
                    {
                        ID_TRANSACCION = t.ID_TRANSACCION,
                        TOTAL_TRANSACCION = t.TOTAL_TRANSACCION,
                        APROBADO = t.APROBADO,
                        FECHA = t.FECHA,
                        METODO_PAGO_ID = t.METODO_PAGO_ID,
                        USUARIO_ID = t.USUARIO_ID,
                        PEDIDO_ID = t.PEDIDO_ID
                    })
                    .ToList();

                    // Verificar si se encontraron transacciones para el usuario
                    if (transacciones.Any())
                    {
                        return transacciones;
                    }
                    else
                    {
                        // No se encontraron transacciones para el usuario
                        ErrorDetails errorDetails = new ErrorDetails
                        {
                            StatusCode = HttpStatusCode.NotFound,
                            Message = "No se encontraron transacciones para el usuario con RUT: " + user_rut
                        };

                        throw new CustomSoapException(errorDetails.Message, errorDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorDetails errorDetails = new ErrorDetails
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Se produjo un error al obtener las transacciones del usuario con RUT: " + user_rut,
                    ExceptionMessage = ex.Message,
                    StackTrace = ex.StackTrace
                };
                throw new CustomSoapException(errorDetails.Message, errorDetails);
            }
        }
    }
}