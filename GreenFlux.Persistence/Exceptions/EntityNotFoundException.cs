namespace GreenFlux.Persistence.Exceptions
{
    public class EntityNotFoundException : GreenFluxException
    {
        public EntityNotFoundException(string message) : base(message)
        {
        }
    }
}
