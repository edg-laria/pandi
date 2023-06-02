using Negocio.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAC.Atributos;
using SAC.Models;
using AutoMapper;
using Negocio.Modelos;
using System.Globalization;
using System.Text;
using SAC.Models.Afip;
using OfficeOpenXml;
using System.IO;
using SAC.Helpers;
using System.Data;
using ClosedXML.Excel;

namespace SAC.Controllers
{
    public class ClienteController : BaseController
    {

        private ServicioCliente oServicioCliente = new ServicioCliente();
        private ServicioClienteDireccion oServicioClienteDireccion = new ServicioClienteDireccion();
        private ServicioTipoCliente OservicioTipoCliente = new ServicioTipoCliente();
        private ServicioPieNota OservicioPieNota = new ServicioPieNota();
        private ServicioTipoMoneda OservicioTipoMoneda = new ServicioTipoMoneda();
        private ServicioGrupoPresupuesto OservicioGrupoPresupuesto = new ServicioGrupoPresupuesto();
        private ServicioPais OservicioPais = new ServicioPais();
        private ServicioFacturaVenta servicioFacturaVenta = new ServicioFacturaVenta();
        private ServicioArticulo servicioArticulo = new ServicioArticulo();
        private ServicioDepartamento servicioDepartamento = new ServicioDepartamento();

        public ClienteController()
        {
            oServicioCliente._mensaje = (msg_, tipo_) => CrearTempData(msg_, tipo_);
            oServicioClienteDireccion._mensaje = (msg_, tipo_) => CrearTempData(msg_, tipo_);
        }


        // GET: Usuario
        public ActionResult Index()
        {
            ClienteModelView model = new ClienteModelView();
            model.CVisible = false;
            CargarTipoCliente();
            return View(model);
        }


        [HttpPost]
        public ActionResult Index(int idTipoCliente, string CNombreCliente)
        {

            ClienteModelView model = new ClienteModelView();
            if (idTipoCliente == 0)
            {
                model.ListaCliente = Mapper.Map<List<ClienteModel>, List<ClienteModelView>>(oServicioCliente.GetClientePorNombre(CNombreCliente));
            }
            if (idTipoCliente > 0)
            {
                model.ListaCliente = Mapper.Map<List<ClienteModel>, List<ClienteModelView>>(oServicioCliente.GetClientePorTipoCliente(idTipoCliente));
            }
            model.CVisible = true;
            CargarTipoCliente();
            return View(model);
        }

        public ActionResult AddOrEdit(int id = 0)
        {
            CargarTipoCliente();
            CargarTipoIva();
            CargarPieNota();
            CargarIdioma();
            CargarGrupoPresupuesto();
            CargarTipoMoneda();
            ClienteModelView model;
            if (id == 0)
            {
                model = new ClienteModelView();
            }
            else
            {
                model = Mapper.Map<ClienteModel, ClienteModelView>(oServicioCliente.GetClientePorId(id));
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult AddOrEdit(ClienteModelView model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CargarTipoCliente();
                    CargarTipoIva();
                    CargarPieNota();
                    CargarIdioma();
                    CargarGrupoPresupuesto();
                    CargarTipoMoneda();

                    var OUsuario = (UsuarioModel)System.Web.HttpContext.Current.Session["currentUser"];
                    model.IdUsuario = OUsuario.IdUsuario;

                    System.Web.HttpContext.Current.Session["idCliente"] = model.Id;

                    if (model.Id <= 0)
                    {
                        model = Mapper.Map<ClienteModel, ClienteModelView>(oServicioCliente.GuardarCliente(Mapper.Map<ClienteModelView, ClienteModel>(model)));
                    }
                    else
                    {
                        oServicioCliente.ActualizarCliente(Mapper.Map<ClienteModelView, ClienteModel>(model));
                    }

                    //  return RedirectToAction(nameof(Index));
                    return RedirectToAction("AddOrEdit", new { Id = model.Id });
                }

      
               
                return View(model);
            }
            catch (Exception)
            {
                return View(model);
            }
        }



        #region "Servicios Reportes"

        // 1 Cuenta Corriente Detalle


