using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using WhiteLabel.Models;
using WhiteLabel.Services.Database;
using Xamarin.Essentials;

namespace WhiteLabel
{
    public class ChatMainViewModel : ObservableObject
    {
        private readonly Repository<PersonData> _repositoryPerson = Repository<PersonData>.Instance();
        public ObservableCollection<PersonDataView> Contacts { get; } = new ObservableCollection<PersonDataView>();

        private bool _isEmpty;
        public bool IsEmpty
        {
            get => _isEmpty;
            set
            {
                _isEmpty = value;
                NotifyPropertyChanged();
            }
        }

        public ChatMainViewModel()
        {
            Preferences.Set("firstTime", false);
        }

        public void ReloadData()
        {
            var docs = _repositoryPerson?.GetItems();
            var datas = (docs ?? throw new InvalidOperationException()).ToList();
            if (datas.Any())
            {
                IsEmpty = false;
                Contacts?.Clear();

                var vr = datas.Select(doc => new PersonDataView
                {
                    Id = doc.Id,
                    Name = doc.Name,
                    FullName = $"{doc.Name} {doc.ApellidoPaterno} {doc.ApellidoMaterno}",
                    DocumentType = doc.DocumentType,
                    Base64 = string.IsNullOrWhiteSpace(doc.FaceImage) ? doc.Avatar : doc.FaceImage,
                    CapturedDate = doc.CapturedDate
                }).ToList();

                var serialized = JsonConvert.SerializeObject(vr);
                JsonConvert.PopulateObject(serialized, Contacts);
            }
            else
            {
                IsEmpty = true;
            }
        }
    }
}
