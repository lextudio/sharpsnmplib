namespace Lextm.SharpSnmpLib.Agent
{
    internal class SnmpApplicationFactory
    {
        private readonly Logger _logger = new Logger();
        private readonly ObjectStore _store = new ObjectStore();

        public SnmpApplication Create(SnmpContext context)
        {
            return new SnmpApplication(context, _logger, _store);
        }
    }
}