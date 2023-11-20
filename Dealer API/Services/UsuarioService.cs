using Dealer_API.Models;

namespace Dealer_API.Services
{
    public class UsuarioService
    {

        public readonly DealerContext _dbContext;

        public UsuarioService(DealerContext _context)
        {
            _dbContext = _context;
        }

        public bool GetUsuarios(Usuario usuario)
        {

        }


    }
}
