﻿using Negocio.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAC.Atributos;
using SAC.Models;
using AutoMapper;
using Negocio.Modelos;
using System.Text;
using Entidad.Models;
using System.Web.Script.Serialization;

namespace SAC.Controllers
{
    public class ContabilidadController : BaseController
    {

        private ServicioImputacion servicioimputacion = new ServicioImputacion();
        public PlanContableModelView planContableModelView = new PlanContableModelView();
        private String JsonTreeView;
        private JavaScriptSerializer jsonString = new JavaScriptSerializer();


        public ContabilidadController()
        {
            servicioimputacion._mensaje = (msg_, tipo_) => CrearTempData(msg_, tipo_);
        }

        /// Consulta asientos contables"
        /// 
        public ActionResult ConsultaAsientosContables(string Periodo = null, string Tipo = null)
        {
            DiarioModelView model = new DiarioModelView();
            if (string.IsNullOrEmpty(Periodo))
            {
                Periodo = DateTime.Now.ToString("yyMM");
            }  
            Tipo = (!string.IsNullOrEmpty(Tipo)) ? Tipo : "CF";
            model.ListaDiario = Mapper.Map<List<DiarioModel>, List<DiarioModelView>>(servicioimputacion.GetAsientosContables(Periodo, Tipo));
              
           
            model.TipoAsiento = TipoAsiento();
            model.Periodo = Periodo;
            model.Tipo = Tipo;
            CargarAnio();
            CargarMes();
            return View(model);
        }

        [HttpGet()]
        public ActionResult GetDetallesAsientosContables(string cuenta = null,string periodo = null, string tipo = null)
        {

            try
            {
                ///ak realizar la busqueda por idcta del numero de cheque  
                ///
                List<DiarioModelView> diarioModelViews = new List<DiarioModelView>();
                int IdImputacion = 0;
                if (!string.IsNullOrEmpty(cuenta))
                {
                    IdImputacion = int.Parse(cuenta);
                    tipo = (!string.IsNullOrEmpty(tipo)) ? tipo : "CF";
                    periodo = (!string.IsNullOrEmpty(periodo)) ? periodo : DateTime.Now.ToString("yy") + DateTime.Now.ToString("dd");
              
                    diarioModelViews = Mapper.Map<List<DiarioModel>, List<DiarioModelView>>(servicioimputacion.GetAsientoContableDetalles(IdImputacion, periodo, tipo));
                }
           
                 return Json(new { result = true, data = Newtonsoft.Json.JsonConvert.SerializeObject(diarioModelViews) }, JsonRequestBehavior.AllowGet);
               // return Json(new { result = false, data = "Ops!, A ocurriodo un error. Contacte al Administrador" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, data = "Ops!, A ocurriodo un error. Contacte al Administrador" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// Consulta Suma y saldos"
        /// 
        public ActionResult ConsultaSumaYSaldos(string Periodo = null, string Tipo = "CF")
        {
            DiarioModelView model = new DiarioModelView();
            if (!string.IsNullOrEmpty(Periodo))
            {
                model.ListaDiario = Mapper.Map<List<DiarioModel>, List<DiarioModelView>>(servicioimputacion.GetAsientosContables(Periodo, Tipo));
            }
            model.TipoAsiento = TipoAsiento();
            CargarAnio();
            CargarMes();
            return View(model);
        }




        public ActionResult DiarioCompraFactura(string Periodo = null)
        {
            DiarioModelView model = new DiarioModelView();
            if (!string.IsNullOrEmpty(Periodo))
            {
                model.ListaDiario = Mapper.Map<List<DiarioModel>, List<DiarioModelView>>(servicioimputacion.GetCompraFactura(Periodo));
            }
            model.TipoAsiento = TipoAsiento();
            CargarAnio();
            CargarMes();
            return View(model);
        }

        public ActionResult DiarioCompraPagos(string Periodo = null)
        {
            DiarioModelView model = new DiarioModelView();
            if (!string.IsNullOrEmpty(Periodo))
            {
                model.ListaDiario = Mapper.Map<List<DiarioModel>, List<DiarioModelView>>(servicioimputacion.GetCompraFactura(Periodo));
            }
            CargarAnio();
            CargarMes();
            return View(model);
        }

        private SelectList TipoAsiento()
        {
            var diccionario = servicioimputacion.GetTipoAsiento();
            return new SelectList(diccionario, "Key", "Value");
        }


        private void CargarMes()
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
                sb.Append("<option value='" + type.Id + "'>" + type.Descripcion + "</option>");
            }
            ViewBag.ListaMes = sb.ToString();
        }

