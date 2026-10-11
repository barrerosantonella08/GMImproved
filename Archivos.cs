using System;
using System.Globalization;
using System.IO;

namespace GMImproved
{
    // Modulo de archivos: guarda y carga inventario y pedidos en archivos de texto
    internal static class Archivos
    {
        const string ArchivoInventario = "inventario.txt";
        const string ArchivoPedidos = "pedidos.txt";

        public static void Guardar()
        {
            string[] lineasInventario = new string[Inventario.total];
            for (int i = 0; i < Inventario.total; i++)
            {
                lineasInventario[i] = Inventario.ids[i] + "|" + Inventario.nombres[i].Replace("|", "/") + "|" +
                                      Inventario.precios[i].ToString(CultureInfo.InvariantCulture) + "|" + Inventario.stocks[i];
            }
            File.WriteAllLines(ArchivoInventario, lineasInventario);

            string[] lineasPedidos = new string[Pedidos.total];
            for (int i = 0; i < Pedidos.total; i++)
            {
                lineasPedidos[i] = Pedidos.ids[i] + "|" + Pedidos.mesas[i] + "|" + Pedidos.idsProducto[i] + "|" +
                                   Pedidos.cantidades[i] + "|" + Pedidos.estados[i] + "|" + Pedidos.observaciones[i].Replace("|", "/");
            }
            File.WriteAllLines(ArchivoPedidos, lineasPedidos);
        }

        public static void Cargar()
        {
            if (File.Exists(ArchivoInventario))
            {
                string[] lineas = File.ReadAllLines(ArchivoInventario);
                for (int i = 0; i < lineas.Length && Inventario.total < Inventario.Capacidad; i++)
                {
                    string[] partes = lineas[i].Split('|');
                    if (partes.Length == 4)
                    {
                        int pos = Inventario.total;
                        Inventario.ids[pos] = int.Parse(partes[0]);
                        Inventario.nombres[pos] = partes[1];
                        Inventario.precios[pos] = double.Parse(partes[2], CultureInfo.InvariantCulture);
                        Inventario.stocks[pos] = int.Parse(partes[3]);
                        Inventario.total++;

                        if (Inventario.ids[pos] >= Inventario.siguienteId)
                        {
                            Inventario.siguienteId = Inventario.ids[pos] + 1;
                        }
                    }
                }
            }

            if (File.Exists(ArchivoPedidos))
            {
                string[] lineas = File.ReadAllLines(ArchivoPedidos);
                for (int i = 0; i < lineas.Length && Pedidos.total < Pedidos.Capacidad; i++)
                {
                    string[] partes = lineas[i].Split('|');
                    if (partes.Length == 6)
                    {
                        int pos = Pedidos.total;
                        Pedidos.ids[pos] = int.Parse(partes[0]);
                        Pedidos.mesas[pos] = int.Parse(partes[1]);
                        Pedidos.idsProducto[pos] = int.Parse(partes[2]);
                        Pedidos.cantidades[pos] = int.Parse(partes[3]);
                        Pedidos.estados[pos] = partes[4];
                        Pedidos.observaciones[pos] = partes[5];
                        Pedidos.total++;

                        if (Pedidos.ids[pos] >= Pedidos.siguienteId)
                        {
                            Pedidos.siguienteId = Pedidos.ids[pos] + 1;
                        }
                    }
                }
            }
        }
    }
}
