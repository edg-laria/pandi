﻿
$(document).ready(function ()
{
    $("#Anio").on('change', function () {
        $("#Anio option:selected").each(function ()
        {
            anio = $(this).val();

            mes = $("#Mes").val();

           // alert("Año" + anio + "Mes" + mes);

            var elegido = anio + mes

            if (elegido.length == 4) {

                $('.btn').prop('disabled', false);

                $("#Periodo").val(elegido);
            }

            else {

                $("#Periodo").val("Selecciones Año y Mes");
                $('.btn').prop('disabled', true);
             }

           


        });
    });


    $("#Mes").on('change', function () {
        $("#Mes option:selected").each(function ()
        {
           
            anio = $("#Anio").val();

            mes = $(this).val();


            var elegido = anio + mes

           
            if (elegido.length == 4)
            {
                              
                $('.btn').prop('disabled', false);

                $("#Periodo").val(elegido);
            }

            else {

                $("#Periodo").val("Selecciones Año y Mes");
                $('.btn').prop('disabled', true);

            }



        });
    });


});

function verDetalle(cuenta, periodo,tipo) {

    // ajax traer la informacion para el modal detalles
    $.ajax({
        type: "Get",
        url: "/Contabilidad/GetDetallesAsientosContables/",
        data: { cuenta: cuenta , periodo: periodo ,tipo : tipo},
        success: function (result) {
            if (result.data != "null" && result.result) {
                bindInforme(result.data, 'tblDetalles');              
               // toastr.warning(result.data);
            } else {
                toastr.error(result.data);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            toastr.error(' Ops!, A ocurriodo un error. Contacte al Administrador ');
        }

    });
    // mostrar modal
    $("#ModalDetallesCuenta").modal('show');
}


function bindInforme(datosJson, oTable) {
    datosJson = JSON.parse(datosJson);

    var th_ini = "";
    var tr_ini = "";
    var thjson = "";

    if ((typeof (datosJson) !== 'undefined') && (datosJson.length > 0)) {

        // CABECERA DE TABLA
     
        //thjson += "<th> Cuenta </th>";
        //thjson += "<th> Descripcion </th>";
        //thjson += "<th> Debe </th>";
        //thjson += "<th> Haber </th>";
        //th_ini += "<tr>" + thjson + "</tr>";

        var TotalDebe = 0;
        var TotalHaber = 0;

       
        for (x in datosJson) {
             var item = datosJson[x];
            if (item.Importe > 0) { TotalDebe += item.Importe; } else { TotalHaber += item.Importe; }    
            tr_ini += "<tr>";           
            tr_ini += "<td> " + item.IdImputacion + " </td>";          
            tr_ini += "<td> " + item.DescripcionMa + " </td>"; 
            tr_ini += "<td> " + (item.Importe > 0 ? item.Importe  : "0.00")    + " </td>"; 
            tr_ini += "<td> " + (item.Importe < 0 ? item.Importe : "0.00") +" </td>"; 
            tr_ini += " </tr>";
        }
        tr_ini += "<tr>";
        tr_ini += "<td>  </td>";
        tr_ini += "<td> <small class='font-weight-bold '>Saldos</small>  </td>";
        tr_ini += "<td> <small class='font-weight-bold '>" + TotalDebe + "</small>   </td>";
        tr_ini += "<td> <small class='font-weight-bold '>" + TotalHaber + "</small>  </td>";
        tr_ini += " </tr>";

        $('#' + oTable + ' tbody tr').remove();
        $('#' + oTable + ' tbody').append(tr_ini);
        var saldo = 0;
        saldo = (TotalDebe - (TotalHaber * -1));
        $("#SaldoCuenta").html( saldo );

    } else {

        toastr.warning('No se encontraron datos...');

    }
}