        private void CargarAnio()
        {
            List<Anios> ListaAnio = new List<Anios>()
            {
                new Anios(){ Id = "0", Descripcion = "Selecionar" },
                new Anios(){ Id = "19", Descripcion = "2019" },
                new Anios(){ Id = "20", Descripcion = "2020" },
                new Anios(){ Id = "21", Descripcion = "2021" },
                new Anios(){ Id = "22", Descripcion = "2022" },
                new Anios(){ Id = "23", Descripcion = "2023" },
                new Anios(){ Id = "24", Descripcion = "2024" }};

            StringBuilder sb = new StringBuilder();
            foreach (var type in ListaAnio)
            {
                sb.Append("<option value='" + type.Id + "'>" + type.Descripcion + "</option>");
            }
            ViewBag.ListaAnio = sb.ToString();
        }


        //---------------- PLAN CONTABLE --------

        // GET: 
        //[AutorizacionDeSistema]
        public ActionResult Index()
        {


            CuentaPlanContableModelView nuevaCuenta = new CuentaPlanContableModelView();
            nuevaCuenta.TipoElemento = GetTipoElemento();
            nuevaCuenta.selectListCuentaSuperior = GetCuentaSuperior(0);

            planContableModelView = new PlanContableModelView
            {
                IEmenuSideBar = Mapper.Map<List<MenuSideBarModel>, List<MenuSideBarModelView>>(servicioimputacion.GetPlanContable()),
                cuentaContable = nuevaCuenta,
            };
            JsonTreeView = jsonString.Serialize(TreeViewNode(servicioimputacion.GetPlanContable()));
            ViewBag.JsonMenuSider = JsonTreeView;          
            return View(planContableModelView);
        }

        public String TreeView(List<MenuSideBarModel> model)
        {
            List<TreeViewModel> ListTreeView = new List<TreeViewModel>();
            foreach (var i in model)
            {
                TreeViewModel item = new TreeViewModel();
                item.text = i.Titulo;
                item.href = "/Contabilidad/Edit/" + i.IdMenuSidebar.ToString();
                if (i.Group.Count > 0)
                {
                    List<TreeViewModel> ListNode = new List<TreeViewModel>();
                    foreach (var n in i.Group)
                    {
                        TreeViewModel nodo = new TreeViewModel();
                        nodo.text = n.Titulo;
                        nodo.href = "/Contabilidad/Edit/" + n.IdMenuSidebar.ToString();
                        ListNode.Add(nodo);
                    }
                    item.nodes = ListNode;
                }
                ListTreeView.Add(item);
            }
            JsonTreeView += jsonString.Serialize(ListTreeView);
            return JsonTreeView;
        }

        public List<TreeViewModel> TreeViewNode(ICollection<MenuSideBarModel> model)
        {
            List<TreeViewModel> ListTreeView = new List<TreeViewModel>();
            foreach (var i in model)
            {
                TreeViewModel item = new TreeViewModel();
                item.text = i.Titulo;
                item.href = "/Contabilidad/Edit/" + i.Url + i.IdMenuSidebar.ToString();
                if (i.Group.Count > 0)
                {
                    item.nodes = TreeViewNode(i.Group);
                }
                ListTreeView.Add(item);
            }

            return ListTreeView;
        }

        private SelectList GetTipoElemento()
        {
            var diccionario = servicioimputacion.GetTipoElemento();
            return new SelectList(diccionario, "Key", "Value");
        }