        public ActionResult CtaCteDetalle(int IdCliente = 0, string searchFecha = null)
        {
            CtaCteClienteModelView model = new CtaCteClienteModelView();
            DateTime fechaHasta = DateTime.Now;

            if (IdCliente != 0)
            {
                if (!string.IsNullOrEmpty(searchFecha))
                {
                    fechaHasta = DateTime.ParseExact(searchFecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                model.CtaCte = Mapper.Map<List<CobroFacturaModel>, List<CobroFacturaModelView>>(oServicioCliente.GetCtaCteDetalle(IdCliente, fechaHasta));
                model.cliente = Mapper.Map<ClienteModel, ClienteModelView>(oServicioCliente.GetClientePorId(IdCliente));
               //--------------
                var facturasImpagas = oServicioCliente.GetCbtImpagosClientePorId(IdCliente, fechaHasta);
                var Totales = facturasImpagas.GroupBy(c => new { c.Cliente.Codigo })
                                  .Select(c => new
                                  {
                                      TotalPesos = c.Sum(x => x.IdMoneda == 1 && x.Tipo != "P" ? x.Total : 0  ),
                                      TotalDolares = c.Sum(x => x.IdMoneda == 2 && x.Tipo != "P" ? x.Total : 0),
                                      TotalPesosCobros = c.Sum(x => x.IdMoneda == 1 && x.Tipo == "P" ? x.Total : 0),
                                      TotalDolaresCobros = c.Sum(x => x.IdMoneda == 2 && x.Tipo == "P" ? x.Total : 0),
                                      TotalParcialPesos = c.Sum(x => x.IdMoneda == 1 && x.Tipo != "P" ? x.Parcial : 0) ,
                                      TotalParcialDolares = c.Sum(x => x.IdMoneda == 2 && x.Tipo != "P" ? x.Parcial : 0),
                                      TotalSaldoPesos = c.Sum(x => x.IdMoneda == 1 ? x.Saldo : 0),
                                      TotalSaldoDolares = c.Sum(x => x.IdMoneda == 2  ? x.Saldo : 0),
                                  }).FirstOrDefault();

                model.TotalPesos = Totales.TotalPesos;
                model.TotalDolares = Totales.TotalDolares;
                model.TotalPesosCobros = Totales.TotalPesosCobros;
                model.TotalDolaresCobros = Totales.TotalDolaresCobros;
                model.TotalParcialPesos = Totales.TotalParcialPesos;
                model.TotalParcialDolares = Totales.TotalParcialDolares;
                model.TotalSaldoPesos = Totales.TotalSaldoPesos;
                model.TotalSaldoDolares = Totales.TotalSaldoDolares;

            }

            return View(model);

        }



        // 2 Cuenta Corriente Resumen
        public ActionResult CtaCteResumen(string searchFecha)
        {
            List<CteCteClienteResumenModelView> model = new List<CteCteClienteResumenModelView>();
            try
            {
                DateTime fechaHasta = DateTime.Now;

                if (!string.IsNullOrEmpty(searchFecha))

                {
                    fechaHasta = DateTime.ParseExact(searchFecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                }
                model = Mapper.Map<List<CteCteClienteResumenModel>, List<CteCteClienteResumenModelView>>(oServicioCliente.GetCtaCteResumen(fechaHasta));



            }
            catch (Exception ex)
            {
                oServicioCliente._mensaje(ex.Message, "error");
            }
            return View(model);
        }


        //3  Registro de Ventas Mensuales

        //4  Registro de Boton Resumen


        public ActionResult ConsultaIvaVentas(string Periodo = null, string Anio = null, string Mes = null)
        {
            ConsultaIvaVentaModelView model = new ConsultaIvaVentaModelView();
            model.ListaConsultaIva = null;
            model.ListaFacturaVentaIva = null;

            if (string.IsNullOrEmpty(Periodo))
            {
                Periodo = DateTime.Now.ToString("yyMM");
                }  
                
                model.Anio = Anio;
                model.MesNro = Mes;
                model.Periodo = Periodo;
                model.ListaConsultaIva = Mapper.Map<List<ConsultaIvaVentaModel>, List<ConsultaIvaVentaModelView>>(oServicioCliente.GetIvaVentas(Periodo));
                model.ConsultaIvaTotales = Mapper.Map<ConsultaIvaTotalesModel, ConsultaIvaTotalesModelView>(oServicioCliente.GetIvaVentasTotales(Periodo));
                model.ListaFacturaVentaIva = Mapper.Map<List<FacturaVentaIvaModel>, List<FacturaVentaIvaModelView>>(oServicioCliente.GetIvaImpresion(Periodo));
                model.ConsultaIvaTotales.Periodo = Mes + "/20" + Anio;
                Session["Datos"] = model.ListaFacturaVentaIva;
                Session["Periodo"] = Periodo;
         
            CargarAnio(Anio);
            CargarMes(Mes);
            return View(model);
        }


        [HttpGet()]
        public ActionResult GetFacturaJson(string idCliente, string idComprobante)
        {
            try
            {
                string strJson;

                FacturaVentaModel factura = servicioFacturaVenta.GetFacturaVentaPorId(int.Parse(idCliente), int.Parse(idComprobante));

                ImprimirFacturaModelView iFactura = new ImprimirFacturaModelView();

                iFactura.NombreComprobante = factura.TipoComprobanteVenta.Denominacion.ToString();

                iFactura.NombreComprobante = obtenerNombreCbt(factura.TipoComprobanteVenta.CodigoAfip, factura.Cliente.IdIdioma);

                iFactura.PuntoVenta = factura.FacturaElectronica.PUNTOVTA.ToString();
                iFactura.NumeroComprobante = factura.FacturaElectronica.NROCBTE_AFIP.ToString();

                iFactura.TipoComprobante = factura.TipoComprobanteVenta.CodigoAfip.ToString(); // factura.FacturaElectronica.ID_TIPOCBTE.ToString();
                //
                iFactura.LetraComprobante = factura.TipoComprobanteVenta.Abreviatura.Substring(factura.TipoComprobanteVenta.Abreviatura.Length - 1, 1);
                var f = (DateTime)(factura.FacturaElectronica.FECHACBTE != null ? factura.FacturaElectronica.FECHACBTE : DateTime.Now);
                iFactura.FechaEmision = f.ToString("dd/MM/yyyy");
                iFactura.OurRef = factura.ORef;
                iFactura.YourRef = factura.YRef;
                iFactura.RazonSocial = factura.FacturaElectronica.NOMBRE;
                iFactura.Domicilio = factura.FacturaElectronica.DOMICILIO;
                iFactura.CondicionIVA = factura.FacturaElectronica.CATEGORIA;
                iFactura.NumeroDocumentoReceptor = factura.FacturaElectronica.NRODOC;
                iFactura.TipoDocumentoReceptor = factura.FacturaElectronica.TIPODOC.ToString();

                List<ItemFactura> itemFacturas = new List<ItemFactura>();

                foreach (var i in factura.ItemImpre)
                {
                    ItemFactura item = new ItemFactura();
                    item.Nombre = i.Codigo + " " + i.Descripcion;
                    item.Precio = i.Precio;
                    itemFacturas.Add(item);
                }
                iFactura.Items = itemFacturas;
                var idDto = factura.IdDto ?? 0;
                iFactura.Dto = servicioDepartamento.GetDepartamentoPorId(idDto).Descripcion.ToString();
                iFactura.codAut = factura.FacturaElectronica.CAE;

                var VtoCAE = (DateTime)factura.FacturaElectronica.FECHAVTO;
                iFactura.FechaVtoCodAut = VtoCAE.ToString("dd/MM/yyyy");
                iFactura.ImporteTotal = factura.FacturaElectronica.TOTAL.ToString();
                if (factura.Cliente.IdIdioma == 1)
                {
                    MonedaNroStr oPesos = new MonedaNroStr();
                    iFactura.ImporteEnLetra = " $ Pesos " + MonedaNroStr.Convertir(factura.FacturaElectronica.TOTAL.ToString(), true);
                }
                else
                {
                    NumToStr oMoneda = new NumToStr();
                    iFactura.ImporteEnLetra = NumToStr.ConvertToWords(factura.FacturaElectronica.TOTAL.ToString());
                }

                iFactura.Header = factura.Concepto != null ? factura.Concepto : "";
                iFactura.Nota = factura.Cliente.PieNota.Nota;
                iFactura.QrBase64 = factura.FacturaElectronica.QR != null ? factura.FacturaElectronica.QR : "";

                strJson = Newtonsoft.Json.JsonConvert.SerializeObject(iFactura);
                if ((strJson != null))
                {
                    var rJson = Json(new { result = true, data = strJson }, JsonRequestBehavior.AllowGet);
                    return rJson;
                }
                return Json(new { result = true, data = strJson }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, data = "" }, JsonRequestBehavior.AllowGet);
            }

        }

        private string obtenerNombreCbt(int CodigoAfip, int IdIdioma)
        {
            if (CodigoAfip == 211 || CodigoAfip == 11 || CodigoAfip == 19)
            {
                return (IdIdioma == 1) ? "Factura" : "Invoice";
            }
            if (CodigoAfip == 212 || CodigoAfip == 12 || CodigoAfip == 20)
            {
                return (IdIdioma == 1) ? "Débito" : "Debit";
            }
            if (CodigoAfip == 213 || CodigoAfip == 13 || CodigoAfip == 21)
            {
                return (IdIdioma == 1) ? "Crédito " : "Credit";
            }
            return "Factura";
        }



        //  5. Boton Impresión

        //  BOTON IMPRESION

        // La Impresión tiene 2 partes. (IVA.PDF)

        //1.- Listado de todos los movimientos.
        //2.- Los totales
        //3.-Totales Locales y Exterior por mes se obtiene de la Tabla ITEMImpr.

        [HttpPost]
        public FileResult Export()
        {
            List<FacturaVentaIvaModelView> listResultado = new List<FacturaVentaIvaModelView>();


            DataTable dt = new DataTable("Grid");

            dt.Columns.AddRange(new DataColumn[14]
                                             {
                                             new DataColumn("Clase"),
                                             new DataColumn("Punto"),
                                             new DataColumn("NumeroFactura"),
                                             new DataColumn("Empresa"),
                                             new DataColumn("Neto"),
                                             new DataColumn("TotalIva"),
                                             new DataColumn("Gasto"),
                                             new DataColumn("Isib"),
                                             new DataColumn("Total"),
                                             new DataColumn("Cuit"),
                                             new DataColumn("Dolar"),
                                             new DataColumn("Moneda"),
                                             new DataColumn("Periodo"),
                                             new DataColumn("Asiento")

                                             });


            listResultado = Session["Datos"] as List<FacturaVentaIvaModelView>;

            string Periodo = "";
            Periodo = Session["Periodo"] as string;

            foreach (FacturaVentaIvaModelView i in listResultado)
            {

                dt.Rows.Add(i.Clase, i.Punto, i.NumeroFactura,  i.Empresa, i.Neto, i.TotalIva, i.Gasto, i.Isib, i.Total, i.Cuit, i.Dolar, i.Moneda, i.Periodo, i.Asiento);


            }




            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "IvaCLI" + Periodo + ".xlsx");
                }
            }
        }


