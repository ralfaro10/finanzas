using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using finanzas;

namespace GestionFinanzasConsola
{

    class Program
    {
        //static List<Usuario> usuarios = LeerArchivoUsuarios();
        static Usuario usuarioActual = new(); 
        static List<IngresoMensual> ingresosMensuales = new();
        static List<GastoFijo> gastosFijos = new();
        static decimal ahorroMensual = 0;

        static void Main()
        {
            var ubicacionArchivoUsuarios = @"D:\Development\Rod_BitBucket\personal\finanzas-main\usuarios.txt";
            var usuarios = LeerArchivoUsuarios(ubicacionArchivoUsuarios);

            bool continuar = true;
            while (continuar)
            {
                Console.WriteLine("===== Gestionador de Finanzas Personales =====");
                Console.WriteLine("1. Crear Usuario");
                Console.WriteLine("2. Iniciar Sesión");
                Console.WriteLine("3. Salir");
                Console.Write("Seleccione una opción: ");
                var opcionMenuPrincipal = Console.ReadLine();

                switch (opcionMenuPrincipal)
                {
                    case "1":
                        CrearUsuario(ubicacionArchivoUsuarios, usuarios);
                        break;
                    case "2":
                        IniciarSesion(usuarios);
                        break;
                    case "3":
                        continuar = false;
                        break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }
        }

        static void CrearUsuario(string ubicacionArchivoUsuarios, List<Usuario> usuarios)
        {
            var usuarioAgregado = false;

            do
            {
                Console.Write("Ingrese un nombre de usuario: ");
                var nombreUsuario = Console.ReadLine();
                Console.Write("Ingrese una contraseña: ");
                var contraseña = Console.ReadLine();

                if (usuarios!=null && usuarios.Where(x => x.NombreUsuario == nombreUsuario).FirstOrDefault() != null)
                {
                    Console.WriteLine("Usuario ya existente");
                }
                else
                {
                    usuarioAgregado = true;

                    usuarios.Add(new Usuario { NombreUsuario = nombreUsuario, Contraseña = contraseña });

                    GuardarUsuariosEnArchivo(ubicacionArchivoUsuarios, usuarios);

                    Console.WriteLine("Usuario creado exitosamente.");
                }

            } while (!usuarioAgregado);

        }

        static void IniciarSesion(List<Usuario> usuarios)
        {
            Console.Write("Nombre de Usuario: ");
            var nombreUsuario = Console.ReadLine();
            Console.Write("Contraseña: ");
            var contraseña = Console.ReadLine();

            usuarioActual = usuarios.Find(u => u.NombreUsuario == nombreUsuario && u.Contraseña == contraseña);

            if (usuarioActual != null)
            {
                Console.WriteLine($"¡Bienvenido, {usuarioActual.NombreUsuario}!");
                MenuPrincipal();
            }
            else
            {
                Console.WriteLine("Credenciales incorrectas.");
                Main();
            }
        }

        static void MenuPrincipal()
        {
            bool autenticado = true;
            var ubicacionArchivoCSV = @"D:\Development\Rod_BitBucket\personal\finanzas-main\balanceGeneral.csv";

            while (autenticado)
            {
                Console.WriteLine("===== Menú Principal =====");
                Console.WriteLine("1. Ingresar Ingreso Mensual");
                Console.WriteLine("2. Ingresar Gasto Fijo");
                Console.WriteLine("3. Ingresar Monto de Ahorro");
                Console.WriteLine("4. Ingresar Compra Adicional");
                Console.WriteLine("5. Generar Informe");
                Console.WriteLine("6. Cerrar Sesión");
                Console.Write("Seleccione una opción: ");
                string opcionMenuPrincipal = Console.ReadLine();

                switch (opcionMenuPrincipal)
                {
                    case "1":
                        IngresarIngresoMensual();
                        break;
                    case "2":
                        IngresarGastoFijo();
                        break;
                    case "3":
                        IngresarMontoAhorro();
                        break;
                    case "4":
                        IngresarCompraAdicional();
                        break;
                    case "5":
                        GenerarInforme(ubicacionArchivoCSV);
                        break;
                    case "6":
                        autenticado = false;
                        Console.WriteLine("Sesión cerrada.");
                        break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }
        }

        static void GuardarUsuariosEnArchivo(string ubicacionArchivoUsuarios, List<Usuario> usuarios)
        {
            var sb = new List<string>();

            foreach (var item in usuarios)
            {
                var usuario = $"{item.NombreUsuario};{item.Contraseña}";
                sb.Add(usuario);
            }

            File.WriteAllLines(ubicacionArchivoUsuarios, sb.ToArray());
        }

        static List<Usuario> LeerArchivoUsuarios(string ubicacionArchivoUsuarios)
        {
            var listaUsuarios = File.ReadAllLines(ubicacionArchivoUsuarios);
            var listaUsuariosArchivo = new List<Usuario>();

            if (listaUsuarios != null)
            {
                foreach (var usuario in listaUsuarios)
                {
                    var credenciales = usuario.Split(";");
                    var usuarioNuevo = new Usuario()
                    {
                        Contraseña = credenciales[1],
                        NombreUsuario = credenciales[0]
                    };
                    listaUsuariosArchivo.Add(usuarioNuevo);
                }
            }

            return listaUsuariosArchivo;
        }

        static void IngresarIngresoMensual()
        {
            Console.Write("Ingrese el monto del ingreso mensual: ");
            decimal monto = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Ingrese la fecha del ingreso (yyyy-mm-dd): ");
            DateTime fecha = Convert.ToDateTime(Console.ReadLine());

            ingresosMensuales.Add(new IngresoMensual { Cantidad = monto, Fecha = fecha });

            Console.WriteLine("Ingreso mensual registrado exitosamente.");
        }

        static void IngresarGastoFijo()
        {
            Console.Write("Ingrese la descripción del gasto fijo: ");
            string descripcion = Console.ReadLine();
            Console.Write("Ingrese el monto del gasto fijo: ");
            decimal monto = Convert.ToDecimal(Console.ReadLine());

            gastosFijos.Add(new GastoFijo { Descripcion = descripcion, Cantidad = monto });

            Console.WriteLine("Gasto fijo registrado exitosamente.");
        }

        static void IngresarMontoAhorro()
        {
            Console.Write("Ingrese el monto de ahorro mensual: ");
            ahorroMensual = Convert.ToDecimal(Console.ReadLine());

            Console.WriteLine("Monto de ahorro registrado exitosamente.");
        }

        static void IngresarCompraAdicional()
        {
            Console.Write("Ingrese el monto de la compra adicional: ");
            decimal monto = Convert.ToDecimal(Console.ReadLine());

            // Restar el monto de la compra adicional del ahorro mensual
            ahorroMensual -= monto;

            Console.WriteLine("Compra adicional registrada exitosamente.");
        }

        static void GenerarInforme(string ubicacionArchivoCSV)
        {
            Console.Clear();

            decimal totalIngresos = 0;
            decimal totalGastosFijos = 0;

            totalIngresos = ingresosMensuales.Sum(x=>x.Cantidad);
            totalGastosFijos = gastosFijos.Sum(x => x.Cantidad);

            decimal dineroDisponible = totalIngresos - totalGastosFijos - ahorroMensual;

            Console.WriteLine("===== Informe Financiero =====");
            Console.WriteLine($"Total Ingresos: {totalIngresos}");
            Console.WriteLine($"Total Gastos Fijos: {totalGastosFijos}");
            Console.WriteLine($"Ahorro Mensual: {ahorroMensual}");
            Console.WriteLine($"Dinero Disponible: {dineroDisponible}");
            Console.WriteLine();

            // Mostrar entradas de dinero (ingresos mensuales)
            Console.WriteLine("===== Entradas de Dinero =====");
            foreach (var ingreso in ingresosMensuales)
            {
                Console.WriteLine($"Fecha: {ingreso.Fecha.ToString("yyyy-MM-dd")}, Monto: {ingreso.Cantidad}");
            }
            Console.WriteLine();
            // Mostrar salidas de dinero (gastos fijos y compras adicionales)
            Console.WriteLine("===== Salidas de Dinero =====");
            foreach (var gasto in gastosFijos)
            {
                Console.WriteLine($"Descripción: {gasto.Descripcion}, Monto: {gasto.Cantidad}");
            }
            Console.WriteLine($"Compra Adicional, Monto: {-ahorroMensual}");

            // Mostrar el balance total
            Console.WriteLine();
            Console.WriteLine("===== Balance Total =====");
            Console.WriteLine($"Balance: {dineroDisponible}");
            Console.WriteLine("Archivo .csv creado con informacion");

            using (var w = new StreamWriter(ubicacionArchivoCSV))
            {
                var reporte = new StringBuilder();
                reporte
                .AppendLine("===== Informe Financiero =====")
                .AppendLine($"Total Ingresos: {totalIngresos}")
                .AppendLine($"Total Gastos Fijos: {totalGastosFijos}")
                .AppendLine($"Ahorro Mensual: {ahorroMensual}")
                .AppendLine($"Dinero Disponible: {dineroDisponible}")
                .AppendLine($"Balance: {dineroDisponible}");
                w.WriteLine(reporte);
                w.Flush();
                
            }
        }
    }
}