        public ActionResult Edit(string id = null)
        {
            CuentaPlanContableModelView model = new CuentaPlanContableModelView();
            model.TipoElemento = GetTipoElemento();

            if (id != null)
            {
                //edit
                string[] param = id.Split('-');
                int TipoCuenta = int.Parse(param[0]);
                int IdCuenta = int.Parse(param[1]);
                switch (TipoCuenta)
                {
                    case 0:

                        var g = servicioimputacion.GetGrupoCuentaContable(IdCuenta);
                        model.Codigo = g.Id;
                        model.IdNuevo = g.Id;
                        model.Descripcion = g.Descripcion;
                        model.IdTipoElemento = "G";
                        model.selectListCuentaSuperior = GetCuentaSuperior(0);
                        model.IdCuentaSuperior = 0;
                        break;

                    case 1:

                        var r = servicioimputacion.GetRubroContable(IdCuenta);
                        model.Codigo = r.Id;
                        model.IdNuevo = r.Id;
                        model.Descripcion = r.Descripcion;
                        model.IdTipoElemento = "R";
                        model.selectListCuentaSuperior = GetCuentaSuperior(0);
                        model.IdCuentaSuperior = r.IdGrupoCuenta ?? 0;
                        break;

                    case 2:

                        var s = servicioimputacion.GetSubRubroContable(IdCuenta);
                        model.Codigo = s.Id;
                        model.IdNuevo = s.Id;
                        model.Descripcion = s.Descripcion;
                        model.IdTipoElemento = "S";
                        model.IdCuentaSuperior = s.IdRubro;
                        model.selectListCuentaSuperior = GetCuentaSuperior(1);
                        break;

                    case 3:
                        var c = servicioimputacion.GetImputacionContable(IdCuenta);
                        model.Codigo = c.Id;
                        model.IdNuevo = c.Id;
                        model.Descripcion = c.Descripcion;
                        model.IdTipoElemento = "C";
                        model.selectListCuentaSuperior = GetCuentaSuperior(2);
                        model.IdCuentaSuperior = c.IdSubRubro ?? 0;
                        break;

                    default:
                        // code block
                        break;
                }
            }



            return View(model);

        }
        [HttpPost]
        public ActionResult Edit(CuentaPlanContableModelView modelView)
        {
                modelView.Activo = true;
                servicioimputacion.ActualizarCuentaContable(Mapper.Map<CuentaPlanContableModelView, CuentaPlanContableModel>(modelView));
            
            return RedirectToAction("Index");


        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(CuentaPlanContableModelView modelView)
        {           
                modelView.Activo = false;
                servicioimputacion.ActualizarCuentaContable(Mapper.Map<CuentaPlanContableModelView, CuentaPlanContableModel>(modelView));           
            return RedirectToAction("Index");
           
        }


        //[AutorizacionDeSistema]
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlanContableModelView modelView)
        {

            if (ModelState.IsValid)
            {
                modelView.cuentaContable.Activo = true;
                servicioimputacion.NuevaCuentaContable(Mapper.Map<CuentaPlanContableModelView, CuentaPlanContableModel>(modelView.cuentaContable));

            }
        
            return RedirectToAction("Index");
        }



        public List<SelectListItem> GetCuentaSuperior(int TipoElemento)
        {

            List<SelectListItem> selectListItems = new List<SelectListItem>();


            switch (TipoElemento)
            {
                case 0:

                    var g = servicioimputacion.GetGrupoCuentaContable();
                    selectListItems = (g.Select(x =>
                                  new SelectListItem()
                                  {
                                      Value = x.Id.ToString(),
                                      Text = x.Descripcion
                                  })).ToList();

                    break;

                case 1:

                    var r = servicioimputacion.GetRubroContable();
                    selectListItems = (r.Select(x =>
                                  new SelectListItem()
                                  {
                                      Value = x.Id.ToString(),
                                      Text = x.Descripcion
                                  })).ToList();
                    break;

                case 2:

                    var s = servicioimputacion.GetSubRubroContable();
                    selectListItems = (s.Select(x =>
                                    new SelectListItem()
                                    {
                                        Value = x.Id.ToString(),
                                        Text = x.Descripcion
                                    })).ToList();
                    break;

                case 3:
                    var c = servicioimputacion.GetImputacionContable();
                    selectListItems = (c.Select(x =>
                                   new SelectListItem()
                                   {
                                       Value = x.Id.ToString(),
                                       Text = x.Descripcion
                                   })).ToList();
                    break;

                default:
                    // code block
                    break;
            }
            return selectListItems;
        }

        public JsonResult GetlistaCuentas(string TipoElemento)
        {

            List<CuentaPlanContableModelView> model = new List<CuentaPlanContableModelView>();
            switch (TipoElemento)
            {
                //case "G":

                //    var g = servicioimputacion.GetGrupoCuentaContable();
                //    model = (from i in g
                //             select new CuentaPlanContable
                //             {
                //                 Id = i.Id,
                //                 Descripcion = i.Descripcion,
                //             }).ToList();
                //    break;

                case "R":

                    var r = servicioimputacion.GetGrupoCuentaContable();

                    model = (from i in r
                             select new CuentaPlanContableModelView
                             {
                                 Id = i.Id,
                                 Descripcion = i.Descripcion,
                             }).ToList();
                    break;

                case "S":

                    var s = servicioimputacion.GetRubroContable();
                    model = (from i in s
                             select new CuentaPlanContableModelView
                             {
                                 Id = i.Id,
                                 Descripcion = i.Descripcion,
                             }).ToList();
                    break;

                case "C":
                    var c = servicioimputacion.GetSubRubroContable();
                    model = (from i in c
                             select new CuentaPlanContableModelView
                             {
                                 Id = i.Id,
                                 Descripcion = i.Descripcion,
                             }).ToList();
                    break;

                default:
                    // code block
                    break;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }










    }

}



