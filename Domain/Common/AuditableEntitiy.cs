namespace Domain.Common
{
    /// <summary>
    /// Pozwala na dodanie funkcjonalności audytu do modeli domenowych
    /// Dostarcza informacje kto i kiedy utworzył oraz modyfikował daną encję
    /// Klasa bazowa dziedziczona przez każdą inną klasę domenową, kiedy chcemy dodać funkcjonalność audytu
    /// </summary>
    public abstract class AuditableEntitiy
    {
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }
    }
}
