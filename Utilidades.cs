using System;
using System.Globalization;

namespace GMImproved
{
    // Funciones de ayuda para leer y validar datos del teclado
    internal static class Utilidades
    {
        public static int LeerEntero(string mensaje, int minimo)
        {
            while (true)
            {
                Console.Write(mensaje);
                int numero;
                if (int.TryParse(Console.ReadLine(), out numero) && numero >= minimo)
                {
                    return numero;
                }
                Console.WriteLine("[!] Ingresa un numero entero valido (minimo " + minimo + ").");
            }
        }

        public static int LeerOpcion(string mensaje, int minimo, int maximo)
        {
            while (true)
            {
                int numero = LeerEntero(mensaje, minimo);
                if (numero <= maximo)
                {
                    return numero;
                }
                Console.WriteLine("[!] Elige una opcion entre " + minimo + " y " + maximo + ".");
            }
        }

        public static double LeerDecimal(string mensaje, double minimo)
        {
            while (true)
            {
                Console.Write(mensaje);
                double numero;
                if (double.TryParse(Console.ReadLine(), NumberStyles.Float, CultureInfo.InvariantCulture, out numero) && numero >= minimo)
                {
                    return numero;
                }
                Console.WriteLine("[!] Ingresa un numero valido (ejemplo: 12.50), minimo " + minimo + ".");
            }
        }

        public static string LeerTexto(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                string texto = (Console.ReadLine() ?? "").Trim();
                if (texto != "")
                {
                    return texto;
                }
                Console.WriteLine("[!] Este campo no puede quedar vacio.");
            }
        }

        public static void Pausa()
        {
            Console.WriteLine("\nPresiona Enter para continuar...");
            Console.ReadLine();
        }
    }
}
