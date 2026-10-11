using System;

namespace GMImproved
{
    // Modulo de pedidos: registrar, buscar, cambiar estado, cancelar y ordenar
    internal static class Pedidos
    {
        public const int Capacidad = 200;
        public static string[] listaEstados = { "Pendiente", "En preparacion", "Listo", "Entregado", "Pagado" };

        public static int[] ids = new int[Capacidad];
        public static int[] mesas = new int[Capacidad];
        public static int[] idsProducto = new int[Capacidad];
        public static int[] cantidades = new int[Capacidad];
        public static string[] observaciones = new string[Capacidad];
        public static string[] estados = new string[Capacidad];
        public static int total = 0;
        public static int siguienteId = 1;

        public static string NombreProducto(int idProducto)
        {
            int pos = Inventario.BuscarPosicion(idProducto);
            if (pos == -1)
            {
                return "(producto eliminado)";
            }
            return Inventario.nombres[pos];
        }

        static void MostrarUno(int i)
        {
            Console.WriteLine("Pedido #" + ids[i] + " | Mesa " + mesas[i] + " | " + NombreProducto(idsProducto[i]) + " x" + cantidades[i] + " | Estado: " + estados[i] + " | Obs: " + observaciones[i]);
        }

        public static void MostrarTodos()
        {
            Console.WriteLine("\n--- PEDIDOS ---");
            if (total == 0)
            {
                Console.WriteLine("No hay pedidos registrados.");
                return;
            }
            for (int i = 0; i < total; i++)
            {
                MostrarUno(i);
            }
        }

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

        public static void Registrar()
        {
            Console.WriteLine("\n--- REGISTRAR PEDIDO ---");
            if (Inventario.total == 0)
            {
                Console.WriteLine("[!] No hay productos en el inventario.");
                return;
            }
            if (total >= Capacidad)
            {
                Console.WriteLine("[!] No hay espacio para mas pedidos.");
                return;
            }

            int mesa = Utilidades.LeerEntero("Numero de mesa: ", 1);
            Inventario.Mostrar();
            int posProducto = Inventario.PedirProducto();
            if (posProducto == -1)
            {
                return;
            }

            int cantidad = Utilidades.LeerEntero("Cantidad: ", 1);
            if (cantidad > Inventario.stocks[posProducto])
            {
                Console.WriteLine("[!] Stock insuficiente. Solo quedan " + Inventario.stocks[posProducto] + " unidades.");
                return;
            }

            Console.Write("Observaciones (alergias, aniadidos; Enter si no hay): ");
            string obs = (Console.ReadLine() ?? "").Trim();
            if (obs == "")
            {
                obs = "Sin observaciones";
            }

            // Se descuenta el stock y se guarda el pedido
            Inventario.stocks[posProducto] = Inventario.stocks[posProducto] - cantidad;
            ids[total] = siguienteId;
            mesas[total] = mesa;
            idsProducto[total] = Inventario.ids[posProducto];
            cantidades[total] = cantidad;
            observaciones[total] = obs;
            estados[total] = "Pendiente";

            Console.WriteLine("\n[OK] Pedido #" + siguienteId + " registrado para la mesa " + mesa + " (Pendiente).");
            siguienteId++;
            total++;
        }

        public static void Buscar()
        {
            Console.WriteLine("\n--- BUSCAR PEDIDO ---");
            if (total == 0)
            {
                Console.WriteLine("No hay pedidos registrados.");
                return;
            }

            Console.WriteLine("Buscar por: 1. ID  2. Mesa  3. Producto  4. Estado");
            int opcion = Utilidades.LeerOpcion("Opcion: ", 1, 4);
            int encontrados = 0;

            if (opcion == 1 || opcion == 2)
            {
                int valor = Utilidades.LeerEntero("Valor a buscar: ", 1);
                for (int i = 0; i < total; i++)
                {
                    if ((opcion == 1 && ids[i] == valor) || (opcion == 2 && mesas[i] == valor))
                    {
                        MostrarUno(i);
                        encontrados++;
                    }
                }
            }
            else
            {
                string texto = Utilidades.LeerTexto("Texto a buscar: ").ToLower();
                for (int i = 0; i < total; i++)
                {
                    string campo;
                    if (opcion == 3)
                    {
                        campo = NombreProducto(idsProducto[i]);
                    }
                    else
                    {
                        campo = estados[i];
                    }

                    if (campo.ToLower().Contains(texto))
                    {
                        MostrarUno(i);
                        encontrados++;
                    }
                }
            }

            if (encontrados == 0)
            {
                Console.WriteLine("No se encontraron pedidos.");
            }
        }

