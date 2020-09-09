using System.Threading;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using ApiGuiAll.Models;
using NLog;
using ReactiveUI;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace ApiGuiAll.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public ObservableCollection<Setting> Settings { get; set; }
        public ObservableCollection<string> HttpMethods { get; set; }
        public ObservableCollection<Header> Headers { get; set; }
        public ObservableCollection<GetRequestsSetting> GetRequestsList { get; set; }
        public ObservableCollection<PostRequestsSetting> PostRequestsList { get; set; }
        public ObservableCollection<GetRequestsSetting> SelectedGetRequests { get; set; }
        public ObservableCollection<PostRequestsSetting> SelectedPostRequests { get; set; }
        public IObservable<bool> canExecuteGetRequest { get; set; }
        public ReactiveCommand<System.Reactive.Unit, System.Reactive.Unit> SendGetRequestCommand { get; set; }
        public IObservable<bool> canExecutePostRequest { get; set; }
        public ReactiveCommand<System.Reactive.Unit, System.Reactive.Unit> SendPostRequestCommand { get; set; }
        public IObservable<bool> canExecuteOtherRequest { get; set; }
        public ReactiveCommand<System.Reactive.Unit, System.Reactive.Unit> SendOtherRequestCommand { get; set; }
        public IObservable<bool> canExecuteNomenclatureRequest { get; set; }
        public ReactiveCommand<System.Reactive.Unit, System.Reactive.Unit> SendNomenclatureRequestCommand { get; set; }
        public IObservable<bool> canExecuteFindNomenclature { get; set; }
        public ReactiveCommand<System.Reactive.Unit, System.Reactive.Unit> FindNomenclatureCommand { get; set; }
        public IObservable<bool> canExecuteNomenclatureR4Request { get; set; }
        public ReactiveCommand<System.Reactive.Unit, System.Reactive.Unit> SendNomenclatureR4RequestCommand { get; set; }
        public IObservable<bool> canExecuteFindNomenclatureR4 { get; set; }
        public ReactiveCommand<System.Reactive.Unit, System.Reactive.Unit> FindNomenclatureR4Command { get; set; }
        public IObservable<bool> canExecuteFindTasksByLabelId { get; set; }
        public ReactiveCommand<System.Reactive.Unit, System.Reactive.Unit> FindTasksByLabelIdCommand { get; set; }

        private bool isWaiting;
        public bool IsWaiting
        {
            get
            {
                return isWaiting;
            }
            set
            {

                this.RaiseAndSetIfChanged(ref isWaiting, value);
            }
        }

        private bool isWaitingR4;
        public bool IsWaitingR4
        {
            get
            {
                return isWaitingR4;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref isWaitingR4, value);
            }
        }

        private bool isValueNomenclatureContractEnterFocused;
        public bool IsValueNomenclatureContractEnterFocused
        {
            get
            {
                return isValueNomenclatureContractEnterFocused;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref isValueNomenclatureContractEnterFocused, value);
            }
        }

        private bool isValueNomenclatureContractEnterFocusedR4;
        public bool IsValueNomenclatureContractEnterFocusedR4
        {
            get
            {
                return isValueNomenclatureContractEnterFocusedR4;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref isValueNomenclatureContractEnterFocusedR4, value);
            }
        }

        private bool isValueFindNomenclatureEnterFocused;
        public bool IsValueFindNomenclaturetEnterFocused
        {
            get
            {
                return isValueFindNomenclatureEnterFocused;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref isValueFindNomenclatureEnterFocused, value);
            }
        }

        private bool isValueFindNomenclatureEnterFocusedR4;
        public bool IsValueFindNomenclatureEnterFocusedR4
        {
            get
            {
                return isValueFindNomenclatureEnterFocusedR4;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref isValueFindNomenclatureEnterFocusedR4, value);
            }
        }

        private List<NomenclatureStu3> nomenclature;
        public List<NomenclatureStu3> Nomenclature
        {
            get
            {
                return nomenclature;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref nomenclature, value);
            }
        }

        private List<NomenclatureR4> nomenclatureListR4;
        public List<NomenclatureR4> NomenclatureListR4
        {
            get
            {
                return nomenclatureListR4;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref nomenclatureListR4, value);
            }
        }
        private List<NomenclatureStu3> foundNomenclature;
        public List<NomenclatureStu3> FoundNomenclature
        {
            get
            {
                return foundNomenclature;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref foundNomenclature, value);
            }
        }

        private List<NomenclatureR4> foundNomenclatureR4;
        public List<NomenclatureR4> FoundNomenclatureR4
        {
            get
            {
                return foundNomenclatureR4;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref foundNomenclatureR4, value);
            }
        }

        private string labelId;
        public string LabelId
        {
            get
            {
                return labelId;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref labelId, value);
            }
        }

        private string valueNomenclatureContract;
        public string ValueNomenclatureContract
        {
            get
            {
                return valueNomenclatureContract;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref valueNomenclatureContract, value);
            }
        }

        private string valueNomenclatureContractR4;
        public string ValueNomenclatureContractR4
        {
            get
            {
                return valueNomenclatureContractR4;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref valueNomenclatureContractR4, value);
            }
        }
        private string valueFindNomenclature;
        public string ValueFindNomenclature
        {
            get
            {
                return valueFindNomenclature;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref valueFindNomenclature, value);
            }
        }

        private string valueFindNomenclatureR4;
        public string ValueFindNomenclatureR4
        {
            get
            {
                return valueFindNomenclatureR4;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref valueFindNomenclatureR4, value);
            }
        }
        private NomenclatureStu3 selectedFoundNomenclature;
        public NomenclatureStu3 SelectedFoundNomenclature
        {
            get
            {
                return selectedFoundNomenclature;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref selectedFoundNomenclature, value);
            }
        }

        private NomenclatureR4 selectedFoundNomenclatureR4;
        public NomenclatureR4 SelectedFoundNomenclatureR4
        {
            get
            {
                return selectedFoundNomenclatureR4;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref selectedFoundNomenclatureR4, value);
            }
        }


        public ObservableCollection<string> HeaderContentType { get; set; }

        private string selectedHeaderContentType;

        public string SelectedHeaderContentType
        {
            get
            {
                return selectedHeaderContentType;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref selectedHeaderContentType, value);
            }
        }

        private string selectedMethod;

        public string SelectedMethod
        {
            get
            {
                return selectedMethod;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref selectedMethod, value);
            }
        }

        private string filePath = default;
        public string FilePath
        {
            get
            {
                return filePath;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref filePath, value);
            }
        }
        public bool isFilePath = default;
        public bool IsFilePath
        {
            get
            {
                return isFilePath;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref isFilePath, value);
            }
        }

        public bool isAttributesFilled = false;
        public bool IsAttributesFilled
        {
            get
            {
                return isAttributesFilled = (IsSecondAttributePostRequest && (!string.IsNullOrEmpty(ValueWhatToEnterOne) && !string.IsNullOrEmpty(ValueWhatToEnterTwo))) || (!IsSecondAttributePostRequest && (!string.IsNullOrEmpty(ValueWhatToEnterOne)));
            }
            set
            {
                this.RaiseAndSetIfChanged(ref isAttributesFilled, value);
            }
        }

        private Setting chosenSetting;

        public Setting ChosenSetting
        {
            get
            {
                return chosenSetting;
            }
            set
             {
                if (value != null && value != chosenSetting)
                {
                    this.RaiseAndSetIfChanged(ref chosenSetting, value);
                    ValueToTransfer = string.Empty;
                    SelectedToken = GetToken.Get(value);
                    GetRequestsList = RequestsSettings.Get<GetRequestsSetting>("GetRequests.json");
                    if (!(GetRequestsList is null))
                    {
                        if (!(SelectedGetRequests is null))
                        {
                            SelectedGetRequests.Clear();
                        }
                        else
                        {
                            SelectedGetRequests = new ObservableCollection<GetRequestsSetting>();
                        }
                        List<GetRequestsSetting> selectedGetRequests = GetRequestsList.Where(x => x.Zone.Contains(value.Zone)).ToList();
                        if (selectedGetRequests.Count > 0)
                        {
                            foreach (GetRequestsSetting s in selectedGetRequests)
                            {
                                SelectedGetRequests.Add(s);
                            }
                        }
                        ChosenGetRequestsSetting = SelectedGetRequests.Count > 0 ? SelectedGetRequests[0] : null;
                    }
                    PostRequestsList = RequestsSettings.Get<PostRequestsSetting>("PostRequests.json");
                    if (!(PostRequestsList is null))
                    {
                        if (!(SelectedPostRequests is null))
                        {
                            SelectedPostRequests.Clear();
                        }
                        else
                        {
                            SelectedPostRequests = new ObservableCollection<PostRequestsSetting>();
                        }
                        List<PostRequestsSetting> selectedPostRequests = PostRequestsList.Where(x => x.Zone.Contains(value.Zone)).ToList();
                        if (selectedPostRequests.Count > 0)
                        {
                            foreach (PostRequestsSetting s in selectedPostRequests)
                            {
                                SelectedPostRequests.Add(s);
                            }
                        }
                        ChosenPostRequestsSetting = SelectedPostRequests.Count > 0 ? SelectedPostRequests[0] : null;
                    }
                }
            }
        }

        private Token selectedToken;

        public Token SelectedToken
        {
            get
            {
                return selectedToken;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref selectedToken, value);
            }
        }


        private GetRequestsSetting chosenGetRequestsSetting;

        public GetRequestsSetting ChosenGetRequestsSetting
        {
            get
            {
                return chosenGetRequestsSetting;
            }
            set
            {
                if (value != null && value != chosenGetRequestsSetting)
                {
                    this.RaiseAndSetIfChanged(ref chosenGetRequestsSetting, value);
                    ChosenByWhatGetSettings = value?.ByWhat[0];
                    ValueToTransfer = string.Empty;
                }
            }
        }

        private ByWhat chosenByWhatGetSettings;

        public ByWhat ChosenByWhatGetSettings
        {
            get
            {
                return chosenByWhatGetSettings;
            }
            set
            {
                if (value != null && value != chosenByWhatGetSettings)
                {
                    this.RaiseAndSetIfChanged(ref chosenByWhatGetSettings, value);
                    ValueToTransfer = string.Empty;
                }
            }
        }

        private string valueToTransfer;

        public string ValueToTransfer
        {
            get
            {
                return valueToTransfer;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref valueToTransfer, value);
            }
        }

        private PostRequestsSetting chosenPostRequestsSetting;

        public PostRequestsSetting ChosenPostRequestsSetting
        {
            get
            {
                return chosenPostRequestsSetting;
            }
            set
            {
                if (value != null && value != chosenPostRequestsSetting)
                {
                    this.RaiseAndSetIfChanged(ref chosenPostRequestsSetting, value);
                    ValueWhatToEnterTwo = string.Empty;
                    IsSecondAttributePostRequest = !string.IsNullOrEmpty(value.WithWhat.WhatToEnterTwo);
                    IsAttributesFilled = (IsSecondAttributePostRequest && (!string.IsNullOrEmpty(ValueWhatToEnterOne) && !string.IsNullOrEmpty(ValueWhatToEnterTwo))) || (!IsSecondAttributePostRequest && (!string.IsNullOrEmpty(ValueWhatToEnterOne)));
                }
            }
        }
        private bool isSecondAttributePostRequest;

        public bool IsSecondAttributePostRequest
        {
            get
            {
                //logger.Info($"Is WhatToEnterTwo for {ChosenPostRequestsSetting.What} - {!string.IsNullOrEmpty(ChosenPostRequestsSetting.WithWhat.WhatToEnterTwo)}");
                return isSecondAttributePostRequest;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref isSecondAttributePostRequest, !string.IsNullOrEmpty(ChosenPostRequestsSetting.WithWhat.WhatToEnterTwo));
            }
        }

        private string valueWhatToEnterOne;

        public string ValueWhatToEnterOne
        {
            get
            {
                return valueWhatToEnterOne;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref valueWhatToEnterOne, value);
                IsAttributesFilled = (IsSecondAttributePostRequest && (!string.IsNullOrEmpty(ValueWhatToEnterOne) && !string.IsNullOrEmpty(ValueWhatToEnterTwo))) || (!IsSecondAttributePostRequest && (!string.IsNullOrEmpty(ValueWhatToEnterOne)));
            }
        }

        private string valueWhatToEnterTwo;

        public string ValueWhatToEnterTwo
        {
            get
            {
                return valueWhatToEnterTwo;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref valueWhatToEnterTwo, value);
                IsAttributesFilled = (IsSecondAttributePostRequest && (!string.IsNullOrEmpty(ValueWhatToEnterOne) && !string.IsNullOrEmpty(ValueWhatToEnterTwo))) || (!IsSecondAttributePostRequest && (!string.IsNullOrEmpty(ValueWhatToEnterOne)));
            }
        }

        private string valueOtherPost;

        public string ValueOtherPost
        {
            get
            {
                return valueOtherPost;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref valueOtherPost, value);
            }
        }

        private string answerText;

        public string AnswerText
        {
            get
            {
                return answerText;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref answerText, value);
            }
        }

        private string valueOther;

        public string ValueOther
        {
            get
            {
                return valueOther;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref valueOther, value);
            }
        }

        public MainWindowViewModel()
        {
            Settings = new ObservableCollection<Setting>(GetSettings.Get());
            if (Settings is null)
            {
                throw new Exception("Не прочитать настройки!");
            }
            Directory.CreateDirectory("Downloads");
            Directory.CreateDirectory("Responses");
            IsWaiting = false;
            IsFilePath = false;
            HttpMethods = new ObservableCollection<string>(new List<string>() { "Get", "Post" });
            SelectedMethod = HttpMethods[0];
            canExecuteGetRequest = this.WhenAnyValue(x => x.ValueToTransfer, x => x.SelectedToken.StatusCode, (vtt, sc) => !string.IsNullOrEmpty(vtt) && sc != "exception");
            SendGetRequestCommand = ReactiveCommand.CreateFromTask(async () => await Task.Factory.StartNew(() => SendGetRequest()), canExecuteGetRequest);
            canExecutePostRequest = this.WhenAnyValue(x => x.IsAttributesFilled, x => x.SelectedToken.StatusCode, (iaf, sc) => iaf && sc != "exception");
            SendPostRequestCommand = ReactiveCommand.CreateFromTask(async () => await Task.Factory.StartNew(() => SendPostRequest()), canExecutePostRequest);
            canExecuteOtherRequest = this.WhenAnyValue(x => x.ValueOther, x => x.SelectedMethod, x => x.ValueOtherPost, x => x.SelectedToken.StatusCode, (vo, sm, vop, sc) => (sm == "Get" && !string.IsNullOrEmpty(vo) && sc != "exception") || (sm == "Post" && !string.IsNullOrEmpty(vo) && !string.IsNullOrEmpty(vop) && sc != "exception"));
            SendOtherRequestCommand = ReactiveCommand.CreateFromTask(async () => await Task.Factory.StartNew(() => SendOtherRequest()), canExecuteOtherRequest);
            canExecuteNomenclatureRequest = this.WhenAnyValue(x => x.ValueNomenclatureContract, x => x.SelectedToken.StatusCode, (vnc, sc) => !string.IsNullOrEmpty(vnc) && vnc.Length == 10 && Regex.IsMatch(vnc.ToUpper(), "C[0-9]{9,9}") && sc != "exception");
            SendNomenclatureRequestCommand = ReactiveCommand.CreateFromTask(async () =>  await Task.Factory.StartNew(() => SendNomenclatureRequest()), canExecuteNomenclatureRequest);
            canExecuteFindNomenclature = this.WhenAnyValue(x => x.ValueFindNomenclature, x => x.Nomenclature, x => x.SelectedToken.StatusCode, (vfn, nom, sc) => !string.IsNullOrEmpty(vfn) && vfn.Length > 2 && !(nom is null) && sc != "exception");// && vfn.Length > 2);
            FindNomenclatureCommand = ReactiveCommand.CreateFromTask(async () => await Task.Factory.StartNew(() => FindNomenclature()), canExecuteFindNomenclature);
            canExecuteNomenclatureR4Request = this.WhenAnyValue(x => x.ValueNomenclatureContractR4, x => x.SelectedToken.StatusCode, (vnc, sc) => !string.IsNullOrEmpty(vnc) && vnc.Length == 10 && Regex.IsMatch(vnc.ToUpper(), "C[0-9]{9,9}") && sc != "exception");
            SendNomenclatureR4RequestCommand = ReactiveCommand.CreateFromTask(async () => await Task.Factory.StartNew(() => SendNomenclatureRequestR4()), canExecuteNomenclatureR4Request);
            canExecuteFindNomenclatureR4 = this.WhenAnyValue(x => x.ValueFindNomenclatureR4, x => x.NomenclatureListR4, x => x.SelectedToken.StatusCode, (vfn, nom, sc) => !string.IsNullOrEmpty(vfn) && vfn.Length > 2 && !(nom is null) && sc != "exception");// && vfn.Length > 2);
            FindNomenclatureR4Command = ReactiveCommand.CreateFromTask(async () => await Task.Factory.StartNew(() => FindNomenclatureR4()), canExecuteFindNomenclatureR4);
            canExecuteFindTasksByLabelId = this.WhenAnyValue
                (
                    x => x.LabelId, x => x.ChosenSetting.Zone, (li, z) => 
                    !string.IsNullOrWhiteSpace(li) && 
                    (li.Length == 10 || li.Length == 16 ) && 
                    (Regex.IsMatch(li, "[0-9]{10,10}") || Regex.IsMatch(li, "[0-9]{16,16}") )
                );
            FindTasksByLabelIdCommand = ReactiveCommand.CreateFromTask(async () =>  await Task.Factory.StartNew(() => GetOrderProcessingTaskByLabel()), canExecuteFindTasksByLabelId);
            ChosenSetting = Settings[0];
            SelectedToken = GetToken.Get(ChosenSetting);
            var contType = new List<string>();
            List<Header> heads = GetHeaders.Get(out contType);
            HeaderContentType = new ObservableCollection<string>(contType);
            selectedHeaderContentType = HeaderContentType[0];
            Headers = new ObservableCollection<Header>(heads);
        }

        public void SendGetRequest()
        {
            IsFilePath = false;
            IsWaiting = true;
            string address = new Uri(new Uri(ChosenSetting.BaseUrl), Path.Combine(ChosenByWhatGetSettings.Endpoint.Replace(ChosenByWhatGetSettings.Argument, ValueToTransfer))).ToString();
            try
            {
                GetHttpClient.Get().DefaultRequestHeaders.Clear();
                GetHttpClient.Get().DefaultRequestHeaders.TryAddWithoutValidation("Authorization", GetToken.Get(ChosenSetting).Value);
                using (var response = GetHttpClient.Get().GetAsync(address).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string resp = response.Content.ReadAsStringAsync().Result;
                        if (resp.Length < 20000)
                        {
                            AnswerText = FormatJson.Process(resp);
                        }
                        else
                        {
                            bool isFilePath = IsFilePath;
                            string filePath = FilePath;
                            AnswerText = ResponseFile.Save(resp, ref isFilePath, ref filePath);
                            IsFilePath = isFilePath;
                            FilePath = filePath;
                        }
                    }
                    else
                    {
                        throw new Exception(response.Content.ReadAsStringAsync().Result + "\n\n" + response.ToString());
                    }

                }
            }
            catch (Exception ex)
            {
                AnswerText = $"{address}\r\n{ex}";
                logger.Error($"GET REQUEST: {AnswerText}");
            }
            finally
            {
                IsWaiting = false;
            }
        }

        public void OpenFile()
        {
            try
            {
                using (Process proc = new Process())
                {
                    proc.StartInfo.FileName = FilePath;
                    proc.StartInfo.UseShellExecute = true;
                    proc.Start();
                }
            }
            catch (Exception ex) { logger.Error($"{ex}"); }
        }

        public void SendPostRequest()
        {
            IsFilePath = false;
            IsWaiting = true;
            string address = IsSecondAttributePostRequest ?
                                new Uri(new Uri(ChosenSetting.BaseUrl), Path.Combine(ChosenPostRequestsSetting.WithWhat.Endpoint.Replace(ChosenPostRequestsSetting.WithWhat.ArgumentOne, ValueWhatToEnterOne).Replace(ChosenPostRequestsSetting.WithWhat.ArgumentTwo, ValueWhatToEnterTwo))).ToString() :
                                new Uri(new Uri(ChosenSetting.BaseUrl), Path.Combine(ChosenPostRequestsSetting.WithWhat.Endpoint.Replace(ChosenPostRequestsSetting.WithWhat.ArgumentOne, ValueWhatToEnterOne))).ToString();
            logger.Info(address);
            string postData = IsSecondAttributePostRequest ?
                            ChosenPostRequestsSetting.WithWhat.PostData.Replace(ChosenPostRequestsSetting.WithWhat.ArgumentOne, ValueWhatToEnterOne).Replace(ChosenPostRequestsSetting.WithWhat.ArgumentTwo, ValueWhatToEnterTwo) :
                            ChosenPostRequestsSetting.WithWhat.PostData.Replace(ChosenPostRequestsSetting.WithWhat.ArgumentOne, ValueWhatToEnterOne);
            logger.Info(postData);
            Dictionary<string, string> headers = ChosenPostRequestsSetting.WithWhat.Headers;
            try
            {
                GetHttpClient.Get().DefaultRequestHeaders.Clear();
                GetHttpClient.Get().DefaultRequestHeaders.TryAddWithoutValidation("Authorization", GetToken.Get(ChosenSetting).Value);
                try { GetHttpClient.Get().DefaultRequestHeaders.Remove("Content-Type"); } catch (Exception ex) { logger.Warn(ex.ToString()); }
                foreach (string key in headers.Keys)
                {
                    GetHttpClient.Get().DefaultRequestHeaders.TryAddWithoutValidation(key, headers[key]);
                }
                var content = new StringContent(postData, Encoding.UTF8, headers["Content-Type"]);
                logger.Info("Content Headers: " + content.Headers);
                using (HttpResponseMessage response = GetHttpClient.Get().PostAsync(address, content).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string resp = FormatJson.Process(response.Content.ReadAsStringAsync().Result);
                        if (resp.Length < 20000)
                        {
                            AnswerText = FormatJson.Process(resp);
                        }
                        else
                        {
                            bool isFilePath = IsFilePath;
                            string filePath = FilePath;
                            AnswerText = ResponseFile.Save(resp, ref isFilePath, ref filePath);
                            IsFilePath = isFilePath;
                            FilePath = filePath;
                        }
                    }
                    else
                    {
                        throw new Exception(response.Content.ReadAsStringAsync().Result + "\n\n" + response.ToString());
                    }

                }
            }
            catch (Exception exc)
            {
                AnswerText = "Не удалось выполнить POST-запрос:\r\n" + exc.ToString();
                logger.Error("Не удалось выполнить POST-запрос:\r\n" + exc.ToString());
            }
            finally
            {
                IsWaiting = false;
            }
        }

        public void SendOtherRequest()
        {
            IsFilePath = false;
            IsWaiting = true;
            string address = ValueOther;
            if (SelectedMethod == "Get")
            {
                try
                {
                    GetHttpClient.Get().DefaultRequestHeaders.Clear();
                    GetHttpClient.Get().DefaultRequestHeaders.TryAddWithoutValidation("Authorization", GetToken.Get(ChosenSetting).Value);
                    List<HeaderItem> headerItems;
                    string valueHeader;
                    foreach (Header h in Headers)
                    {
                        headerItems = h.HeaderItems;
                        valueHeader = string.Empty;
                        foreach (HeaderItem headerItem in headerItems)
                        {
                            if (headerItem.IsChecked)
                            {
                                valueHeader += headerItem.HeaderValue + ",";
                            }
                        }
                        if (valueHeader.Length > 3)
                        {
                            GetHttpClient.Get().DefaultRequestHeaders.TryAddWithoutValidation(h.HeaderName, valueHeader.Trim(','));
                            //logger.Info($"Header: {h.HeaderName} - {valueHeader.Trim(',')}");
                        }
                    }
                    using (var response = GetHttpClient.Get().GetAsync(address).Result)
                    {
                        if (response.IsSuccessStatusCode && response.Content.Headers.Contains("Content-Disposition"))
                        {
                            var contentDisposition = response.Content.Headers.Where(x => x.Key == "Content-Disposition").Select(x => x.Value).FirstOrDefault();
                            string fileName;
                            foreach (var contDisp in contentDisposition)
                            {
                                if (contDisp.Contains("attachment"))
                                {
                                    fileName = contDisp.Split(';')
                                                .Where(x => x.Trim().ToUpper().StartsWith("FILENAME="))
                                                .Select(x => x.ToUpper().Replace("FILENAME=", string.Empty)).FirstOrDefault();
                                    using (var fs = new FileStream(Path.Combine("Downloads", fileName), FileMode.Create))
                                    {
                                        response.Content.CopyToAsync(fs).Wait();
                                        AnswerText = $"В ответ прислан файл, Он сохранен в папке ./Downloads.\r\nИмя файла '{fileName}'";
                                        IsFilePath = true;
                                        FilePath = Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("Downloads", fileName));
                                    }
                                }
                            }
                        }
                        else if (response.IsSuccessStatusCode)
                        {
                            string resp = FormatJson.Process(response.Content.ReadAsStringAsync().Result);
                            if (resp.Length < 20000)
                            {
                                AnswerText = FormatJson.Process(resp);
                            }
                            else
                            {
                                bool isFilePath = IsFilePath;
                                string filePath = FilePath;
                                AnswerText = ResponseFile.Save(resp, ref isFilePath, ref filePath);
                                IsFilePath = isFilePath;
                                FilePath = filePath;
                            }
                        }
                        else
                        {
                            throw new Exception(response.Content.ReadAsStringAsync().Result + "\n\n" + response.ToString());
                        }
                    }

                }
                catch (Exception ex)
                {
                    AnswerText = $"{address}\r\n{ex.ToString()}";
                    logger.Error($"GET REQUEST: {AnswerText}");
                }
                finally
                {
                    IsWaiting = false;
                }
            }
            else if (SelectedMethod == "Post")
            {
                try
                {
                    GetHttpClient.Get().DefaultRequestHeaders.Clear();
                    GetHttpClient.Get().DefaultRequestHeaders.TryAddWithoutValidation("Authorization", GetToken.Get(ChosenSetting).Value);
                    List<HeaderItem> headerItems;
                    string valueHeader;
                    foreach (Header h in Headers)
                    {
                        headerItems = h.HeaderItems;
                        valueHeader = string.Empty;
                        foreach (HeaderItem headerItem in headerItems)
                        {
                            if (headerItem.IsChecked)
                            {
                                valueHeader += headerItem.HeaderValue + ",";
                            }
                        }
                        if (valueHeader.Length > 3)
                        {
                            GetHttpClient.Get().DefaultRequestHeaders.TryAddWithoutValidation(h.HeaderName, valueHeader.Trim(','));
                        }
                    }
                    var content = new StringContent(ValueOtherPost, Encoding.UTF8, SelectedHeaderContentType);
                    using (HttpResponseMessage response = GetHttpClient.Get().PostAsync(address, content).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string resp = FormatJson.Process(response.Content.ReadAsStringAsync().Result);
                            if (resp.Length < 20000)
                            {
                                AnswerText = FormatJson.Process(resp);
                            }
                            else
                            {
                                bool isFilePath = IsFilePath;
                                string filePath = FilePath;
                                AnswerText = ResponseFile.Save(resp, ref isFilePath, ref filePath);
                                IsFilePath = isFilePath;
                                FilePath = filePath;
                            }
                        }
                        else
                        {
                            throw new Exception(response.Content.ReadAsStringAsync().Result + "\n\n" + response.ToString());
                        }
                    }

                }
                catch (Exception exc)
                {
                    AnswerText = "Не удалось выполнить POST-запрос:\r\n" + exc.ToString();
                    logger.Error("Не удалось выполнить POST-запрос:\r\n" + exc.ToString());
                }
                finally
                {
                    IsWaiting = false;
                }
            }
        }

        public void SendNomenclatureRequest()
        {
            IsFilePath = false;
            IsWaiting = true;
            string address = new Uri(new Uri(ChosenSetting.BaseUrl), "nomenclature?contract=" + ValueNomenclatureContract).ToString();
            //logger.Info(address);
            try
            {
                GetHttpClient.Get().DefaultRequestHeaders.Clear();
                GetHttpClient.Get().DefaultRequestHeaders.TryAddWithoutValidation("Authorization", GetToken.Get(ChosenSetting).Value);
                using (var response = GetHttpClient.Get().GetAsync(address).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string resp = response.Content.ReadAsStringAsync().Result;
                        Nomenclature = JsonConvert.DeserializeObject<List<NomenclatureStu3>>(resp);
                        logger.Info($"Got nomenclature by contract {ValueNomenclatureContract}, nomenclature is null == {Nomenclature is null}");
                    }
                    else
                    {

                        throw new Exception(response.Content.ReadAsStringAsync().Result + "\n\n" + response.ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                ValueFindNomenclature = $"Error: {ex.ToString()}";
                logger.Error($"Try to get NOMENCLATURE STU3: {ex.ToString()}");
            }
            finally
            {
                IsWaiting = false;
            }
        }

        public void FindNomenclature()
        {
            FoundNomenclature = Nomenclature.Where(x =>
                                        x.Id.Contains(ValueFindNomenclature) |
                                        x.Caption.ToUpper().Contains(ValueFindNomenclature.ToUpper()) |
                                        x.LabId.Contains(ValueFindNomenclature) |
                                        x.LabCaption.ToUpper().Contains(ValueFindNomenclature.ToUpper())).ToList();
            SelectedFoundNomenclature = FoundNomenclature.Count == 0 ? null : FoundNomenclature[0];
        }

        public void SendNomenclatureRequestR4()
        {
            IsFilePath = false;
            IsWaitingR4 = true;
            string address = new Uri(new Uri(ChosenSetting.BaseUrl), "r4/fhir/catalog/" + ValueNomenclatureContractR4).ToString();
            logger.Info(address);
            try
            {
                GetHttpClient.Get().DefaultRequestHeaders.Clear();
                GetHttpClient.Get().DefaultRequestHeaders.TryAddWithoutValidation("Authorization", GetToken.Get(ChosenSetting).Value);
                GetHttpClient.Get().DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/fhir+json");
                using (var response = GetHttpClient.Get().GetAsync(address).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string resp = response.Content.ReadAsStringAsync().Result;
                        NomenclatureListR4 = NomenclatureR4.GetNomenclatureR4(resp);
                        logger.Info($"Got nomenclature R4 by contract {ValueNomenclatureContractR4}, nomenclature R4 is null == {NomenclatureListR4 is null}");
                    }
                    else
                    {
                        throw new Exception(response.Content.ReadAsStringAsync().Result + "\n\n" + response.ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                ValueFindNomenclatureR4 = $"Error: {ex}";
                logger.Error($"Try to get NOMENCLATURE R4: {ex}");
            }
            finally
            {
                IsWaitingR4 = false;
            }
        }

        public void FindNomenclatureR4()
        {
            FoundNomenclatureR4 = NomenclatureListR4.Where(x =>
                                        x.Hxid.Contains(ValueFindNomenclatureR4) |
                                        x.Caption.ToUpper().Contains(ValueFindNomenclatureR4.ToUpper()) |
                                        (x.Description?.ToUpper() ?? "").Contains(ValueFindNomenclatureR4.ToUpper())).ToList();
            SelectedFoundNomenclatureR4 = FoundNomenclatureR4.Count == 0 ? null : FoundNomenclatureR4[0];
        }

        public void GetOrderProcessingTaskByLabel()
        {
            IsWaiting = true;
            string orderProcessingTasksInTextForm = "";
            HashSet<string> procedureRequests = new HashSet<string>();
            HashSet<string> tasks = new HashSet<string>();
            HashSet<string> orderProcessingTasks = new HashSet<string>();
            string address = new Uri(new Uri(ChosenSetting.BaseUrl), "fhir/Specimen?container-id=" + LabelId.Trim()).ToString();
            try
            {
                GetHttpClient.Get().DefaultRequestHeaders.Clear();
                GetHttpClient.Get().DefaultRequestHeaders.TryAddWithoutValidation("Authorization", GetToken.Get(ChosenSetting).Value);
                using (var response = GetHttpClient.Get().GetAsync(address).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string specimenResponse = response.Content.ReadAsStringAsync().Result;
                        SpecimenResponse.Rootobject specimenResponseInJson = null;
                        try
                        {
                            specimenResponseInJson = JsonConvert.DeserializeObject<SpecimenResponse.Rootobject>(specimenResponse);
                        }
                        catch { }
                        if (specimenResponseInJson == null || specimenResponseInJson.entry == null)
                        {
                            AnswerText = $"Не найдено specimen по номеру ШК {LabelId}";
                            logger.Warn($"Не найдено specimen по номеру ШК {LabelId}");
                        }
                        else
                        {
                            procedureRequests = GetProcedureRequestsFromSpecimenResponseInJson(specimenResponseInJson);
                            if (procedureRequests.Count > 0)
                            {
                                tasks = GetTasksFromProcedureRequestIds(procedureRequests);
                                if (tasks.Count > 0)
                                {
                                    orderProcessingTasksInTextForm = GetOrderProcessingTasksInStringFromTaskReferences(tasks);
                                }
                                else
                                {
                                    orderProcessingTasksInTextForm = string.Empty;
                                    AnswerText = $"Не найдено Task";
                                    logger.Warn($"Не найдено Task");
                                }
                            }
                            else
                            {
                                AnswerText = $"Не найдено ProcedureRequest";
                                logger.Warn($"Не найдено ProcedureRequest");
                            }
                        }
                        string result = FormatJson.Process(orderProcessingTasksInTextForm);
                        if (result == string.Empty)
                        {
                            AnswerText = $"Не найдено task по номеру ШК {LabelId}";
                            logger.Warn($"Не найдено task по номеру ШК {LabelId}");
                        }
                        else
                        {
                            AnswerText = result;
                        }
                    }
                    else
                    {
                        throw new Exception(response.Content.ReadAsStringAsync().Result + "\n\n" + response.ToString());
                    }

                }
            }
            catch (Exception ex)
            {
                AnswerText = ex.ToString();
            }
            finally
            {
                IsWaiting = false;
            }

        }

        private HashSet<string> GetProcedureRequestsFromSpecimenResponseInJson(SpecimenResponse.Rootobject specimenResponseInJson)
        {
            HashSet<string> procedureRequests = new HashSet<string>();
            List<string> specimenIds = (from xentry in specimenResponseInJson.entry
                                        orderby xentry.response.lastModified descending
                                        select xentry.resource.id)?.Distinct().ToList();
            for (int i = 0; i < specimenIds.Count; i++)
            {
                string address = new Uri(new Uri(ChosenSetting.BaseUrl), "fhir/procedurerequest?specimen=specimen/" + specimenIds[i]).ToString();
                try
                {
                    GetHttpClient.Get().DefaultRequestHeaders.Clear();
                    GetHttpClient.Get().DefaultRequestHeaders.TryAddWithoutValidation("Authorization", GetToken.Get(ChosenSetting).Value);
                    using (var response = GetHttpClient.Get().GetAsync(address).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string procedureRequestResponse = response.Content.ReadAsStringAsync().Result;
                            ProcedureRequestResponse.Rootobject procedureRequestResponseInJson = null;
                            try
                            {
                                procedureRequestResponseInJson = JsonConvert.DeserializeObject<ProcedureRequestResponse.Rootobject>(procedureRequestResponse);
                            }
                            catch { }
                            //catch (Exception exc){ MessageBox.Show("Deserialize PR" + exc.Message); }
                            if (procedureRequestResponseInJson == null || procedureRequestResponseInJson.entry == null)
                                continue;
                            else
                            {
                                foreach (ProcedureRequestResponse.Entry xentry in procedureRequestResponseInJson.entry)
                                {
                                    try
                                    {
                                        procedureRequests.Add(xentry?.resource?.id);
                                    }
                                    catch { }
                                }
                            }
                        }
                        else
                        {
                            AnswerText = $"Ошибка в запросе ProcedureRequest";
                            logger.Warn($"Ошибка в запросе ProcedureRequest");
                        }
                    }
                }
                catch (Exception ex)
                {
                    AnswerText = $"Ошибка в запросе ProcedureRequest: {ex}";
                    logger.Warn($"Ошибка в запросе ProcedureRequest: {ex}");
                }
            }
            return procedureRequests;
        }

        private HashSet<string> GetTasksFromProcedureRequestIds(HashSet<string> procedureRequestIds)
        {
            HashSet<string> tasks = new HashSet<string>();
            foreach (string id in procedureRequestIds)
            {
                string address = new Uri(new Uri(ChosenSetting.BaseUrl), "fhir/task?basedOn.0.reference=procedureRequest/" + id).ToString();
                try
                {
                    GetHttpClient.Get().DefaultRequestHeaders.Clear();
                    GetHttpClient.Get().DefaultRequestHeaders.TryAddWithoutValidation("Authorization", GetToken.Get(ChosenSetting).Value);
                    using (var response = GetHttpClient.Get().GetAsync(address).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string taskResponse = response.Content.ReadAsStringAsync().Result;
                            TaskResponse.Rootobject taskResponseInJson = null;
                            try
                            {
                                taskResponseInJson = JsonConvert.DeserializeObject<TaskResponse.Rootobject>(taskResponse);
                            }
                            catch { }
                            if (taskResponseInJson == null || taskResponseInJson.entry == null)
                                continue;
                            else
                            {
                                foreach (TaskResponse.Entry xentry in taskResponseInJson.entry)
                                {
                                    try
                                    {
                                        tasks.Add(xentry?.resource?.partOf[0]?.reference);
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                }
                catch { }
            }
            return tasks;
        }

        private string GetOrderProcessingTasksInStringFromTaskReferences(HashSet<string> taskReferences)
        {
            StringBuilder orderProcessingTasksInString = new StringBuilder();
            foreach (string reference in taskReferences)
            {
                string address = new Uri(new Uri(ChosenSetting.BaseUrl), "fhir/" + reference).ToString();
                try
                {
                    GetHttpClient.Get().DefaultRequestHeaders.Clear();
                    GetHttpClient.Get().DefaultRequestHeaders.TryAddWithoutValidation("Authorization", GetToken.Get(ChosenSetting).Value);
                    using (var response = GetHttpClient.Get().GetAsync(address).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            orderProcessingTasksInString.AppendLine(FormatJson.Process(response.Content.ReadAsStringAsync().Result));
                        }
                    }
                }
                catch {}
            }
            return orderProcessingTasksInString.ToString();
        }
    }
}
