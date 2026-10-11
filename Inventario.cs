using System;
using System.IO;

namespace GMImproved
{
    // Modulo de inventario: productos, precios y stock (arreglos paralelos)
    internal static class Inventario
    {
        public const int Capacidad = 50;
        public static int[] ids = new int[Capacidad];
        public static string[] nombres = new string[Capacidad];
        public static double[] precios = new double[Capacidad];
        public static int[] stocks = new int[Capacidad];
        public static int total = 0;
        public static int siguienteId = 1;

        public static void Mostrar()
        {
            Console.WriteLine("\n--- INVENTARIO ACTUAL ---");
            if (total == 0)
            {
                Console.WriteLine("No hay productos registrados aun.");
                return;
            }
            for (int i = 0; i < total; i++)
            {
                Console.WriteLine("ID: " + ids[i] + " | " + nombres[i].PadRight(20) + " | S/. " + precios[i].ToString("0.00") + " | Stock: " + stocks[i]);
            }
        }

        // Devuelve la posicion del producto en los arreglos, o -1 si no existe
        public static int BuscarPosicion(int id)
        {
            for (int i = 0; i < total; i++)
            {
                if (ids[i] == id)
                {
                    return i;
                }
            }
            return -1;
        }

        // Compara nombres sin importar mayusculas o minusculas
        public static bool ExisteNombre(string nombre)
        {
            for (int i = 0; i < total; i++)
            {
                if (nombres[i].ToLower() == nombre.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        // Pide un ID y devuelve la posicion (o -1 si no se encontro)
        public static int PedirProducto()
        {
            int id = Utilidades.LeerEntero("ID del producto: ", 1);
            int pos = BuscarPosicion(id);
            if (pos == -1)
            {
                Console.WriteLine("[!] Producto no encontrado.");
            }
            return pos;
        }

        public static void Agregar()
        {
            Console.WriteLine("\n--- AGREGAR PRODUCTO ---");
            if (total >= Capacidad)
            {
                Console.WriteLine("[!] No hay espacio para agregar mas productos.");
                return;
            }

            string nombre = Utilidades.LeerTexto("Nombre del producto: ");
            if (ExisteNombre(nombre))
            {
                Console.WriteLine("[!] Ya existe un producto con ese nombre.");
                return;
            }
            double precio = Utilidades.LeerDecimal("Precio: ", 0.01);
            int stock = Utilidades.LeerEntero("Stock inicial: ", 0);

            ids[total] = siguienteId;
            nombres[total] = nombre;
            precios[total] = precio;
            stocks[total] = stock;

            Console.WriteLine("\n[OK] Producto '" + nombre + "' agregado con ID: " + siguienteId);
            siguienteId++;
            total++;
        }

        public static void Eliminar()
        {
            Console.WriteLine("\n--- ELIMINAR PRODUCTO ---");
            if (total == 0)
            {
                Console.WriteLine("[!] No hay productos registrados.");
                return;
            }

            int pos = PedirProducto();
            if (pos == -1)
            {
                return;
            }

            string nombre = nombres[pos];
            for (int i = pos; i < total - 1; i++)
            {
                ids[i] = ids[i + 1];
                nombres[i] = nombres[i + 1];
                precios[i] = precios[i + 1];
                stocks[i] = stocks[i + 1];
            }
            total--;
            Console.WriteLine("\n[OK] El producto '" + nombre + "' fue eliminado.");
        }

        public static void Reponer()
        {
            Console.WriteLine("\n--- REPOSICION DE STOCK ---");
            if (total == 0)
            {
                Console.WriteLine("[!] No hay productos registrados.");
                return;
            }

            int pos = PedirProducto();
            if (pos == -1)
            {
                return;
            }

            Console.WriteLine("--> " + nombres[pos] + " | Stock actual: " + stocks[pos]);
            int ingreso = Utilidades.LeerEntero("Cantidad a sumar: ", 1);
            stocks[pos] = stocks[pos] + ingreso;
            Console.WriteLine("\n[OK] " + nombres[pos] + " ahora tiene " + stocks[pos] + " unidades.");
        }

        public static void DarDeBaja()
        {
            Console.WriteLine("\n--- BAJA DE STOCK POR MERMA / VENCIDO ---");
            if (total == 0)
            {
                Console.WriteLine("[!] No hay productos registrados.");
                return;
            }

            int pos = PedirProducto();
            if (pos == -1)
            {
                return;
            }

            Console.WriteLine("--> " + nombres[pos] + " | Stock actual: " + stocks[pos]);
            if (stocks[pos] == 0)
            {
                Console.WriteLine("[!] Este producto ya tiene 0 unidades.");
                return;
            }

            int cantidad = Utilidades.LeerEntero("Cantidad a descontar: ", 1);
            if (cantidad > stocks[pos])
            {
                Console.WriteLine("[!] No puedes retirar mas del stock existente (" + stocks[pos] + ").");
                return;
            }
            string motivo = Utilidades.LeerTexto("Motivo de la baja: ");

            stocks[pos] = stocks[pos] - cantidad;

            // Se guarda un registro de la merma en un archivo de texto
            string linea = DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " | " + nombres[pos] + " | " + cantidad + " | " + motivo + Environment.NewLine;
            File.AppendAllText("mermas.txt", linea);

            Console.WriteLine("\n[OK] Baja registrada. Stock restante de " + nombres[pos] + ": " + stocks[pos]);
        }
    }
}
