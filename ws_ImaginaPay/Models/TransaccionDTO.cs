namespace ws_ImaginaPay.Models
{
    public class TransaccionDTO
    {
        public long ID_TRANSACCION { get; set; }
        public decimal TOTAL_TRANSACCION { get; set; }
        public long PEDIDO_ID { get; set; }
        public bool APROBADO { get; set; }
        public System.DateTime FECHA { get; set; }
        public long METODO_PAGO_ID { get; set; }
        public long USUARIO_ID { get; set; }
    }
}