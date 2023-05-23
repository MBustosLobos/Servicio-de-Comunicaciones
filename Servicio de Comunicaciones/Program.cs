using ServicioDeComunicaciones;
using MensajeroModel.DAL;
using MensajeroModel.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimuladorMedidorElectrico
{
    public class Program
    {
        private static IMensajesDAL mensajesDAL = MensajesDALArchivos.GetInstancia();
        static bool Menu()
        {
            bool continuar = true;
            Console.WriteLine("Seleccione una opcion");
            Console.WriteLine(" 1 Mostrar datos de Medidores \n 0.Salir");
            switch (Console.ReadLine().Trim())
            {
             
                case "1":
                    Mostrar();
                    break;
                case "0":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Por favor elija una opción...");
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
