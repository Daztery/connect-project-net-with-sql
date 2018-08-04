﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using upc.order.business;
using upc.order.entities;

namespace upc.order.presentation
{
    public partial class Form1 : Form
    {
        double neto = 0;
        double descuento = 0;
        double precio=0;
        double subtotal = 0;
        int id;
        int idcliente;
        
       string[] productos =
        {
            "Teclado" ,"Impresora","Monitor","Mouse","Parlantes"
        };

        Venta objVenta = new Venta();
        IVenta objVentaBusiness = new VentaB();

        Cliente objcliente = new Cliente();
        ICliente objClienteBusiness = new ClienteB();


        public Form1()
        {
            InitializeComponent();
        }

        private void btnregistrar_Click(object sender, EventArgs e)
        {
            //Obtenemos los datos del formulario para pasarlo a los atributos del objeto
            objVenta.Producto = cboproducto.Text;
            objVenta.Cantidad = int.Parse(txtcantidad.Text);

            //Con los datos obtenidos realizamos los calculos
             precio = objVentaBusiness.AsignarPrecio(objVenta);
             subtotal = objVentaBusiness.CalcularSubtotal(objVenta);
             descuento = objVentaBusiness.CalcularDescuento(objVenta);
             neto = objVentaBusiness.CalcularNeto(objVenta);

            objVenta.Precio = precio;
            objVenta.Subtotal = subtotal;
            objVenta.Descuento = descuento;
            objVenta.Neto = neto;

            objcliente.Id = idcliente;
            objVenta.Objcliente = objcliente;
    

            lblprecio.Text = precio.ToString();
         

            try
            {
                bool create = objVentaBusiness.Create(objVenta);
                MessageBox.Show("Venta Registrada", "Registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ReaderVenta();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en registro venta", ex.Message, MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ReaderProducts();
            ReaderVenta();
        }

        //Methods
        void ReaderProducts()
        {
            foreach(string p in productos)
            {
                cboproducto.Items.Add(p);
            }
        }

        void ReaderVenta()
        {
            dgVenta.DataSource = objVentaBusiness.Reader();
        }

        private void cboproducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            objVenta.Producto = cboproducto.Text;
            lblprecio.Text = objVentaBusiness.AsignarPrecio(objVenta).ToString();
        }

        private void dgVenta_MouseClick(object sender, MouseEventArgs e)
        {
            id = Convert.ToInt16(dgVenta.CurrentRow.Cells[0].Value);
            idcliente = Convert.ToInt16(dgVenta.CurrentRow.Cells[8].Value);
            lblCliente.Text = dgVenta.CurrentRow.Cells[7].Value.ToString();
            cboproducto.Text= dgVenta.CurrentRow.Cells[1].Value.ToString();
            txtcantidad.Text = dgVenta.CurrentRow.Cells[2].Value.ToString();
            
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            objVenta.Id = id;
            objVenta.Producto = cboproducto.Text;
            objVenta.Cantidad = int.Parse(txtcantidad.Text);

            //Con los datos obtenidos realizamos los calculos
            precio = objVentaBusiness.AsignarPrecio(objVenta);
            subtotal = objVentaBusiness.CalcularSubtotal(objVenta);
            descuento = objVentaBusiness.CalcularDescuento(objVenta);
            neto = objVentaBusiness.CalcularNeto(objVenta);

            objVenta.Precio = precio;
            objVenta.Subtotal = subtotal;
            objVenta.Descuento = descuento;
            objVenta.Neto = neto;

            objcliente.Id = idcliente;

            objVenta.Objcliente = objcliente;

            lblprecio.Text = precio.ToString();
            

            try
            {
                objVentaBusiness.Update(objVenta);
                MessageBox.Show("Venta Actualizada", "Actualizacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ReaderVenta();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en actualizar venta", ex.Message, MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable table = objClienteBusiness.
                    ReaderByDni(Convert.ToInt32(txtdni.Text));
                if(table.Rows.Count > 0) { 
                    DataRow fila = table.Rows[0];

                    idcliente = Convert.ToInt32(fila[0].ToString());
                    lblCliente.Text = fila[1].ToString();
                }
                else
                {
                    MessageBox.Show("Cliente no existe");
                }
            }
            catch (Exception e1)
            {
                
                MessageBox.Show(e1.Message);
                
            }
           
           
        }

        private void txtdni_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                MessageBox.Show("Solo se permiten numeros", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }
    }
}

