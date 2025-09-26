namespace tl2_tp4_2025_ramiro_2k2.Models
{
    //using CadeteriaLib;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;

    public class DatosJson : IDataProvider
    {
        private readonly string basePath;

        public DatosJson(string basePath)
        {
            this.basePath = basePath;
        }

        private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public Cadeteria GetCadeteria()
        {
            string path = Path.Combine(basePath, "cadeteria.json");
            if (!File.Exists(path))
                throw new FileNotFoundException("No se encontró cadeteria.json");

            string json = File.ReadAllText(path);
            var cadeteria = JsonSerializer.Deserialize<Cadeteria>(json, jsonOptions);
            if (cadeteria == null) throw new Exception("Error deserializando cadeteria.json");
            cadeteria.ListaCadetes ??= new List<Cadete>();
            cadeteria.Pedidos ??= new List<Pedido>();
            return cadeteria;
        }

        public List<Cadete> GetCadetes()
        {
            string path = Path.Combine(basePath, "cadetes.json");
            if (!File.Exists(path))
                throw new FileNotFoundException("No se encontró cadetes.json");

            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<Cadete>>(json, jsonOptions) ?? new List<Cadete>();
        }
    }

}