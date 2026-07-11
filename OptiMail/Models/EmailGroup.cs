using System.Collections.ObjectModel;

namespace OptiMail.Models
{
    // CollectionView'in IsGrouped özelliğinin ihtiyaç duyduğu koleksiyon yapısı
    // Her grup, yapay zekanın belirlediği bir "kategori_basligi"na karşılık gelir
    public class EmailGroup : ObservableCollection<EmailMessage>
    {
        public string KategoriAdi { get; }

        public EmailGroup(string kategoriAdi, IEnumerable<EmailMessage> emails) : base(emails)
        {
            KategoriAdi = kategoriAdi;
        }
    }
}
