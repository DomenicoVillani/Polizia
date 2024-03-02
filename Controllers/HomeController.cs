using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Polizia.Models;
using System.Diagnostics;

namespace Polizia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Verbale> verbali = new List<Verbale>();

            try
            {
                DB.conn.Open();
                var cmd = new SqlCommand("SELECT * FROM Verbale AS Verb JOIN TipoViolazione AS TipoViol ON Verb.IDViolazione = TipoViol.IDViolazione", DB.conn);
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var violazione = new Verbale()
                        {
                            IDVerbale = (int)reader["IDVerbale"],
                            DataViolazione = (DateTime)reader["DataViolazione"],
                            IndirizzoViolazione = reader["IndirizzoViolazione"].ToString(),
                            Nominativo_Agente = reader["Nominativo_Agente"].ToString(),
                            DataTrascrizioneVerbale = (DateTime)reader["DataTrascrizioneVerbale"],
                            Importo = (decimal)reader["Importo"],
                            DecurtamentoPunti = (int)reader["DecurtamentoPunti"],
                            IDAnagrafica = (int)reader["IDAnagrafica"],
                            Descrizione = reader["Descrizione"].ToString(),
                        };
                        verbali.Add(violazione);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                return View("Error");
            }
            finally
            {
                DB.conn.Close();
            }

            return View(verbali);
        }


        [HttpGet]
        public IActionResult Anagrafica([FromRoute] int? id)
        {
            var utente = new Anagrafica();
            if (id.HasValue)
            {
                try
                {
                    DB.conn.Open();
                    var cmd = new SqlCommand("SELECT * FROM Anagrafica WHERE IDAnagrafica=@IDAnagrafica", DB.conn);
                    cmd.Parameters.AddWithValue("@IDAnagrafica", id);
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        utente.IDAnagrafica = (int)reader["IDAnagrafica"];
                        utente.Cognome = reader["Cognome"].ToString();
                        utente.Nome = reader["Nome"].ToString();
                        utente.Indirizzo = reader["Indirizzo"].ToString();
                        utente.Citta = reader["Citta"].ToString();
                        utente.CAP = reader["CAP"].ToString();
                        utente.Cod_Fisc = reader["Cod_Fisc"].ToString();
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    return View("Error");
                }
                finally
                {
                    DB.conn.Close();
                }
                return View(utente);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Add()
        {
            List<Anagrafica> utenti = new List<Anagrafica>();
            List<TipoViolazione> violazioni = new List<TipoViolazione>();

            try
            {
                DB.conn.Open();
                var cmdViolazioni = new SqlCommand("SELECT * FROM TipoViolazione", DB.conn);
                var readerViolazioni = cmdViolazioni.ExecuteReader();

                if (readerViolazioni.HasRows)
                {
                    while (readerViolazioni.Read())
                    {
                        var violazione = new TipoViolazione()
                        {
                            IDViolazione = (int)readerViolazioni["IDViolazione"],
                            Descrizione = readerViolazioni["Descrizione"].ToString()
                        };
                        violazioni.Add(violazione);
                    }
                    readerViolazioni.Close();
                }

                var cmdUtenti = new SqlCommand("SELECT * FROM Anagrafica", DB.conn);
                var readerUtenti = cmdUtenti.ExecuteReader();

                if (readerUtenti.HasRows)
                {
                    while (readerUtenti.Read())
                    {
                        var utente = new Anagrafica()
                        {
                            IDAnagrafica = (int)readerUtenti["IDAnagrafica"],
                            Nome = readerUtenti["Nome"].ToString(),
                            Cognome = readerUtenti["Cognome"].ToString(),
                            Indirizzo = readerUtenti["Indirizzo"].ToString(),
                            Citta = readerUtenti["Citta"].ToString(),
                            CAP = readerUtenti["CAP"].ToString(),
                            Cod_Fisc = readerUtenti["Cod_Fisc"].ToString()
                        };
                        utenti.Add(utente);
                    }
                    readerUtenti.Close();
                }
            }
            catch (Exception ex)
            {
                return View("Error");
            }
            finally
            {
                DB.conn.Close();
            }
            ViewBag.Violazioni = violazioni;
            ViewBag.Utenti = utenti;
            return View();
        }
        [HttpPost]
        public IActionResult Add(Verbale verbale)
        {
            var error = true;
            try
            {
                DB.conn.Open();
                var cmd = new SqlCommand(@"INSERT INTO Verbale 
                                        (DataViolazione, IndirizzoViolazione, Nominativo_Agente, DataTrascrizioneVerbale, Importo, DecurtamentoPunti, IDAnagrafica, IDViolazione)
                                        VALUES (@DataViolazione, @IndirizzoViolazione, @Nominativo_Agente, @DataTrascrizioneVerbale, @Importo, @DecurtamentoPunti, @IDAnagrafica, @IDViolazione)", DB.conn);
                cmd.Parameters.AddWithValue("@DataViolazione", verbale.DataViolazione);
                cmd.Parameters.AddWithValue("@IndirizzoViolazione", verbale.IndirizzoViolazione);
                cmd.Parameters.AddWithValue("@Nominativo_Agente", verbale.Nominativo_Agente);
                cmd.Parameters.AddWithValue("@DataTrascrizioneVerbale", verbale.DataTrascrizioneVerbale);
                cmd.Parameters.AddWithValue("@Importo", verbale.Importo);
                cmd.Parameters.AddWithValue("@DecurtamentoPunti", verbale.DecurtamentoPunti);
                cmd.Parameters.AddWithValue("@IDAnagrafica", verbale.IDAnagrafica);
                cmd.Parameters.AddWithValue("@IDViolazione", verbale.IDViolazione);
                var rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    error = false;
                }
            }
            catch (Exception ex)
            {
                return View("Error");
            }
            finally
            {
                DB.conn.Close();
            }
            if (!error)
            {
                TempData["MessageSuccess"] = "Il verbale è stato aggiunto";
                return RedirectToAction("Index");
            }
            TempData["MessageError"] = "C'è stato un problema durante l'aggiornamento del DB";
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult AddAnagrafica()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddAnagrafica(Anagrafica anagrafica)
        {
            var error = true;
            try
            {
                DB.conn.Open();
                var cmd = new SqlCommand(@"INSERT INTO Anagrafica 
                                        (Cognome, Nome, Indirizzo, Citta, CAP, Cod_Fisc)
                                        VALUES (@Cognome, @Nome, @Indirizzo, @Citta, @CAP, @Cod_Fisc)", DB.conn);
                cmd.Parameters.AddWithValue("@Cognome", anagrafica.Cognome);
                cmd.Parameters.AddWithValue("@Nome", anagrafica.Nome);
                cmd.Parameters.AddWithValue("@Indirizzo", anagrafica.Indirizzo);
                cmd.Parameters.AddWithValue("@Citta", anagrafica.Citta);
                cmd.Parameters.AddWithValue("@CAP", anagrafica.CAP);
                cmd.Parameters.AddWithValue("@Cod_Fisc", anagrafica.Cod_Fisc);
                var rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    error = false;
                }
            }
            catch (Exception ex)
            {
                return View("Error");
            }
            finally
            {
                DB.conn.Close();
            }
            if (!error)
            {
                TempData["MessageSuccess"] = $"Il criminale {anagrafica.Nome} {anagrafica.Cognome} è stato aggiunto";
                return RedirectToAction("Index");
            }
            TempData["MessageError"] = "C'è stato un problema durante l'aggiornamento del DB";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Anagrafica anagrafica = null;
            try
            {
                DB.conn.Open();
                var cmd = new SqlCommand("SELECT * FROM Anagrafica WHERE IDAnagrafica = @id", DB.conn);
                cmd.Parameters.AddWithValue("@id", id);
                var rows = cmd.ExecuteReader();
                if (rows.HasRows)
                {
                    rows.Read();
                    anagrafica = new Anagrafica
                    {
                        IDAnagrafica = (int)rows["IDAnagrafica"],
                        Cognome = rows["Cognome"].ToString(),
                        Nome = rows["Nome"].ToString(),
                        Indirizzo = rows["Indirizzo"].ToString(),
                        Citta = rows["Citta"].ToString(),
                        CAP = rows["CAP"].ToString(),
                        Cod_Fisc = rows["Cod_Fisc"].ToString(),
                    };
                }
                rows.Close();
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
            finally
            {
                DB.conn.Close();
            }
            return View(anagrafica);
        }

        [HttpPost]
        public IActionResult Edit(Anagrafica anagrafica)
        {
            try
            {
                DB.conn.Open();
                var cmd = new SqlCommand(@"UPDATE Anagrafica SET Cognome=@Cognome, Nome=@Nome, Indirizzo=@Indirizzo,
                                Citta=@Citta, CAP=@CAP, Cod_Fisc=@Cod_Fisc WHERE IDAnagrafica=@id", DB.conn);
                cmd.Parameters.AddWithValue("@id", anagrafica.IDAnagrafica);
                cmd.Parameters.AddWithValue("@Cognome", anagrafica.Cognome);
                cmd.Parameters.AddWithValue("@Nome", anagrafica.Nome);
                cmd.Parameters.AddWithValue("@Indirizzo", anagrafica.Indirizzo);
                cmd.Parameters.AddWithValue("@Citta", anagrafica.Citta);
                cmd.Parameters.AddWithValue("@CAP", anagrafica.CAP);
                cmd.Parameters.AddWithValue("@Cod_Fisc", anagrafica.Cod_Fisc);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
            finally
            {
                DB.conn.Close();
            }
            return RedirectToAction("Anagrafica", new { id = anagrafica.IDAnagrafica });
        }

        [HttpGet]
        public IActionResult EditMulta(int id)
        {
            Verbale verbale = null;
            try
            {
                DB.conn.Open();
                var cmd = new SqlCommand("SELECT * FROM Verbale WHERE  IDVerbale = @id", DB.conn);
                cmd.Parameters.AddWithValue("@id", id);
                var rows = cmd.ExecuteReader();
                if (rows.HasRows)
                {
                    rows.Read();
                    verbale = new Verbale
                    {
                        IDVerbale = (int)rows["IDVerbale"],
                        DataViolazione = (DateTime)rows["DataViolazione"],
                        IndirizzoViolazione = rows["IndirizzoViolazione"].ToString(),
                        Nominativo_Agente = rows["Nominativo_Agente"].ToString(),
                        Importo = (decimal)rows["Importo"],
                        DecurtamentoPunti = (int)rows["DecurtamentoPunti"],
                    };
                }
                rows.Close();
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
            finally
            {
                DB.conn.Close();
            }
            return View(verbale);
        }

        [HttpPost]
        public IActionResult EditMulta(Verbale verbale)
        {
            try
            {
                DB.conn.Open();
                var cmd = new SqlCommand(@"UPDATE Verbale
                        SET
                        Verbale.IndirizzoViolazione = @IndirizzoViolazione,
                        Verbale.Nominativo_Agente = @Nominativo_Agente,
                        Verbale.Importo = @Importo,
                        Verbale.DecurtamentoPunti = @DecurtamentoPunti
                        WHERE Verbale.IDVerbale = @id;
                        ", DB.conn);
                cmd.Parameters.AddWithValue("@id", verbale.IDVerbale);
                cmd.Parameters.AddWithValue("@IndirizzoViolazione", verbale.IndirizzoViolazione);
                cmd.Parameters.AddWithValue("@Nominativo_Agente", verbale.Nominativo_Agente);
                cmd.Parameters.AddWithValue("@Importo", verbale.Importo);
                cmd.Parameters.AddWithValue("@DecurtamentoPunti", verbale.DecurtamentoPunti);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
            finally
            {
                DB.conn.Close();
            }
            return RedirectToAction("Index");
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
