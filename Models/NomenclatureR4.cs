using System.Reflection;
using System.Runtime.CompilerServices;
using System;
using System.Linq;
using System.Text;
using Hl7.Fhir.Serialization;
using Hl7.Fhir.Model;
using System.Collections.Generic;
using NLog;

namespace ApiGuiAll.Models
{
    public class NomenclatureR4
    {
        public string Hxid { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public bool IsMultipleSpecimen { get; set; }
        public bool IsMultipleAnalysis { get; set; }
        public List<SpecimenR4> SpecimensR4 { get; set; }
        public List<SpecimenR4> RequiredSpecimensR4 { get; set; }
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public string NomenclatureR4String
        {
            get
            {
                return $"{Hxid} - {Caption}";
            }
        }

        public static List<NomenclatureR4> GetNomenclatureR4(string bundleR4Text)
        {
            List<NomenclatureR4> result = new List<NomenclatureR4>();
            var parser = new FhirJsonParser();
            Bundle bundle = parser.Parse<Bundle>(bundleR4Text);
            List<ActivityDefinition> activityDefinitions = GetListOfCurrentResource<ActivityDefinition>(bundle);
            List<SpecimenDefinition> specimenDefinitions = GetListOfCurrentResource<SpecimenDefinition>(bundle);
            List<CatalogEntry> catalogEntries = GetListOfCurrentResource<CatalogEntry>(bundle);
            //List<Questionnaire> questionnaires = GetListOfCurrentResource<Questionnaire>(bundle);
            //List<ObservationDefinition> observationDefinitions = GetListOfCurrentResource<ObservationDefinition>(bundle);
            //logger.Info($"activityDefinitions.Count - {activityDefinitions.Count}");
            foreach (var activityDefinition in activityDefinitions)
            {
                NomenclatureR4 nr4 = new NomenclatureR4();
                string fullUrl = bundle.Entry.Where(x => x.Resource == activityDefinition).Select(x => x.FullUrl).FirstOrDefault();
                //logger.Info($"fullUrl - {fullUrl}");
                if (string.IsNullOrEmpty(fullUrl))
                {
                    throw new Exception("Not found FullUrl for ActivityDefinition!!!");
                }
                CatalogEntry catalogEntry = catalogEntries.Where(x => x.ReferencedItem.Reference == fullUrl).FirstOrDefault();
                //logger.Info($"catalogEntries.Count - {catalogEntries.Count}");
                if (catalogEntry is null)
                {
                    throw new Exception("Not found CatalogEntry for ActivityDefinition!!!");
                }
                //logger.Info($"activityDefinition.SpecimenRequirement.Count - {activityDefinition.SpecimenRequirement.Count}");
                List<ResourceReference> specimenRequirement = activityDefinition.SpecimenRequirement;
                List<SpecimenDefinition> specimensR4Temp = new List<SpecimenDefinition>();
                List<SpecimenDefinition> requiredSpecimensR4Temp = new List<SpecimenDefinition>();
                foreach (ResourceReference reference in specimenRequirement)
                {
                    //logger.Info(reference.ReferenceElement.ToString());
                    SpecimenDefinition specimenR4 = bundle.Entry
                        .Where(x => x.Resource.ResourceType == ResourceType.SpecimenDefinition && x.FullUrl == reference.ReferenceElement.ToString())
                        .Select(y => y.Resource as SpecimenDefinition).FirstOrDefault();

                    if (specimenR4 is null)
                    {
                        //logger.Info("specimenR4 is null");
                        continue;
                    }
                    if (specimenR4.Extension.Where(x => x.Url == "https://helix.ru/extension/required" && (bool)(x.Value as FhirBoolean).Value).Count() > 0)
                    {
                        requiredSpecimensR4Temp.Add(specimenR4);
                    }
                    else
                    {
                        specimensR4Temp.Add(specimenR4);
                    }
                }
                nr4.Hxid = activityDefinition.Identifier.Where(x => x.System == "https://helix.ru/codes/nomenclature").Select(x => x.Value).FirstOrDefault();
                logger.Info($"nr4.Hxid - {nr4.Hxid}");
                if (string.IsNullOrEmpty(nr4.Hxid))
                {
                    throw new Exception("Not found Hxid in ActivityDefinition!!!");
                }
                //logger.Info($"activityDefinition.Title - {activityDefinition.Title}");
                nr4.Caption = activityDefinition.Title;
                nr4.Description = activityDefinition?.Description?.Value ?? "";
                //logger.Info($"nr4.Description - {nr4.Description}");
                nr4.IsMultipleSpecimen = catalogEntry.AdditionalCharacteristic.Where(x => x.Coding.Where(y => y.System == "https://helix.ru/codes/specimen-restrictions" && y.Code == "exactly-one").Count() > 0).Count() == 0;
                //logger.Info($"nr4.IsMultipleSpecimen - {nr4.IsMultipleSpecimen}");
                nr4.IsMultipleAnalysis = catalogEntry.AdditionalCharacteristic.Where(x => x.Coding.Where(y => y.System == "https://helix.ru/codes/nomenclature-restrictions" && y.Code == "at-most-one").Count() > 0).Count() == 0;
                //logger.Info($"nr4.IsMultipleAnalysis - {nr4.IsMultipleAnalysis}");
                //logger.Info($"requiredSpecimensR4Temp.Count - {requiredSpecimensR4Temp.Count}");
                if (requiredSpecimensR4Temp.Count > 0)
                {
                    nr4.RequiredSpecimensR4 = MakeSpecimensR4(requiredSpecimensR4Temp);
                }
                //logger.Info($"specimensR4Temp.Count - {specimensR4Temp.Count}");
                if (specimensR4Temp.Count > 0)
                {
                    nr4.SpecimensR4 = MakeSpecimensR4(specimensR4Temp);
                }
                result.Add(nr4);
            }
            return result;
        }

        public static List<T> GetListOfCurrentResource<T>(Bundle bundle) where T : Resource
        {
            return bundle.Entry.Where(x => x.Resource.TypeName == typeof(T).Name).Select(x => x.Resource as T).ToList();
        }

        // public List<T> GetListOfCurrentResourceByFullUrl<T>(string fullUrl) where T : Resource
        // {
        //     return bundle.Entry.Where(x => x.Resource.TypeName == typeof(T).Name).Select(x => x.Resource as T).ToList();
        // }
        private static List<SpecimenR4> MakeSpecimensR4(List<SpecimenDefinition> specimens)
        {
            List<SpecimenR4> result = new List<SpecimenR4>();
            foreach (var specimen in specimens)
            {
                List<string> scond = new List<string>();
                foreach (var tt in specimen.TypeTested)
                {
                    foreach (var h in tt.Handling)
                    {
                        if (!(h.TemperatureQualifier.Coding.Where(z => z.System == "https://helix.ru/codes/storage-conditions").Select(y => y.Code).ToList() is null))
                        {
                            scond.AddRange(h.TemperatureQualifier.Coding.Where(z => z.System == "https://helix.ru/codes/storage-conditions").Select(y => y.Code).ToList());
                        }
                    }
                }
                List<string> strans = new List<string>();
                foreach (var tt in specimen.TypeTested)
                {
                    foreach (var h in tt.Handling)
                    {
                        if (!(h.TemperatureQualifier.Coding.Where(z => z.System == "https://helix.ru/codes/sample-transport").Select(y => y.Code).ToList() is null))
                        {
                            strans.AddRange(h.TemperatureQualifier.Coding.Where(z => z.System == "https://helix.ru/codes/sample-transport").Select(y => y.Code).ToList());
                        }
                    }
                }
                result.Add(new SpecimenR4()
                {
                    Group = specimen.Extension.Where(x => x.Url == "https://helix.ru/extension/sample-group").Select(x => x.Value.ToString()).FirstOrDefault(),
                    IsDestructive = specimen.Extension.Where(x => x.Url == "https://helix.ru/extension/distructive").Select(x => (bool)(x.Value as FhirBoolean).Value).FirstOrDefault(),
                    Rules = specimen.Identifier.Value,
                    SpecimenTypeR4 = specimen.TypeCollected.Text + " (" + specimen.TypeCollected.Coding.Where(x => x.System == "https://helix.ru/codes/sample-type").Select(x => x.Code).FirstOrDefault() + ")",
                    Protocol = specimen.TypeCollected.Coding.Where(x => x.System == "https://helix.ru/codes/sample-protocol").Select(x => x.Code).FirstOrDefault(),
                    ContainerType = specimen.TypeTested.Select(x => x.Container.Type.Coding.Where(y => y.System == "https://helix.ru/codes/container-type").Select(y => y.Code).FirstOrDefault()).FirstOrDefault(),
                    Volume = specimen.TypeTested.Select(x => (x.Container.MinimumVolume as Quantity).Value.ToString()).FirstOrDefault(),
                    StorageConditions = scond,
                    SampleTransport = strans
                });
            }
            return result;
        }
    }


    public class SpecimenR4
    {
        public string Group { get; set; }
        public bool IsDestructive { get; set; }
        public string Rules { get; set; }
        public string SpecimenTypeR4 { get; set; }
        public string Protocol { get; set; }
        public string ContainerType { get; set; }
        public string Volume { get; set; }
        public List<string> StorageConditions { get; set; }
        public List<string> SampleTransport { get; set; }

        public string SpecimenString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"Группа: {Group}");
                sb.AppendLine($"Диструктивный: {IsDestructive.ToString()}");
                sb.AppendLine($"Правила: {Rules}");
                sb.AppendLine($"Тип биоматериала: {SpecimenTypeR4}");
                sb.AppendLine($"Протокол: {Protocol}");
                sb.AppendLine($"Тип контейнера: {ContainerType}");
                sb.AppendLine($"Объем: {Volume}");
                sb.AppendLine($"Условия хранения:");
                sb.AppendLine(string.Join("\r\n", StorageConditions));
                sb.AppendLine($"Условия транспортировки:");
                sb.Append(string.Join("\r\n", SampleTransport));
                return sb.ToString();
            }
        }
    }

}