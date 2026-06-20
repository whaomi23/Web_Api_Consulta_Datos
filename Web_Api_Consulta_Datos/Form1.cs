using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Web_Api_Consulta_Datos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.MouseDown += new MouseEventHandler(Form1_MouseDown);


        }

        private NotifyIcon notifyIcon = new NotifyIcon();

        // Definimos las URL del servicio web como constantes
        private const string UrlServicioGrupo = "http://localhost:{0}/Universidad/ValidarGrupo?grupo={1}";
        private const string UrlServicioCurp = "http://localhost:{0}/Universidad/ValidarCURP?curp={1}";

        // Variable para guardar el puerto personalizado
        private int puertoGuardado = 51347;

        private int ObtenerPuertoPersonalizado()
        {
            if (int.TryParse(txtPuerto.Text, out int puertoPersonalizado))
            {
                return puertoPersonalizado;
            }
            else
            {
                // Si no es válido, usar el puerto guardado
                return puertoGuardado;
            }
        }


        private void btnGuardarPuerto_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtPuerto.Text, out int puertoPersonalizado))
            {
                puertoGuardado = puertoPersonalizado;
                MessageBox.Show($"Puerto guardado correctamente: {puertoGuardado}");
                // Mostrar notificación de que el puerto fue guardado correctamente
                MostrarNotificacionConexion("Puerto actualizado a: " + puertoGuardado, ToolTipIcon.Info);
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un puerto válido.");
            }
        }



        // Botón para validar el grupo
        private void btnValidarGrupo_Click(object sender, EventArgs e)
        {
            string grupo = txtGrupo.Text;

            if (string.IsNullOrEmpty(grupo))
            {
                MessageBox.Show("Por favor ingrese un grupo.");
                return;
            }

            // Ejecutar la validación en un Task asíncrono separado para no bloquear la UI
            Task.Run(async () =>
            {
                int puerto = ObtenerPuertoPersonalizado(); // Obtener el puerto personalizado
                bool esValido = await ValidarGrupo(grupo, puerto); // Ejecutar la validación sin bloquear

                // Actualizar la UI después de la validación
                this.Invoke((Action)(() =>
                {
                    richTextBoxResultadoGrupo.Clear(); // Limpiar el RichTextBox antes de mostrar nuevo resultado

                    if (esValido)
                    {
                        richTextBoxResultadoGrupo.AppendText("Grupo válido.");
                        richTextBoxResultadoGrupo.SelectionStart = 0; // Establecer el inicio de la selección para cambiar el color
                        richTextBoxResultadoGrupo.SelectionLength = richTextBoxResultadoGrupo.Text.Length;
                        richTextBoxResultadoGrupo.SelectionColor = System.Drawing.Color.Green; // Cambiar el color a verde
                    }
                    else
                    {
                        richTextBoxResultadoGrupo.AppendText("Grupo no válido.");
                        richTextBoxResultadoGrupo.SelectionStart = 0;
                        richTextBoxResultadoGrupo.SelectionLength = richTextBoxResultadoGrupo.Text.Length;
                        richTextBoxResultadoGrupo.SelectionColor = System.Drawing.Color.Red; // Cambiar el color a rojo
                    }
                }));
            });
        }

        // Método para validar el Grupo a través de un Web Service
        private async Task<bool> ValidarGrupo(string grupo, int puerto)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Usamos la URL definida y sustituimos el valor de "grupo" y el puerto en la URL
                    string url = string.Format(UrlServicioGrupo, puerto, grupo);
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string respuesta = await response.Content.ReadAsStringAsync();
                        MostrarNotificacionConexion("Conexión exitosa.", ToolTipIcon.Info); // Mostrar solo conexión exitosa
                        return respuesta == "true"; // El Web Service devuelve true o false
                    }
                    else
                    {
                        throw new Exception("Error en la respuesta del servidor.");
                    }
                }
                catch (Exception ex)
                {
                    MostrarNotificacionConexion("Error al conectar al servicio web: " + ex.Message, ToolTipIcon.Error); // Mostrar solo error de conexión
                }
            }
            return false;
        }

        // Botón para validar el CURP
        private void btnValidarCURP_Click(object sender, EventArgs e)
        {
            string curp = txtCURP.Text;

            if (string.IsNullOrEmpty(curp))
            {
                MessageBox.Show("Por favor ingrese un CURP.");
                return;
            }

            // Ejecutar la validación en un Task asíncrono separado para no bloquear la UI
            Task task = Task.Run(async () =>
            {
                int puerto = ObtenerPuertoPersonalizado(); // Obtener el puerto personalizado
                var resultado = await ValidarCURP(curp, puerto); // Ejecutar la validación sin bloquear

                // Actualizar la UI después de la validación
                this.Invoke((Action)(() =>
                {
                    richTextBoxResultadoCURP.Clear(); // Limpiar el RichTextBox antes de mostrar nuevo resultado

                    if (resultado != null)
                    {
                        richTextBoxResultadoCURP.AppendText(resultado.ToString()); // Mostrar el resultado del CURP
                        richTextBoxResultadoCURP.SelectionStart = 0;
                        richTextBoxResultadoCURP.SelectionLength = richTextBoxResultadoCURP.Text.Length;
                        richTextBoxResultadoCURP.SelectionColor = System.Drawing.Color.Green; // Cambiar color a verde
                    }
                    else
                    {
                        richTextBoxResultadoCURP.AppendText("CURP no válido.");
                        richTextBoxResultadoCURP.SelectionStart = 0;
                        richTextBoxResultadoCURP.SelectionLength = richTextBoxResultadoCURP.Text.Length;
                        richTextBoxResultadoCURP.SelectionColor = System.Drawing.Color.Red; // Cambiar color a rojo
                    }
                }));
            });
        }

        // Método para validar el CURP a través de un Web Service
        private async Task<JObject> ValidarCURP(string curp, int puerto)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = string.Format(UrlServicioCurp, puerto, curp);
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string respuesta = await response.Content.ReadAsStringAsync();
                        MostrarNotificacionConexion("Conexión exitosa.", ToolTipIcon.Info); // Mostrar solo conexión exitosa
                        return JObject.Parse(respuesta); // Aquí se devuelve el objeto JSON completo
                    }
                    else
                    {
                        throw new Exception("Error en la respuesta del servidor.");
                    }
                }
                catch (Exception ex)
                {
                    MostrarNotificacionConexion("Error al conectar al servicio web: " + ex.Message, ToolTipIcon.Error); // Mostrar solo error de conexión
                }
            }
            return null;
        }

        // Método para mostrar el resultado del grupo en RichTextBox
        private void MostrarResultadoGrupo(string mensaje, System.Drawing.Color color)
        {
            richTextBoxResultadoGrupo.Text = mensaje;
            richTextBoxResultadoGrupo.SelectionColor = color;
        }

        // Método para mostrar notificaciones solo de conexión usando NotifyIcon
        private void MostrarNotificacionConexion(string mensaje, ToolTipIcon icono)
        {
            notifyIcon.Icon = SystemIcons.Information;
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(1000, "Resultado de la Conexión", mensaje, icono);
        }

        // Función para mover la ventana sin bordes
        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, 0x112, 0xf012, 0);
            }
        }

        // Botónes para reiniciar los campos
        private void btnLimpiar1_Click(object sender, EventArgs e)
        {
            txtGrupo.Clear();
            richTextBoxResultadoGrupo.Clear();
        }

        private void btnLimpiar2_Click(object sender, EventArgs e)
        {
            txtCURP.Clear();
            richTextBoxResultadoCURP.Clear();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
