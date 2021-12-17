namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// Odpowiada za wyszukanie w projekcie wszystkich klas Dto
        /// Utworzenie ich instancji
        /// Wywołanie metody Mapping w przypadku użycia danego mapowania
        /// </summary>
        /// <param name="assembly"></param>
        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes().Where(x =>
                typeof(IMap).IsAssignableFrom(x) && !x.IsInterface).ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping");
                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }
}
