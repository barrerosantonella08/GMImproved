using System;

namespace GMImproved
{
    internal class Program
    {
        // Usuarios del sistema: el nombre de usuario es igual al rol
        static string[] usuarios = { "admin", "mozo", "cocina", "caja" };
        static string[] claves = { "123", "123", "123", "123" };

        static void Main(string[] args)
        {
            Archivos.Cargar();

            bool ejecutando = true;
            while (ejecutando)
            {
                Console.Clear();
                Console.WriteLine("=== GMImproved - LOGIN ===");
                string rol = Login();

                if (rol == "salir")
                {
                    ejecutando = false;
                }
                else if (rol == "admin")
                {
                    MenuAdministrador();
                }
                else if (rol == "mozo")
                {
                    MenuMozo();
                }
                else if (rol == "cocina")
                {
                    MenuCocina();
                }
                else if (rol == "caja")
                {
                    MenuCaja();
                }

                // Se guardan los datos cada vez que se cierra una sesion
                if (rol != "")
                {
                    Archivos.Guardar();
                }
            }
        }

        // Devuelve el rol, "salir" o "" si las credenciales son incorrectas
        static string Login()
        {
            Console.Write("Usuario (admin / mozo / cocina / caja / salir): ");
            string usuario = (Console.ReadLine() ?? "").Trim().ToLower();
            if (usuario == "salir")
            {
                return "salir";
            }

            Console.Write("Contrasena: ");
            string clave = Console.ReadLine() ?? "";

            for (int i = 0; i < usuarios.Length; i++)
            {
                if (usuarios[i] == usuario && claves[i] == clave)
                {
                    return usuarios[i];
                }
            }

            Console.WriteLine("\n[!] Usuario o contrasena incorrectos.");
            Utilidades.Pausa();
            return "";
        }

        static void MenuAdministrador()
        {
            bool enSesion = true;
            while (enSesion)
            {
                Console.Clear();
                Console.WriteLine("=== ADMINISTRADOR ===");
                Console.WriteLine("1. Ver inventario");
                Console.WriteLine("2. Agregar producto");
                Console.WriteLine("3. Eliminar producto");
                Console.WriteLine("4. Ver pedidos");
                Console.WriteLine("5. Buscar pedido");
                Console.WriteLine("6. Ordenar pedidos");
                Console.WriteLine("0. Cerrar sesion");
                Console.Write("Selecciona una opcion: ");
                string opcion = Console.ReadLine() ?? "";

                switch (opcion)
                {
                    case "1": Inventario.Mostrar(); Utilidades.Pausa(); break;
                    case "2": Inventario.Agregar(); Utilidades.Pausa(); break;
                    case "3": Inventario.Eliminar(); Utilidades.Pausa(); break;
                    case "4": Pedidos.MostrarTodos(); Utilidades.Pausa(); break;
                    case "5": Pedidos.Buscar(); Utilidades.Pausa(); break;
                    case "6": Pedidos.Ordenar(); Utilidades.Pausa(); break;
                    case "0": enSesion = false; break;
                    default: Console.WriteLine("[!] Opcion no valida."); Utilidades.Pausa(); break;
                }
            }
        }

        static void MenuMozo()
        {
            bool enSesion = true;
            while (enSesion)
            {
                Console.Clear();
                Console.WriteLine("=== MOZO ===");
                Console.WriteLine("1. Ver inventario");
                Console.WriteLine("2. Registrar pedido");
                Console.WriteLine("3. Ver pedidos");
                Console.WriteLine("4. Buscar pedido");
                Console.WriteLine("5. Cancelar pedido");
                Console.WriteLine("0. Cerrar sesion");
                Console.Write("Selecciona una opcion: ");
                string opcion = Console.ReadLine() ?? "";

                switch (opcion)
                {
                    case "1": Inventario.Mostrar(); Utilidades.Pausa(); break;
                    case "2": Pedidos.Registrar(); Utilidades.Pausa(); break;
                    case "3": Pedidos.MostrarTodos(); Utilidades.Pausa(); break;
                    case "4": Pedidos.Buscar(); Utilidades.Pausa(); break;
                    case "5": Pedidos.Cancelar(); Utilidades.Pausa(); break;
                    case "0": enSesion = false; break;
                    default: Console.WriteLine("[!] Opcion no valida."); Utilidades.Pausa(); break;
                }
            }
        }

        static void MenuCocina()
        {
            bool enSesion = true;
            while (enSesion)
            {
                Console.Clear();
                Console.WriteLine("=== COCINA ===");
                Console.WriteLine("1. Ver pedidos");
                Console.WriteLine("2. Cambiar estado de un pedido");
                Console.WriteLine("3. Ver inventario");
                Console.WriteLine("4. Reponer stock");
                Console.WriteLine("5. Dar de baja stock (merma)");
                Console.WriteLine("0. Cerrar sesion");
                Console.Write("Selecciona una opcion: ");
                string opcion = Console.ReadLine() ?? "";

                switch (opcion)
                {
                    case "1": Pedidos.MostrarTodos(); Utilidades.Pausa(); break;
                    case "2": Pedidos.CambiarEstado(); Utilidades.Pausa(); break;
                    case "3": Inventario.Mostrar(); Utilidades.Pausa(); break;
                    case "4": Inventario.Reponer(); Utilidades.Pausa(); break;
                    case "5": Inventario.DarDeBaja(); Utilidades.Pausa(); break;
                    case "0": enSesion = false; break;
                    default: Console.WriteLine("[!] Opcion no valida."); Utilidades.Pausa(); break;
                }
            }
        }

        static void MenuCaja()
        {
            bool enSesion = true;
            while (enSesion)
            {
                Console.Clear();
                Console.WriteLine("=== CAJA ===");
                Console.WriteLine("1. Ver pedidos");
                Console.WriteLine("2. Cobrar mesa");
                Console.WriteLine("0. Cerrar sesion");
                Console.Write("Selecciona una opcion: ");
                string opcion = Console.ReadLine() ?? "";

                switch (opcion)
                {
                    case "1": Pedidos.MostrarTodos(); Utilidades.Pausa(); break;
                    case "2": Caja.Cobrar(); Utilidades.Pausa(); break;
                    case "0": enSesion = false; break;
                    default: Console.WriteLine("[!] Opcion no valida."); Utilidades.Pausa(); break;
                }
            }
        }
    }
}
