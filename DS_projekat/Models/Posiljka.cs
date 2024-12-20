namespace DS_projekat.Models
{
    public enum StatusPosiljke
    {
        NaPutu, Isporuceno, USkladistu
    }
    
    public class Posiljka
    {
        public Guid Id { get; set; }
        public string Naziv {  get; set; }
        public StatusPosiljke Status { get; set; }
        public DateTime DatumKreiranja { get; set; }
        public DateTime? DatumIsporuke { get; set; }
    }
}
