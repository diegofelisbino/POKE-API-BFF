using Pokedex.Application.Contracts.v1.Responses;
using Pokedex.Application.Models;

namespace Pokedex.Test.Mocks;
public static class PokemonDetailModelMock
{
    public static PokemonDetailModel ObterPikachuModelMock()
    {
        var pikachu = PokemonResponseMock.ObterPikachuResponseMock();            

        return new PokemonDetailModel

        {
            Id = pikachu.Id,
            Nome = pikachu.Name,
            ImagemUri = new Uri(pikachu.Sprites.FrontDefault),
            Peso = pikachu.Weight,
            Altura = pikachu.Height,
            Elementos = pikachu.Types.Select(x => x.Type.Name).ToList()
            //NiveisDePoder = RecuperarNiveisDePoderPorStats(pikachu.Stats)
        };
    }

    public static Dictionary<string, long> RecuperarNiveisDePoderPorStats(List<StatX> stats)
    {
        var niveisDePoder = new Dictionary<string, long>();

        if (stats.Count > 0)
        {
            foreach (var stat in stats)
            {
                niveisDePoder.Add(stat.Stat.Name, stat.BaseStat);
            }

            string maxStat = "max-stat";
            niveisDePoder.Add(maxStat, stats.Select(s => s.BaseStat).Max());
        }

        return niveisDePoder;
    }
}
