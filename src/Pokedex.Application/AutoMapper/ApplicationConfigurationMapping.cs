using AutoMapper;
using Pokedex.Application.Contracts.v1.Responses;
using Pokedex.Application.Models;

namespace Pokedex.Application.AutoMapper
{
    public class ApplicationConfigurationMapping : Profile
    {
        public ApplicationConfigurationMapping()
        {

            CreateMap<Pokemon, PokemonDetailModel>()
                .ForMember(dest => dest.Id, map => map.MapFrom(origem => origem.Id))
                .ForMember(dest => dest.Nome, map => map.MapFrom(origem => origem.Name))
                .ForMember(dest => dest.Peso, map => map.MapFrom(origem => origem.Weight))
                .ForMember(dest => dest.Altura, map => map.MapFrom(origem => origem.Height))
                .ForMember(dest => dest.ImagemUri, map => map.MapFrom(origem => origem.Sprites.FrontDefault))
                .ForMember(dest => dest.Elementos, map => map.MapFrom(origem => origem.Types.Select(x => x.Type.Name)))
                .ForMember(dest => dest.NiveisDePoder, map => map.Ignore());


            CreateMap<PokeList, PokemonListModel>()
                .ForMember(dest => dest.Results, map => map.MapFrom(origem => origem.Results))
                .ForMember(dest => dest.Pokemons, map => map.Ignore()) ;

            /*CreateMap<PokeList, PokemonListPaginadoModel>()
             .ForMember(dest => dest.Proximo, map => map.MapFrom(origem => origem.Next))
             .ForMember(dest => dest.Anterior, map => map.MapFrom(origem => origem.Previous))
             .ForMember(dest => dest.Results, map => map.MapFrom(origem => origem.Results))
             .ForMember(dest => dest.Quantidade, map => map.MapFrom(origem => origem.Count));*/
            
        }

    }
}
