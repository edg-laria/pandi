﻿using Datos.Interfaces;
using Datos.ModeloDeDatos;
using Negocio.Interfaces;
using Negocio.Modelos;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Negocio.Helpers;
using Entidad.Modelos;
using Datos.Repositorios;

namespace Negocio.Servicios
{
  public class ServicioCompraFacturaPago : ServicioBase
    {

        private CompraFacturaPagoRepositorio oCompraFacturaPagoRepositorio;
        public Action<string, string> _mensaje;

        public ServicioCompraFacturaPago()
        {
            oCompraFacturaPagoRepositorio = kernel.Get<CompraFacturaPagoRepositorio>();
        }


        public CompraFacturaPagoModel InsertarCompraFacturaPago (CompraFacturaPagoModel oCompraFacturaPago)
        {
            try
            {
                var comprapago = Mapper.Map<CompraFacturaPagoModel,CompraFacturaPago >(oCompraFacturaPago);
                var comprapagoRetorno = Mapper.Map<CompraFacturaPago, CompraFacturaPagoModel>(oCompraFacturaPagoRepositorio.InsertarCompraFacturaPago(comprapago));
            return comprapagoRetorno;
            }
            catch (Exception ex)
            {
                NLogHelper.Instance.LogExcepcion(ex, "ServicioCompraFacturaPago >> InsertarCompraFacturaPago");
                _mensaje?.Invoke("Ops!, A ocurriodo un error. Intente mas tarde por favor", "error");
                return null;
            }
           
        }


    }
}
