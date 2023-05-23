using Mensajero.Comunicacion;
using MensajeroModel.DAL;
using MensajeroModel.DTO;
using ServidorSocketUtils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Servicio_de_Comunicaciones
{
    public class Program
    {
        private static IMensajesDAL mensajesDAL = MensajesDALArchivos.GetInstancia();
        static bool Menu()
        {
            bool continuar = true;
            Console.WriteLine("Selecciones una opcion");
            Console.WriteLine(" 1. Ingresar \n 2. Mostrar \n 0.Salir");
            switch (Console.ReadLine().Trim())
            {
                case "1":
                    Ingresar();
                    break;
                case "2":
                    Mostrar();
                    break;
                case "0":
                    continuar = false;
                    break;
                default:
                    Console.WriteLine("Ingrese de Nuevo");
                    break;
            }
            return continuar;
        }
        static void Main(string[] args)
        {
            HebraServidor hebra = new HebraServidor();
            Thread t = new Thread(new ThreadStart(hebra.Ejecutar));
            t.Start();

            while (Menu()) ;
        }

        static void Ingresar()
        {

            
            Console.WriteLine("Ingrese el número de su medidor: ");
            string nroMedidor = Console.ReadLine().Trim();
            
         
            string fecha = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

            Console.WriteLine(" ¿Cuál es el valor de consumo de su medidor? \n " +
                "Ingrese el valor en kilowats");
            string valorConsumo = Console.ReadLine().Trim();

            
            Mensaje mensaje = new Mensaje()
            {

                NroMedidor = nroMedidor,
                Fecha = fecha,
                ValorConsumo = valorConsumo
            };
            lock (mensajesDAL)
            {
                mensajesDAL.AgregarMensaje(mensaje);
            }
        }

        static void Mostrar()
        {
            List<Mensaje> lecturas = null;
            lock (mensajesDAL)
            {
                lecturas = mensajesDAL.ObtenerMensajes();
            }

            foreach (Mensaje mensaje in lecturas)
            {
                Console.WriteLine(mensaje);
            }
        }

        public void Ejecutar()
        {
            int puerto = Convert.ToInt32(ConfigurationManager.AppSettings["puerto"]);
            ServerSocket servidor = new ServerSocket(puerto);
            servidor.Iniciar();
        }
    }
}
