using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Polizia.Models;

namespace Polizia.Controllers
{
    public class QueryController : Controller
    {
        public IActionResult VerbaliTrasgre()
        {
            List<VerbaliTrasgre> verbali = new List<VerbaliTrasgre>();

            try
            {
                DB.conn.Open();
                var cmd = new SqlCommand("SELECT Verb.IDAnagrafica, a.Cognome, a.Nome, COUNT(*) as TotaleVerbali FROM Verbale AS Verb JOIN Anagrafica AS a ON Verb.IDAnagrafica = a.IDAnagrafica GROUP BY Verb.IDAnagrafica, a.Cognome, a.Nome", DB.conn);
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var verbale = new VerbaliTrasgre()
                        {
                            Totale = (int)reader["TotaleVerbali"],
                            IDAnagrafica = (int)reader["IDAnagrafica"],
                            Cognome = reader["Cognome"].ToString(),
                            Nome = reader["Nome"].ToString(),
                        };
                        verbali.Add(verbale);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
            finally
            {
                DB.conn.Close();
            }
            return View(verbali);
        }

        public IActionResult TrasgrePunti()
        {
            List<VerbaliTrasgre> verbali = new List<VerbaliTrasgre>();

            try
            {
                DB.conn.Open();
                var cmd = new SqlCommand("SELECT Verb.IDAnagrafica, a.Cognome, a.Nome, SUM(DecurtamentoPunti) as TotPunti FROM Verbale AS Verb JOIN Anagrafica AS a ON Verb.IDAnagrafica = a.IDAnagrafica GROUP BY Verb.IDAnagrafica, a.Cognome, a.Nome", DB.conn);
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var verbale = new VerbaliTrasgre()
                        {
                            Totale = (int)reader["TotPunti"],
                            IDAnagrafica = (int)reader["IDAnagrafica"],
                            Cognome = reader["Cognome"].ToString(),
                            Nome = reader["Nome"].ToString(),
                        };
                        verbali.Add(verbale);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
            finally
            {
                DB.conn.Close();
            }
            return View(verbali);
        }

        public IActionResult DieciOPiu()
        {
            List<PuntiESaldo> verbali = new List<PuntiESaldo>();

            try
            {
                DB.conn.Open();
                var cmd = new SqlCommand("SELECT * FROM Verbale AS Verb JOIN Anagrafica AS a ON Verb.IDAnagrafica = a.IDAnagrafica WHERE DecurtamentoPunti>10", DB.conn);
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var verbale = new PuntiESaldo()
                        {
                            Cognome = reader["Cognome"].ToString(),
                            Nome = reader["Nome"].ToString(),
                            IndirizzoViolazione = reader["IndirizzoViolazione"].ToString(),
                            DataViolazione = (DateTime)reader["DataViolazione"],
                            Importo = (decimal)reader["Importo"],
                            Cod_Fisc = reader["Cod_Fisc"].ToString(),
                            Nominativo_Agente = reader["Nominativo_Agente"].ToString(),
                            IDAnagrafica= reader["IDAnagrafica"].ToString(),
                            DecurtamentoPunti = (int)reader["DecurtamentoPunti"],
                        };
                        verbali.Add(verbale);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
            finally
            {
                DB.conn.Close();
            }
            return View(verbali);
        }

        public IActionResult Saldo()
        {
            List<PuntiESaldo> verbali = new List<PuntiESaldo>();

            try
            {
                DB.conn.Open();
                var cmd = new SqlCommand("SELECT * FROM Verbale AS Verb JOIN Anagrafica AS a ON Verb.IDAnagrafica = a.IDAnagrafica WHERE Importo>400", DB.conn);
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var verbale = new PuntiESaldo()
                        {
                            Cognome = reader["Cognome"].ToString(),
                            Nome = reader["Nome"].ToString(),
                            IndirizzoViolazione = reader["IndirizzoViolazione"].ToString(),
                            DataViolazione = (DateTime)reader["DataViolazione"],
                            Importo = (decimal)reader["Importo"],
                            Cod_Fisc = reader["Cod_Fisc"].ToString(),
                            Nominativo_Agente = reader["Nominativo_Agente"].ToString(),
                            IDAnagrafica = reader["IDAnagrafica"].ToString(),
                            DecurtamentoPunti = (int)reader["DecurtamentoPunti"],
                        };
                        verbali.Add(verbale);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
            finally
            {
                DB.conn.Close();
            }
            return View(verbali);
        }
    }
}
