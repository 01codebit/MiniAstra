using Newtonsoft.Json;
using System.Collections.Generic;

namespace model
{
    public enum CodiceFamigliaEnum
    {
        ALTRO = 0,
        AUTOVETTURA = 1,
        AUTOCARRO = 2,
        AUTOMEZZO_FRINGE_BENEFIT = 3,
        EXTRAURBANO = 4,
        NOLEGGIO_CON_CONDUCENTE = 5,
        URBANO = 7,
        VEICOLO_AD_USO_SPECIALE = 11,
        SCUOLABUS = 12,
        VEICOLO_DUMMY = 14,
        TARGA_PROVA = 20,
        ATTREZZATURE_ED_IMPIANTI = 99
    };


    static public class CodiceFamigliaStr
    {
        static public Dictionary<CodiceFamigliaEnum, string> TypeMap = new Dictionary<CodiceFamigliaEnum, string>
        {
            { CodiceFamigliaEnum.ALTRO, "ALTRO" },
            { CodiceFamigliaEnum.AUTOVETTURA, "AUTOVETTURA" },
            { CodiceFamigliaEnum.AUTOCARRO, "AUTOCARRO" },
            { CodiceFamigliaEnum.AUTOMEZZO_FRINGE_BENEFIT, "AUTOMEZZO FRINGE" },
            { CodiceFamigliaEnum.EXTRAURBANO, "EXTRAURBANO" },
            { CodiceFamigliaEnum.NOLEGGIO_CON_CONDUCENTE, "NOLEGGIO C.C." },
            { CodiceFamigliaEnum.URBANO, "URBANO" },
            { CodiceFamigliaEnum.VEICOLO_AD_USO_SPECIALE, "USO SPECIALE" },
            { CodiceFamigliaEnum.SCUOLABUS, "SCUOLABUS" },
            { CodiceFamigliaEnum.VEICOLO_DUMMY, "DUMMY" },
            { CodiceFamigliaEnum.TARGA_PROVA, "TARGA PROVA" },
            { CodiceFamigliaEnum.ATTREZZATURE_ED_IMPIANTI, "ATTR. ED IMP." },
        };
    }



    public class Location
    {
        [JsonProperty("coordinates")]
        public double[] Coordinates { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        public override string ToString()
        {
            string str = "Location { " 
                + "Coordinates: [ ";

            for(int i=0; i<Coordinates.Length; i++)
            {
                str += Coordinates[i]; 
                if(i<Coordinates.Length - 1)
                    str += ", ";
            }

            str += " ]"
                + ", Type: " + this.Type
                + "}";
            
            return str;
        }
    }


    public class Vehicle
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("avm")]
        public bool Avm { get; set; }

        [JsonProperty("codice_famiglia")]
        public CodiceFamigliaEnum CodiceFamiglia { get; set; }

        [JsonProperty("delay")]
        public string Delay { get; set; }
        
        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("release_date")]
        public string MatricolaAziendaleVeicolo { get; set; }

        [JsonProperty("line_ref")]
        public string LineRef { get; set; }

        [JsonProperty("origin_name")]
        public string OriginName { get; set; }

        [JsonProperty("destination_name")]
        public string DestinationName { get; set; }


        public override string ToString()
        {
            return "Vehicle { Id: " + this.Id 
                    + ", Avm: " + this.Avm
                    + ", Delay: " + this.Delay
                    + ", CodiceFamiglia: " + this.CodiceFamiglia
                    + ", MatricolaAziendaleVeicolo: " + this.MatricolaAziendaleVeicolo
                    + ", LineRef: " + this.LineRef
                    + ", OriginName: " + this.OriginName
                    + ", DestinationName: " + this.DestinationName
                    + ", Location: " + this.Location?.ToString()
                    + " }";
        }
    }

}