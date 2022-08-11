using Pokedex.Application.Notificacoes;

namespace Pokedex.Test.UnitTests.PokemonServiceTest;

public class NotificadorTest
{   

    [Fact]
    public void ObterNotificacoes()
    {
        //Arrange
        var notificador = new Notificador();        

        notificador.Notificar(new Notificacao("Teste"));
        
        //Act
        var notificacoes = notificador.ObterNotificacoes();
        
        string mensagemEsperada = "Teste";
        string mensagemObtida = notificacoes.Select(m => m.Mensagem).First();

        //Assert
        Assert.Equal(mensagemEsperada, mensagemObtida);
    }

}




