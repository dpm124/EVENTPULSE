using System.ComponentModel;
using System.Runtime.CompilerServices;

public class EscenarioModel : INotifyPropertyChanged
{
    private string nombre;
    public string Nombre
    {
        get => nombre;
        set
        {
            if (nombre != value)
            {
                nombre = value;
                OnPropertyChanged();
            }
        }
    }

    private bool tieneInformacion;
    public bool TieneInformacion
    {
        get => tieneInformacion;
        set
        {
            if (tieneInformacion != value)
            {
                tieneInformacion = value;
                OnPropertyChanged();
            }
        }
    }

    private string artista;
    public string Artista
    {
        get => artista;
        set
        {
            if (artista != value)
            {
                artista = value;
                OnPropertyChanged();
            }
        }
    }

    private string fecha;
    public string Fecha
    {
        get => fecha;
        set
        {
            if (fecha != value)
            {
                fecha = value;
                OnPropertyChanged();
            }
        }
    }

    private int aforoMax;
    public int AforoMax
    {
        get => aforoMax;
        set
        {
            if (aforoMax != value)
            {
                aforoMax = value;
                OnPropertyChanged();
            }
        }
    }

    private int aforo;
    public int Aforo
    {
        get => aforo;
        set
        {
            if (aforo != value)
            {
                aforo = value;
                OnPropertyChanged();
            }
        }
    }

    private int numeroSalidas;
    public int NumeroSalidas
    {
        get => numeroSalidas;
        set
        {
            if (numeroSalidas != value)
            {
                numeroSalidas = value;
                OnPropertyChanged();
            }
        }
    }

    private int numeroAsesos;
    public int NumeroAsesos
    {
        get => numeroAsesos;
        set
        {
            if (numeroAsesos != value)
            {
                numeroAsesos = value;
                OnPropertyChanged();
            }
        }
    }

    private int numeroServiciosMedicos;
    public int NumeroServiciosMedicos
    {
        get => numeroServiciosMedicos;
        set
        {
            if (numeroServiciosMedicos != value)
            {
                numeroServiciosMedicos = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string nombrePropiedad = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombrePropiedad));
    }
}