        public static void CambiarEstado()
        {
            MostrarTodos();
            if (total == 0)
            {
                return;
            }

            int id = Utilidades.LeerEntero("\nID del pedido: ", 1);
            int pos = BuscarPosicion(id);
            if (pos == -1)
            {
                Console.WriteLine("[!] Pedido no encontrado.");
                return;
            }
            if (estados[pos] == "Pagado")
            {
                Console.WriteLine("[!] Ese pedido ya esta pagado.");
                return;
            }

            Console.WriteLine("1. Pendiente  2. En preparacion  3. Listo  4. Entregado");
            int opcion = Utilidades.LeerOpcion("Nuevo estado: ", 1, 4);
            estados[pos] = listaEstados[opcion - 1];
            Console.WriteLine("\n[OK] El pedido #" + id + " ahora esta: " + estados[pos]);
        }

        public static void Cancelar()
        {
            MostrarTodos();
            if (total == 0)
            {
                return;
            }

            int id = Utilidades.LeerEntero("\nID del pedido a cancelar: ", 1);
            int pos = BuscarPosicion(id);
            if (pos == -1)
            {
                Console.WriteLine("[!] Pedido no encontrado.");
                return;
            }
            if (estados[pos] != "Pendiente" && estados[pos] != "En preparacion")
            {
                Console.WriteLine("[!] Solo se pueden cancelar pedidos Pendientes o En preparacion.");
                return;
            }

            // Se devuelve el stock al inventario
            int posProducto = Inventario.BuscarPosicion(idsProducto[pos]);
            if (posProducto != -1)
            {
                Inventario.stocks[posProducto] = Inventario.stocks[posProducto] + cantidades[pos];
            }

            for (int i = pos; i < total - 1; i++)
            {
                ids[i] = ids[i + 1];
                mesas[i] = mesas[i + 1];
                idsProducto[i] = idsProducto[i + 1];
                cantidades[i] = cantidades[i + 1];
                observaciones[i] = observaciones[i + 1];
                estados[i] = estados[i + 1];
            }
            total--;
            Console.WriteLine("\n[OK] Pedido #" + id + " cancelado y stock devuelto.");
        }

        static int IndiceEstado(string estado)
        {
            for (int i = 0; i < listaEstados.Length; i++)
            {
                if (listaEstados[i] == estado)
                {
                    return i;
                }
            }
            return -1;
        }

        // Intercambia dos pedidos en todos los arreglos
        static void Intercambiar(int a, int b)
        {
            int auxInt;
            string auxTexto;

            auxInt = ids[a]; ids[a] = ids[b]; ids[b] = auxInt;
            auxInt = mesas[a]; mesas[a] = mesas[b]; mesas[b] = auxInt;
            auxInt = idsProducto[a]; idsProducto[a] = idsProducto[b]; idsProducto[b] = auxInt;
            auxInt = cantidades[a]; cantidades[a] = cantidades[b]; cantidades[b] = auxInt;
            auxTexto = observaciones[a]; observaciones[a] = observaciones[b]; observaciones[b] = auxTexto;
            auxTexto = estados[a]; estados[a] = estados[b]; estados[b] = auxTexto;
        }

        // Ordenamiento burbuja por ID o por estado
        public static void Ordenar()
        {
            Console.WriteLine("\n--- ORDENAR PEDIDOS ---");
            if (total == 0)
            {
                Console.WriteLine("No hay pedidos registrados.");
                return;
            }

            Console.WriteLine("Ordenar por: 1. ID  2. Estado");
            int opcion = Utilidades.LeerOpcion("Opcion: ", 1, 2);

            for (int i = 0; i < total - 1; i++)
            {
                for (int j = 0; j < total - 1 - i; j++)
                {
                    bool cambiar;
                    if (opcion == 1)
                    {
                        cambiar = ids[j] > ids[j + 1];
                    }
                    else
                    {
                        cambiar = IndiceEstado(estados[j]) > IndiceEstado(estados[j + 1]);
                    }

                    if (cambiar)
                    {
                        Intercambiar(j, j + 1);
                    }
                }
            }
            MostrarTodos();
        }
    }
}
