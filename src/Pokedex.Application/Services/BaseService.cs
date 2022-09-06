using Pokedex.Application.Interfaces;
using Pokedex.Application.Notificacoes;
using System.ComponentModel.DataAnnotations;

namespace Pokedex.Domain.Services
{
    public abstract class BaseService
    {
        private readonly INotificador _notificador;

        public BaseService(INotificador notificador)
        {
            _notificador = notificador;
        }
        public virtual void Notificar(string mensagem)
        {
            _notificador.Notificar(new Notificacao(mensagem));
        }
    }
}
