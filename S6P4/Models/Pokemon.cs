
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace S6P4.Models
{
    // Respuesta de la lista de Pokémon
    public class PokemonListResponse
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("results")]
        public List<PokemonRef> Results { get; set; }
    }

    public class PokemonRef
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    // Información detallada de cada Pokémon
    public class PokemonDetail
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("weight")]
        public int Weight { get; set; }

        [JsonProperty("base_experience")]
        public int? BaseExperience { get; set; }

        [JsonProperty("types")]
        public List<TypeSlot> Types { get; set; }

        [JsonProperty("stats")]
        public List<StatSlot> Stats { get; set; }

        [JsonProperty("abilities")]
        public List<AbilitySlot> Abilities { get; set; }

        [JsonProperty("sprites")]
        public Sprites Sprites { get; set; }

        [JsonIgnore]
        public string NombreVisible
        {
            get
            {
                return string.IsNullOrEmpty(Name)
                    ? ""
                    : char.ToUpper(Name[0]) +
                      Name.Substring(1).Replace("-", " ");
            }
        }

        [JsonIgnore]
        public string TipoPrincipal
        {
            get
            {
                return Types != null && Types.Any()
                    ? Types.OrderBy(t => t.Slot).First().Type.Name
                    : "normal";
            }
        }

        [JsonIgnore]
        public string ImagenUrl
        {
            get
            {
                if (Sprites == null) return "";

                var art = Sprites.Other != null &&
                          Sprites.Other.OfficialArtwork != null
                    ? Sprites.Other.OfficialArtwork.FrontDefault
                    : null;

                return art ?? Sprites.FrontDefault ?? "";
            }
        }

        [JsonIgnore]
        public double AlturaMetros
        {
            get { return Height / 10.0; }
        }

        [JsonIgnore]
        public double PesoKg
        {
            get { return Weight / 10.0; }
        }

        [JsonIgnore]
        public string NumeroFormateado
        {
            get { return "#" + Id.ToString("000"); }
        }
    }

    public class TypeSlot
    {
        [JsonProperty("slot")]
        public int Slot { get; set; }

        [JsonProperty("type")]
        public NamedResource Type { get; set; }
    }

    public class StatSlot
    {
        [JsonProperty("base_stat")]
        public int BaseStat { get; set; }

        [JsonProperty("stat")]
        public NamedResource Stat { get; set; }
    }

    public class AbilitySlot
    {
        [JsonProperty("is_hidden")]
        public bool IsHidden { get; set; }

        [JsonProperty("ability")]
        public NamedResource Ability { get; set; }
    }

    public class NamedResource
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class Sprites
    {
        [JsonProperty("front_default")]
        public string FrontDefault { get; set; }

        [JsonProperty("other")]
        public OtherSprites Other { get; set; }
    }

    public class OtherSprites
    {
        [JsonProperty("official-artwork")]
        public OfficialArtwork OfficialArtwork { get; set; }
    }

    public class OfficialArtwork
    {
        [JsonProperty("front_default")]
        public string FrontDefault { get; set; }
    }

    // Información de la especie
    public class PokemonSpecies
    {
        [JsonProperty("flavor_text_entries")]
        public List<FlavorTextEntry> FlavorTextEntries { get; set; }
    }

    public class FlavorTextEntry
    {
        [JsonProperty("flavor_text")]
        public string FlavorText { get; set; }

        [JsonProperty("language")]
        public NamedResource Language { get; set; }
    }

    // Modelos para las vistas
    public class PokemonListaViewModel
    {
        public List<PokemonDetail> Pokemones { get; set; }
            = new List<PokemonDetail>();

        public int Pagina { get; set; }
        public int TotalPaginas { get; set; }
        public int TotalPokemon { get; set; }
        public string Error { get; set; }
    }

    public class PokemonDetalleViewModel
    {
        public PokemonDetail Pokemon { get; set; }
        public string Descripcion { get; set; }
        public string Error { get; set; }
    }

    // Traducciones al español
    public static class PokemonHelper
    {
        private static readonly Dictionary<string, string> Tipos =
            new Dictionary<string, string>
        {
            {"normal","Normal"},
            {"fire","Fuego"},
            {"water","Agua"},
            {"electric","Eléctrico"},
            {"grass","Planta"},
            {"ice","Hielo"},
            {"fighting","Lucha"},
            {"poison","Veneno"},
            {"ground","Tierra"},
            {"flying","Volador"},
            {"psychic","Psíquico"},
            {"bug","Bicho"},
            {"rock","Roca"},
            {"ghost","Fantasma"},
            {"dragon","Dragón"},
            {"dark","Siniestro"},
            {"steel","Acero"},
            {"fairy","Hada"}
        };

        private static readonly Dictionary<string, string> Stats =
            new Dictionary<string, string>
        {
            {"hp","PS"},
            {"attack","Ataque"},
            {"defense","Defensa"},
            {"special-attack","At. Esp."},
            {"special-defense","Def. Esp."},
            {"speed","Velocidad"}
        };

        public static string TipoEs(string tipo)
        {
            string v;
            return tipo != null && Tipos.TryGetValue(tipo, out v)
                ? v : tipo;
        }

        public static string StatEs(string stat)
        {
            string v;
            return stat != null && Stats.TryGetValue(stat, out v)
                ? v : stat;
        }

        public static int PorcentajeStat(int valor)
        {
            return (int)Math.Min(
                100,
                Math.Round(valor * 100.0 / 160.0)
            );
        }
    }
}
