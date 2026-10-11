using System.Collections;
using System.Windows;
using System.Windows.Controls;
using Entidades;
using Negocio;

namespace Lab07
{
    public partial class MainWindow : Window
    {
        private readonly PersonaNegocio personaNegocio = new PersonaNegocio();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CargarPersonas();
        }

        private void CargarPersonas()
        {
            try
            {
                MostrarResultados(personaNegocio.Listar(), "Personas activas");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void MostrarResultados(IEnumerable resultados, string vista)
        {
            LimpiarFormulario();
            dgPersonas.ItemsSource = resultados;
            txtVista.Text = vista;
        }

        private void LimpiarFormulario()
        {
            dgPersonas.SelectedItem = null;
            txtId.Clear();
            txtNombres.Clear();
            txtApellidos.Clear();
            txtDni.Clear();
            btnGuardar.IsEnabled = true;
            btnActualizar.IsEnabled = false;
            btnEliminar.IsEnabled = false;
        }

        private void Personas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgPersonas.SelectedItem is Persona persona)
            {
                txtId.Text = persona.IdPersona.ToString();
                txtNombres.Text = persona.Nombres;
                txtApellidos.Text = persona.Apellidos;
                txtDni.Text = persona.Dni;
                btnGuardar.IsEnabled = false;
                btnActualizar.IsEnabled = true;
                btnEliminar.IsEnabled = true;
            }
            else
            {
                LimpiarFormulario();
            }
        }

        private Persona LeerFormulario(int idPersona = 0)
        {
            return new Persona
            {
                IdPersona = idPersona,
                Nombres = txtNombres.Text,
                Apellidos = txtApellidos.Text,
                Dni = txtDni.Text
            };
        }

        private int ObtenerIdSeleccionado()
        {
            if (dgPersonas.SelectedItem is Persona persona)
                return persona.IdPersona;

            throw new Exception("Seleccione una persona del listado de personas activas.");
        }

        private void Nuevo_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
            txtNombres.Focus();
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!personaNegocio.Insertar(LeerFormulario()))
                    throw new Exception("No se pudo guardar la persona.");

                MessageBox.Show("Persona guardada correctamente.", "Lab07", MessageBoxButton.OK, MessageBoxImage.Information);
                LimpiarFormulario();
                CargarPersonas();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void Actualizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Persona persona = LeerFormulario(ObtenerIdSeleccionado());
                if (!personaNegocio.Actualizar(persona))
                    throw new Exception("No se pudo actualizar la persona.");

                MessageBox.Show("Persona actualizada correctamente.", "Lab07", MessageBoxButton.OK, MessageBoxImage.Information);
                LimpiarFormulario();
                CargarPersonas();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int idPersona = ObtenerIdSeleccionado();
                MessageBoxResult respuesta = MessageBox.Show("¿Desea eliminar esta persona?",
                    "Confirmar eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                if (respuesta != MessageBoxResult.Yes)
                    return;

                if (!personaNegocio.Eliminar(idPersona))
                    throw new Exception("No se pudo eliminar la persona.");

                MessageBox.Show("Persona eliminada lógicamente.", "Lab07", MessageBoxButton.OK, MessageBoxImage.Information);
                LimpiarFormulario();
                CargarPersonas();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void Listar_Click(object sender, RoutedEventArgs e)
        {
            CargarPersonas();
        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MostrarResultados(personaNegocio.Buscar(txtBuscar.Text), "Resultados de búsqueda · Personas activas");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void Ordenar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MostrarResultados(personaNegocio.OrdenarPorApellido(), "Personas activas · Ordenadas por apellidos y nombres");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void VerEstados_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MostrarResultados(personaNegocio.ListarConEstado(), "Todas las personas · Activas y eliminadas");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void Agrupar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MostrarResultados(personaNegocio.AgruparPorEstado(), "Cantidad de personas por estado");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void MostrarError(Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Lab07 - Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

    }
}
