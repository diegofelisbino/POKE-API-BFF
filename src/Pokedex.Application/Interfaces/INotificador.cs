using Pokedex.Application.Notificacoes;


namespace Pokedex.Application.Interfaces
{
    public interface INotificador
    {
        bool TemNotificacao();
        List<Notificacao> ObterNotificacoes();
        void Notificar(Notificacao mensagem);
    }
}
