namespace Lextm.SharpSnmpLib.Agent
{
    internal class SysObjectId : IScalarObject
    {
        private static readonly ObjectIdentifier Identifier = new ObjectIdentifier("1.3.6.1.2.1.1.2.0");
        private readonly ObjectIdentifier _objectId = new ObjectIdentifier("1.3.6.1");

        public ISnmpData Data
        {
            get { return _objectId; }
            set { throw new AccessFailureException(); }
        }

        public ObjectIdentifier Id
        {
            get { return Identifier; }
        }
    }
}
