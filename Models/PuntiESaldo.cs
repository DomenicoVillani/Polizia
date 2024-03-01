namespace Polizia.Models
{
    public class PuntiESaldo
    {
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string IndirizzoViolazione { get; set; }
        public DateTime DataViolazione { get; set; }
        public decimal Importo { get; set; }
        public int DecurtamentoPunti { get; set; }
        public string Cod_Fisc {  get; set; }
        public string Nominativo_Agente { get; set; }
        public string IDAnagrafica { get; set; }

    }
}
