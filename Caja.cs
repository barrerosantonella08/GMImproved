using System;

namespace GMImproved
{
    // Modulo de caja: cobro de una mesa y generacion de boleta o factura
    internal static class Caja
    {
        static bool RucValido(string ruc)
        {
            if (ruc.Length != 11)
            {
                return false;
            }
            for (int i = 0; i < ruc.Length; i++)
            {
                if (!char.IsDigit(ruc[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static void Cobrar()
        {
            Console.WriteLine("\n--- CAJA: COBRAR MESA ---");
            Pedidos.MostrarTodos();
            if (Pedidos.total == 0)
            {
                return;
            }

            int mesa = Utilidades.LeerEntero("\nMesa a cobrar: ", 1);

            // Se suman solo los pedidos ENTREGADOS de esa mesa
            double subtotal = 0;
            int cantidadPedidos = 0;
            for (int i = 0; i < Pedidos.total; i++)
            {
                if (Pedidos.mesas[i] == mesa && Pedidos.estados[i] == "Entregado")
                {
                    int pos = Inventario.BuscarPosicion(Pedidos.idsProducto[i]);
                    if (pos != -1)
                    {
                        subtotal = subtotal + Pedidos.cantidades[i] * Inventario.precios[pos];
                    }
                    cantidadPedidos++;
                }
            }

            if (cantidadPedidos == 0)
            {
                Console.WriteLine("[!] No hay pedidos Entregados para la mesa " + mesa + ".");
                return;
            }

            double propina = Utilidades.LeerDecimal("Propina en S/. (0 si no hay): ", 0);

            Console.WriteLine("Metodo de pago: 1. Efectivo  2. Tarjeta  3. Yape/Plin");
            int pago = Utilidades.LeerOpcion("Opcion: ", 1, 3);
            string[] metodos = { "Efectivo", "Tarjeta", "Yape/Plin" };

            Console.WriteLine("Comprobante: 1. Boleta  2. Factura");
            int tipo = Utilidades.LeerOpcion("Opcion: ", 1, 2);
            string ruc = "";
            if (tipo == 2)
            {
                while (true)
                {
                    ruc = Utilidades.LeerTexto("RUC (11 digitos): ");
                    if (RucValido(ruc))
                    {
                        break;
                    }
                    Console.WriteLine("[!] El RUC debe tener 11 numeros.");
                }
            }

            double total = subtotal + propina;

            Console.WriteLine("\n==============================");
            if (tipo == 1)
            {
                Console.WriteLine("        BOLETA - GOOD MOOV");
            }
            else
            {
                Console.WriteLine("        FACTURA - GOOD MOOV");
                Console.WriteLine("RUC cliente: " + ruc);
            }
            Console.WriteLine("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            Console.WriteLine("Mesa: " + mesa);
            Console.WriteLine("Subtotal: S/. " + subtotal.ToString("0.00"));
            Console.WriteLine("Propina:  S/. " + propina.ToString("0.00"));
            Console.WriteLine("TOTAL:    S/. " + total.ToString("0.00"));
            Console.WriteLine("Pago: " + metodos[pago - 1]);
            Console.WriteLine("==============================");

            // Los pedidos cobrados pasan a Pagado
            for (int i = 0; i < Pedidos.total; i++)
            {
                if (Pedidos.mesas[i] == mesa && Pedidos.estados[i] == "Entregado")
                {
                    Pedidos.estados[i] = "Pagado";
                }
            }
            Console.WriteLine("[OK] Cobro registrado.");
        }
    }
}
