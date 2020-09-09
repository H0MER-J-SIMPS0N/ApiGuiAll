using System.Text;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;



namespace ApiGuiAll.Models
{
    [Serializable]
    public class NomenclatureStu3
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("caption")]
        public string Caption { get; set; }
        [JsonProperty("allow_multiple_items")]
        public bool AllowMultipleItems { get; set; }
        [JsonProperty("lab_id")]
        public string LabId { get; set; }
        [JsonProperty("lab_caption")]
        public string LabCaption { get; set; }
        [JsonProperty("group")]
        public string Group { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("patient_preparation")]
        public List<string> PatientPreparation { get; set; }
        [JsonProperty("multiple_specimen")]
        public bool MultipleSpecimen { get; set; }
        [JsonProperty("specimen")]
        public List<SpecimenNomenclature> Specimen { get; set; }
        [JsonProperty("required_specimen")]
        public List<SpecimenNomenclature> RequiredSpecimen { get; set; }
        [JsonProperty("price")]
        public double Price { get; set; }
        private string nomenclatureStu3String;

        public string NomenclatureStu3String
        {
            get
            {
                return $"{LabId} ({Id}) - {Caption}";
            }
            set
            {
                nomenclatureStu3String = value;
            }
        }

        private string patientPreparationString;

        public string PatientPreparationString
        {
            get
            {
                if (PatientPreparation is null)
                {
                    return string.Empty;
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < PatientPreparation.Count; i++)
                    {
                        sb.AppendLine(PatientPreparation[i]);
                    }
                    return sb.ToString();
                }
            }
            set
            {
                patientPreparationString = value;
            }
        }
    }

    public class SpecimenNomenclature
    {
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("specimen_code")]
        public long SpecimenCode { get; set; }
        [JsonProperty("specimen_name")]
        public string SpecimenName { get; set; }
        [JsonProperty("bodysite_code")]
        public string BodysiteCode { get; set; }
        [JsonProperty("bodysite_name")]
        public string BodysiteName { get; set; }
        [JsonProperty("container_type")]
        public string ContainerType { get; set; }
        [JsonProperty("container_name")]
        public string ContainerName { get; set; }

        public string SpecimenString
        {
            get
            {
                return $"description: {Description}\n" +
                       $"specimen_code: {SpecimenCode}\n" +
                       $"specimen_name: {SpecimenName}\n" +
                       $"bodysite_code: {BodysiteCode}\n" +
                       $"bodysite_name: {BodysiteName}\n" +
                       $"container_type: {ContainerType}\n" +
                       $"container_name: {ContainerName}\n";
            }
        }
    }
}