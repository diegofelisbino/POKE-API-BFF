using Pokedex.Application.Interfaces;
using Pokedex.Application.Notificacoes;
using System.ComponentModel.DataAnnotations;

namespace Pokedex.Domain.Services
{
    public abstract class BaseService
    {
        private readonly INotificador _notificador;

        protected BaseService(INotificador notificador)
        {
            _notificador = notificador;
        }
        protected void Notificar(string mensagem)
        {
            _notificador.Notificar(new Notificacao(mensagem));
        }
    }
}