        public void CargarAnio(string anio = null)
        {
            List<Anios> ListaAnio = new List<Anios>()
            {
                new Anios(){ Id = "0", Descripcion = "Selecionar" },
                new Anios(){ Id = "19", Descripcion = "2019" },
                new Anios(){ Id = "20", Descripcion = "2020" },
                new Anios(){ Id = "21", Descripcion = "2021" },
                new Anios(){ Id = "22", Descripcion = "2022" },
                new Anios(){ Id = "23", Descripcion = "2023" },
                new Anios(){ Id = "24", Descripcion = "2024" },
                new Anios(){ Id = "25", Descripcion = "2025" }

            };

            StringBuilder sb = new StringBuilder();
            foreach (var type in ListaAnio)
            {

                if (anio == type.Id)
                {
                    sb.Append("<option value='" + type.Id + "' selected>" + type.Descripcion + "</option>");

                }

                else

                {
                    sb.Append("<option value='" + type.Id + "'>" + type.Descripcion + "</option>");

                }




            }

            ViewBag.ListaAnio = sb.ToString();
        }

        public void CargarMes(string mes = null)
        {
            List<Meses> ListaMes = new List<Meses>()
            {
                new Meses(){ Id = "0", Descripcion = "Selecionar" },
                new Meses(){ Id = "01", Descripcion = "Enero" },
                new Meses(){ Id = "02", Descripcion = "Febrero" },
                new Meses(){ Id = "03", Descripcion = "Marzo" },
                new Meses(){ Id = "04", Descripcion = "Abril" },
                new Meses(){ Id = "05", Descripcion = "Mayo" },
                new Meses(){ Id = "06", Descripcion = "Junio" },
                new Meses(){ Id = "07", Descripcion = "Julio" },
                new Meses(){ Id = "08", Descripcion = "Agosto" },
                new Meses(){ Id = "09", Descripcion = "Septiembre" },
                new Meses(){ Id = "10", Descripcion = "Octubre" },
                new Meses(){ Id = "11", Descripcion = "Noviembre" },
                new Meses(){ Id = "12", Descripcion = "Diciembre" }};
            StringBuilder sb = new StringBuilder();

            foreach (var type in ListaMes)
            {
                if (mes == type.Id)
                {
                    sb.Append("<option value='" + type.Id + "' selected>" + type.Descripcion + "</option>");

                }

                else

                {
                    sb.Append("<option value='" + type.Id + "'>" + type.Descripcion + "</option>");

                }


            }
            ViewBag.ListaMes = sb.ToString();
        }

