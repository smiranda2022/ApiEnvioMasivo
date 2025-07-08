using System.Threading.Tasks;

namespace ApiEnvioMasivo.Services
{
    public interface IFlujoService
    {
        Task<string> EjecutarFlujoAsync(int flujoId); // <- corregir esto
        Task EjecutarFlujosAsync();
        Task EjecutarTodosLosFlujosAsync();
    }


}


