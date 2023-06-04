using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;
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

    public class ImaginaPay : System.Web.Services.WebService
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml)] // Establecer el formato de respuesta en XML
        public string CreatePayment(decimal total, long pedido_id, long metodo_pago_id, string usuario_rut)
        {
            try
            {
                using (var dbContext = new Entities())
                {
                    // Obtener el Pedido, MétodoPago y Usuario correspondientes a los IDs proporcionados
                    PEDIDO pedido = dbContext.PEDIDO.FirstOrDefault(p => p.ID_PEDIDO == pedido_id);
                    METODO_PAGO metodoPago = dbContext.METODO_PAGO.FirstOrDefault(mp => mp.ID == metodo_pago_id);
                    USUARIO usuario = dbContext.USUARIO.FirstOrDefault(u => u.RUT == usuario_rut);

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

                        // Crear una estructura XML para la respuesta SOAP
                        XmlDocument responseXml = new XmlDocument();
                        XmlElement rootElement = responseXml.CreateElement("CreatePaymentResponse");
                        XmlElement statusElement = responseXml.CreateElement("Status");
                        statusElement.InnerText = "Success";
                        rootElement.AppendChild(statusElement);
                        responseXml.AppendChild(rootElement);

                        // Convertir la estructura XML en una cadena para su retorno
                        return responseXml.OuterXml;
                    }
                    else
                    {
                        ErrorDetails errorDetails = new ErrorDetails
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            Message = "No se encontraron los registros requeridos para crear la transacción."
                        };

                        // Serializar el objeto de error en XML
                        XmlSerializer serializer = new XmlSerializer(typeof(ErrorDetails));
                        using (StringWriter writer = new StringWriter())
                        {
                            serializer.Serialize(writer, errorDetails);
                            return writer.ToString();
                        }
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

                // Serializar el objeto de error en XML
                XmlSerializer serializer = new XmlSerializer(typeof(ErrorDetails));
                using (StringWriter writer = new StringWriter())
                {
                    serializer.Serialize(writer, errorDetails);
                    return writer.ToString();
                }
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
        public string GetPaymentDetails(string id)
        {
            try
            {
                using (var dbContext = new Entities())
                {
                    TRANSACCION transaccion = dbContext.TRANSACCION.FirstOrDefault(t => t.ID_TRANSACCION.Equals(id));

                    if (transaccion != null)
                    {
                        // Serializar el objeto de transacción en XML
                        XmlSerializer serializer = new XmlSerializer(typeof(TRANSACCION));
                        using (StringWriter writer = new StringWriter())
                        {
                            serializer.Serialize(writer, transaccion);
                            return writer.ToString();
                        }
                    }
                    else
                    {
                        ErrorDetails errorDetails = new ErrorDetails
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            Message = "No se encontraron registros de la transacción indicada."
                        };

                        // Serializar el objeto de error en XML
                        XmlSerializer serializer = new XmlSerializer(typeof(ErrorDetails));
                        using (StringWriter writer = new StringWriter())
                        {
                            serializer.Serialize(writer, errorDetails);
                            return writer.ToString();
                        }
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

                // Serializar el objeto de error en XML
                XmlSerializer serializer = new XmlSerializer(typeof(ErrorDetails));
                using (StringWriter writer = new StringWriter())
                {
                    serializer.Serialize(writer, errorDetails);
                    return writer.ToString();
                }
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Xml)]
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

                        // Serializar el objeto de error en XML
                        XmlSerializer serializer = new XmlSerializer(typeof(ErrorDetails));
                        using (StringWriter writer = new StringWriter())
                        {
                            serializer.Serialize(writer, errorDetails);
                            return null;
                        }
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

                // Serializar el objeto de error en XML
                XmlSerializer serializer = new XmlSerializer(typeof(ErrorDetails));
                using (StringWriter writer = new StringWriter())
                {
                    serializer.Serialize(writer, errorDetails);
                    return null;
                }
            }
        }
    }
}