        #endregion



        #region "Direccion de los Clientes"

        public ActionResult AddOrEditDireccion(int IdCliente, int Id = 0)
        {

            CargarPieNota();
            CargarIdioma();
            CargarTipoMoneda();
            CargarPais();

            ClienteDireccionModelView model;

            if (Id == 0)
            {
                model = new ClienteDireccionModelView();
                model.IdCliente = IdCliente;
            }
            else
            {
                model = Mapper.Map<ClienteDireccionModel, ClienteDireccionModelView>(oServicioClienteDireccion.ObtenerPorID(Id));
            }
            CargarProvincia(model.IdPais ?? 0);
            CargarLocalidad(model.IdProvincia ?? 0);

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrEditDireccion(ClienteDireccionModelView model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var OUsuario = (UsuarioModel)System.Web.HttpContext.Current.Session["currentUser"];
                    model.IdUsuario = OUsuario.IdUsuario;
                    if (model.Id <= 0)
                    {
                        oServicioClienteDireccion.GuardarDireccion(Mapper.Map<ClienteDireccionModelView, ClienteDireccionModel>(model));
                    }
                    else
                    {
                        oServicioClienteDireccion.ActualizarDireccion(Mapper.Map<ClienteDireccionModelView, ClienteDireccionModel>(model));
                    }
                }
                return RedirectToAction("DireccionCliente", new { IdCliente = model.IdCliente });
            }
            catch (Exception)
            {
                return View(model);
            }
        }

        #endregion

        [HttpPost]
        public ActionResult Eliminar(int id)
        {
            try
            {
                oServicioCliente.Eliminar(id);

            }
            catch (Exception ex)
            {
                oServicioCliente._mensaje(ex.Message, "error");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult BloquearCliente(int id)
        {
            oServicioCliente.BloquearCliente(id);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult HabilitarCliente(int id)
        {
            try
            {


                oServicioCliente.HabilitarCliente(id);


            }
            catch (Exception ex)
            {
                oServicioCliente._mensaje(ex.Message, "error");


            }


            return RedirectToAction("Index");

        }

        [HttpPost]
        public ActionResult EliminarDireccion(int Id, int IdCliente = 0)
        {
            try
            {
                oServicioClienteDireccion.Eliminar(Id);

            }
            catch (Exception ex)
            {
                oServicioCliente._mensaje(ex.Message, "error");
            }
            return RedirectToAction("DireccionCliente", new { IdCliente = IdCliente });
        }


        public ActionResult DireccionCliente(int IdCliente)
        {

            ClienteDireccionModelView model = new ClienteDireccionModelView();

            model.ListaDireccion = Mapper.Map<List<ClienteDireccionModel>, List<ClienteDireccionModelView>>(oServicioClienteDireccion.GetDireccionPorcliente(IdCliente));
            model.Cliente = Mapper.Map<ClienteModel, ClienteModelView>(oServicioCliente.GetClientePorId(IdCliente));
            model.IdCliente = IdCliente;
            return View(model);

        }



        #region "Cargar Combos"



        //combo CargarTipoCliente
        public void CargarTipoCliente()
        {
            List<TipoClienteModelView> ListaTipoCliente = Mapper.Map<List<TipoClienteModel>, List<TipoClienteModelView>>(OservicioTipoCliente.GetAllTipoCliente());
            List<SelectListItem> retornoListaTipoCliente = null;
            retornoListaTipoCliente = (ListaTipoCliente.Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Descripcion
            })).ToList();
            retornoListaTipoCliente.Insert(0, new SelectListItem { Text = "Consultar Por Nombre", Value = "" });
            ViewBag.ListaTipoCliente = retornoListaTipoCliente;
        }


        //combo Tipo Iva
        public void CargarTipoIva()
        {
            ServicioTipoIva ServicioIva = new ServicioTipoIva();
            List<TipoIvaViewModel> ListaTipoIva = Mapper.Map<List<TipoIvaModel>, List<TipoIvaViewModel>>(ServicioIva.GetAllTipoIva());

            //esto es para pasarlo a select list (drop down list)
            List<SelectListItem> retornoListaTipoIva = null;
            retornoListaTipoIva = (ListaTipoIva.Select(x =>
                                  new SelectListItem()
                                  {
                                      Value = x.Id.ToString(),
                                      Text = x.Descripcion
                                  })).ToList();
            retornoListaTipoIva.Insert(0, new SelectListItem { Text = "--Seleccione el Tipo de Iva--", Value = "" });

            ViewBag.ListaTipoIva = retornoListaTipoIva;
        }


        //combo PieNota
        public void CargarPieNota()
        {
            ServicioPieNota Servicio = new ServicioPieNota();
            List<PieNotaModelView> ListaTipoIva = Mapper.Map<List<PieNotaModel>, List<PieNotaModelView>>(Servicio.GetAllPieNota());

            //esto es para pasarlo a select list (drop down list)
            List<SelectListItem> retornoListaPieNota = null;
            retornoListaPieNota = (ListaTipoIva.Select(x =>
                                  new SelectListItem()
                                  {
                                      Value = x.Id.ToString(),
                                      Text = x.Descripcion
                                  })).ToList();
            //retornoListaPieNota.Insert(0, new SelectListItem { Text = "--Seleccione el Pie de Nota--", Value = "" });

            ViewBag.ListaPieNota = retornoListaPieNota;
        }



        //combo Idioma
        public void CargarIdioma()
        {


            ServicioTipoIdioma Servicio = new ServicioTipoIdioma();
            List<TipoIdiomaModelView> Lista = Mapper.Map<List<TipoIdiomaModel>, List<TipoIdiomaModelView>>(Servicio.GetAllTipoIdioma());

            //esto es para pasarlo a select list (drop down list)
            List<SelectListItem> retornoLista = null;
            retornoLista = (Lista.Select(x =>
                                  new SelectListItem()
                                  {
                                      Value = x.Id.ToString(),
                                      Text = x.Descripcion
                                  })).ToList();
            retornoLista.Insert(0, new SelectListItem { Text = "--Seleccione el Idioma--", Value = "" });

            ViewBag.ListaTipoIdioma = retornoLista;
        }



        //combo Tipo Iva
        public void CargarTipoMoneda()
        {
            ServicioTipoMoneda Servicio = new ServicioTipoMoneda();
            List<TipoMonedaModelView> ListaTipoIva = Mapper.Map<List<TipoMonedaModel>, List<TipoMonedaModelView>>(Servicio.GetAllTipoMonedas());

            //esto es para pasarlo a select list (drop down list)
            List<SelectListItem> retornoListaTipoMoneda = null;
            retornoListaTipoMoneda = (ListaTipoIva.Select(x =>
                                  new SelectListItem()
                                  {
                                      Value = x.Id.ToString(),
                                      Text = x.Descripcion
                                  })).ToList();
            retornoListaTipoMoneda.Insert(0, new SelectListItem { Text = "--Seleccione la Moneda--", Value = "" });

            ViewBag.ListaTipoMoneda = retornoListaTipoMoneda;
        }



        //combo Tipo Iva
        public void CargarGrupoPresupuesto()
        {
            ServicioPresupuestoActual _servicio = new ServicioPresupuestoActual();
            List<PresupuestoActualModelView> Lista = Mapper.Map<List<PresupuestoActualModel>, List<PresupuestoActualModelView>>(_servicio.GetPresupuestoCliente());

            //esto es para pasarlo a select list (drop down list)
            List<SelectListItem> retornoLista = null;
            retornoLista = (Lista.Select(x =>
                                  new SelectListItem()
                                  {
                                      Value = x.Id.ToString(),
                                      Text = x.Concepto
                                  })).ToList();
            retornoLista.Insert(0, new SelectListItem { Text = "--Seleccione el Grupo de Presupuesto--", Value = "" });

            ViewBag.ListaGrupoPresupuesto = retornoLista;
        }


        //combo Pais
        public void CargarPais()
        {
            ServicioPais servicioPais = new ServicioPais();
            List<PaisModelView> ListaPais = Mapper.Map<List<PaisModel>, List<PaisModelView>>(servicioPais.GetAllPais());
            ListaPais = ListaPais.OrderBy(p => p.Nombre).ToList();

            //esto es para pasarlo a select list (drop down list)
            List<SelectListItem> retornoListaPais = null;
            retornoListaPais = (ListaPais.Select(x =>
                                  new SelectListItem()
                                  {
                                      Value = x.Id.ToString(),
                                      Text = x.Nombre
                                  })).ToList();
            retornoListaPais.Insert(0, new SelectListItem { Text = "-- Seleccione País --", Value = "" });
            ViewBag.ListaPais = retornoListaPais;
        }

        public void CargarProvincia(int oPais)
        {
            ServicioProvincia servicioProvincia = new ServicioProvincia();
            List<ProvinciaModelView> ListaProvincia = Mapper.Map<List<ProvinciaModel>, List<ProvinciaModelView>>(servicioProvincia.GetAllProvincias(oPais));
            ListaProvincia = ListaProvincia.OrderBy(p => p.Nombre).ToList();

            //esto es para pasarlo a select list (drop down list)
            List<SelectListItem> retornoListaProvincia = null;
            retornoListaProvincia = (ListaProvincia.Select(x =>
                                  new SelectListItem()
                                  {
                                      Value = x.Id.ToString(),
                                      Text = x.Nombre
                                  })).ToList();
            retornoListaProvincia.Insert(0, new SelectListItem { Text = "-- Seleccione Provincia --", Value = "" });
            ViewBag.ListaProvincia = retornoListaProvincia;
        }

        public void CargarLocalidad(int oProvincia)
        {
            ServicioLocalidad servicioLocalidad = new ServicioLocalidad();
            List<LocalidadModelView> ListaLocalidad = Mapper.Map<List<LocalidadModel>, List<LocalidadModelView>>(servicioLocalidad.GetAllLocalidads(oProvincia));
            ListaLocalidad = ListaLocalidad.OrderBy(p => p.Nombre).ToList();

            //esto es para pasarlo a select list (drop down list)
            List<SelectListItem> retornoListaLocalidad = null;
            retornoListaLocalidad = (ListaLocalidad.Select(x =>
                                  new SelectListItem()
                                  {
                                      Value = x.Id.ToString(),
                                      Text = x.Nombre
                                  })).ToList();
            retornoListaLocalidad.Insert(0, new SelectListItem { Text = "-- Seleccione Localidad --", Value = "" });
            ViewBag.ListaLocalidad = retornoListaLocalidad;
        }


        #endregion


    }






}



